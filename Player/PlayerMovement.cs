using UnityEngine;
using System.Collections;
using Rewired;
using Matcha.Lib;

[RequireComponent(typeof(CharacterController2D))]


public class PlayerMovement : CacheBehaviour
{
	public float gravity = -35f;                        // set gravity for player
	public float runSpeed = 8f;                         // set player's run speed
	public float groundDamping = 20f;                   // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;                     // how fast do we change direction mid-air?
	public float jumpHeight = 2.6f;                     // player's jump height
	public float maxFallingSpeed = 100f;                // max falling speed, for throttling falls, etc
	public float maxRisingSpeed = 2f;                   // max rising speed, for throttling player on moving platforms, etc

	private float previousX;							// previous update's x position, for horizontal movement comparisons
	private float previousY;                            // previous update's y position, for speed comparisons
	private float speedCheck = .08f;                    // compare against to see if we need to throttle rising speed
	private float h;                                    // input horizontal axis
	private bool moveRight;
	private bool moveLeft;
	private bool jump;
	private RaycastHit2D lastControllerColliderHit;
	private Vector3 velocity;
	private Player playerControls;
	private CharacterController2D controller;
	private PlayerState state;
	private float normalizedHorizontalSpeed;


	void Start()
	{
		state = GetComponent<PlayerState>();
		controller = GetComponent<CharacterController2D>();
		playerControls = ReInput.players.GetPlayer(0);
	}

	void Update()
	{
		h = playerControls.GetAxisRaw("Move Horizontal");

		if (h > 0)
			moveRight = true; 

		if (h < 0)
			moveLeft = true; 

		if (playerControls.GetButtonDown("Jump") && controller.isGrounded)
			jump = true; 			
	}

	void LateUpdate()
	{
		// keep movement in LateUpdate() to prevent falling through edge colliders
		gravity = -35f;

		velocity = controller.velocity;

		if (controller.isGrounded)
		{
			velocity.y = 0;
		}
		else
		{
			animator.Play(Animator.StringToHash("Jump"));
		}

		if (moveRight)
		{
			normalizedHorizontalSpeed = 1;
			
			if (transform.localScale.x < 0f)
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			if (controller.isGrounded)
				animator.Play(Animator.StringToHash("Run"));

			moveRight = false;

			state.FacingRight = true;
		}
		else if (moveLeft)
		{
			normalizedHorizontalSpeed = -1;

			if (transform.localScale.x > 0f)
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			if (controller.isGrounded)
				animator.Play(Animator.StringToHash("Run"));

			moveLeft = false;

			state.FacingRight = false;
		}
		else if (controller.isGrounded)
		{
			normalizedHorizontalSpeed = 0;

			if (controller.isGrounded)
				animator.Play(Animator.StringToHash("Idle"));
		}
		
		if (MLib2D.Equals(transform.position.x, previousX) && state.TouchingWall)
		{
			// flush horizontal axis if player is falling or jumping while pressed against a wall
			normalizedHorizontalSpeed = 0;
			velocity.x = 0f;
		}

		if (jump)
		{
			velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
			animator.Play(Animator.StringToHash("Jump"));
			jump = false;
		}
		
		// compute x and y movements
		var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;
		velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

		ApplyGravity();

		ClampYMovement();

		controller.move(velocity * Time.deltaTime);

		SaveCurrentPosition();
	}

	bool MovingTooFast()
	{
		return transform.position.y - previousY > speedCheck;
	}

	void ApplyGravity()
	{
		if (!moveLeft && !moveRight)
			velocity.y += gravity * Time.deltaTime;
	}

	void ClampYMovement()
	{
		// clamp to maxRisingSpeed to eliminate jitteriness when rising too fast,
		// otherwise, clamp to maxFallingSpeed to prevent player leaving screen
		if (MovingTooFast() && state.RidingFastPlatform) 
		{
			velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxRisingSpeed);
		} 
		else 
		{
			velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxFallingSpeed);
		}
	}

	void SaveCurrentPosition()
	{
		previousX = transform.position.x;
		previousY = transform.position.y;
	}

	void OnPlayerDead(string methodOfDeath, bool alreadyDead, Collider2D coll)
	{
		// pass control of movement to DeathHandler
		if (!alreadyDead)
			this.enabled = false;
	}

	void OnEnable()
	{
		Messenger.AddListener<string, bool, Collider2D>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, bool, Collider2D>( "player dead", OnPlayerDead);
	}
}
