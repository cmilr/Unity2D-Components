using UnityEngine;
using System.Collections;
using Rewired;
using Matcha.Lib;

[RequireComponent(typeof(CharacterController2D))]


public class PlayerMovement : CacheBehaviour
{
	public int groundLine = -50;						// y coordinate where aboveground ends
	public float gravity = -35f;                        // set gravity for player
	public float runSpeed = 8f;                         // set player's run speed
	public float groundDamping = 20f;                   // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;                     // how fast do we change direction mid-air?
	public float jumpHeight = 2.6f;                     // player's jump height
	public float maxFallingSpeed = 100f;                // max falling speed, for throttling falls, etc
	public float maxRisingSpeed = 2f;                   // max rising speed, for throttling player on moving platforms, etc
	public bool FacingRight { get; private set; }

	private float previousX;							// previous update's x position, for horizontal movement comparisons
	private float previousY;                            // previous update's y position, for speed comparisons
	private float speedCheck = .08f;                    // compare against to see if we need to throttle rising speed
	private float h;                                    // input horizontal axis
	private bool ridingFastPlatform;                    // this is set in response to a message from MovingPlatform
	private bool touchingWall;
	private bool moveRight;
	private bool moveLeft;
	private bool jump;
	private RaycastHit2D lastControllerColliderHit;
	private Vector3 velocity;
	private Player playerControls;
	private CharacterController2D controller;
	private float normalizedHorizontalSpeed;


	void Start()
	{
		base.CacheComponents();
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

			FacingRight = true;

			moveRight = false;
		}
		else if (moveLeft)
		{
			normalizedHorizontalSpeed = -1;

			if (transform.localScale.x > 0f)
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			if (controller.isGrounded)
				animator.Play(Animator.StringToHash("Run"));

			FacingRight = false;

			moveLeft = false;
		}
		else if (controller.isGrounded)
		{
			normalizedHorizontalSpeed = 0;

			if (controller.isGrounded)
				animator.Play(Animator.StringToHash("Idle"));
		}
		
		if (MLib2D.Equals(transform.position.x, previousX) && touchingWall)
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

		CheckPlayerLocation();

		SaveCurrentPosition();
	}

	bool MovingTooFast()
	{
		return transform.position.y - previousY > speedCheck;
	}

	void ApplyGravity()
	{
		velocity.y += gravity * Time.deltaTime;
	}

	void ClampYMovement()
	{
		// clamp to maxRisingSpeed to eliminate jitteriness when rising too fast,
		// otherwise, clamp to maxFallingSpeed to prevent player leaving screen
		if (ridingFastPlatform && MovingTooFast()) 
		{
			velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxRisingSpeed);
		} 
		else 
		{
			velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxFallingSpeed);
		}
	}

	void CheckPlayerLocation()
	{
		if (transform.position.y > groundLine)
			Messenger.Broadcast<bool>("player above ground", true);
		else 
			Messenger.Broadcast<bool>("player above ground", false);
	}

	void SaveCurrentPosition()
	{
		previousX = transform.position.x;
		previousY = transform.position.y;
	}

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<bool>( "touching wall", OnTouchingWall);
		Messenger.AddListener<bool>( "riding fast platform", OnRidingFastPlatform);
		Messenger.AddListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>( "touching wall", OnTouchingWall);		
		Messenger.RemoveListener<bool>( "riding fast platform", OnRidingFastPlatform);
		Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	// EVENT RESPONDERS
	void OnRidingFastPlatform(bool status)
	{
		ridingFastPlatform = status;
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		this.enabled = false;
	}

	void OnTouchingWall(bool status)
	{
		touchingWall = status;
	}
}
