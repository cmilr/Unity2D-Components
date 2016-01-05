using Matcha.Unity;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine;

public class PlayerMovement : CacheBehaviour, ICreatureController
{
	private float gravity         = -35f;           // set gravity for player
	private float runSpeed        = 7f;             // set player's run speed
	private float groundDamping   = 20f;            // how fast do we change direction? higher means faster
	private float inAirDamping    = 5f;             // how fast do we change direction mid-air?
	private float jumpHeight      = 3.50f;          // player's jump height
	private float maxFallingSpeed = 100f;           // max falling speed, for throttling falls, etc
	private float maxRisingSpeed  = 2f;             // max rising speed, for throttling player on moving platforms, etc
	private float speedCheck      = .1f;            // compare against to see if we need to throttle rising speed
	private float normalizedHorizontalSpeed;
	private float previousX;
	private float previousY;
	private float repulseVelocity;
	private bool facingRight;
	private bool moveRight;
	private bool moveLeft;
	private bool jump;
	private bool attack;
	private bool repulseRight;
	private bool repulseLeft;
	private bool ridingFastPlatform;
	private bool movingHorizontally;
	private bool touchingWall;
	private bool jumpedFromFastPlatform;
	private Vector3 velocity;
	private RaycastHit2D lastControllerColliderHit;
	private CharacterController2D controller;
	private Animator anim;
	private WeaponManager weaponManager;

	void Start()
	{
		controller = GetComponent<CharacterController2D>();
		weaponManager = GetComponentInChildren<WeaponManager>();
		anim = GameObject.Find("Player").GetComponent<Animator>();

		InvokeRepeating("BroadcastCurrentPosition", .3f, .3f);
	}

	// input methods required by ICreatureController
	public void MoveRight()
	{
		moveRight = true;
	}

	public void MoveLeft()
	{
		moveLeft = true;
	}

	public void Jump()
	{
		jump = true;
	}

	public void Attack()
	{
		attack = true;
	}

	//main movement loop
	//keep in LateUpdate() to prevent player falling through edge colliders
	void LateUpdate()
	{
		InitializeVelocity();

		CheckIfStandingOrFalling();

		if (!attack)
		{
			if (moveRight)                         //run right
			{
				MovePlayerRight();
			}
			else if (moveLeft)                     //run left
			{
				MovePlayerLeft();
			}
			else if (controller.isGrounded)        //idle
			{
				PlayerIdle();
			}

			if (jump && controller.isGrounded)     //jump
			{
				PlayerJump();
			}
		}
		else
		{
			if (moveRight)                         //attack while running right
			{
				MovePlayerRight();
				AttackWhileRunning();
			}
			else if (moveLeft)                     //attack while running left
			{
				MovePlayerLeft();
				AttackWhileRunning();
			}
			else if (controller.isGrounded)        //attack while idle
			{
				AttackWhileIdle();
			}

			if (!controller.isGrounded)            //attack while jumping
			{
				AttackWhileJumping();
			}
		}

		CheckForFreefall();

		ComputeMovement();

		ApplyGravity();

		ComputeRepulse();

		ClampYMovement();

		ApplyMovement();

		SavePreviousPosition();
	}

	void CheckIfStandingOrFalling()
	{
		if (controller.isGrounded)       //player on solid ground
		{
			velocity.y = 0;

			anim.SetBool("jump", false);

			if (jumpedFromFastPlatform) {
				jumpedFromFastPlatform = false;
				EventKit.Broadcast<bool>("player jumped from fast platform", false);
			}
		}
		else                             //player jumping or falling
		{
			anim.SetBool("jump", true);
		}
	}

	void PlayerIdle()
	{                                   //player idle
		normalizedHorizontalSpeed = 0;

		anim.SetBool("jump", false);
		anim.SetBool("run", false);
		anim.SetBool("attack", false);
	}

	void PlayerJump()
	{
		velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

		anim.SetBool("jump", true);

		jump = false;

		if (ridingFastPlatform && movingHorizontally)
		{
			if (!jumpedFromFastPlatform) {
				jumpedFromFastPlatform = true;
				EventKit.Broadcast<bool>("player jumped from fast platform", true);
			}
		}
	}

	void MovePlayerRight()
	{
		normalizedHorizontalSpeed = 1;

		if (transform.localScale.x < 0f)
		{
			// reverse sprite direction
			transform.SetLocalScaleX(-transform.localScale.x);

			// offset so player isn't pushed too far forward when sprite flips
			transform.SetPositionX(transform.position.x - ABOUTFACE_OFFSET);
		}

		if (controller.isGrounded)       //player running right
		{
			anim.SetBool("run", true);
		}
		else                             //player flying right
		{
			anim.SetBool("run", false);
		}

		anim.SetBool("attack", false);

		if (!facingRight)
		{
			facingRight = true;
			BroadcastMessage("OnFacingRight", true);
		}

		moveRight = false;
	}

	void MovePlayerLeft()
	{
		normalizedHorizontalSpeed = -1;

		if (transform.localScale.x > 0f)
		{
			// reverse sprite direction
			transform.SetLocalScaleX(-transform.localScale.x);

			// offset so player isn't pushed too far forward when sprite flips
			transform.SetPositionX(transform.position.x + ABOUTFACE_OFFSET);
		}

		if (controller.isGrounded)       //player running left
		{
			anim.SetBool("run", true);
		}
		else                             //player flying left
		{
			anim.SetBool("run", false);
		}

		anim.SetBool("attack", false);

		// only broadcast message once, each time player turns
		if (facingRight)
		{
			facingRight = false;
			BroadcastMessage("OnFacingRight", false);
		}

		moveLeft = false;
	}

	void AttackWhileIdle()
	{
		if (controller.isGrounded)
		{
			normalizedHorizontalSpeed = 0;
			anim.SetBool("attack", true);
			anim.SetBool("run", false);
			weaponManager.Attack();
		}

		attack = false;
	}

	void AttackWhileRunning()
	{
		if (controller.isGrounded)
		{
			anim.SetBool("attack", true);
			anim.SetBool("run", true);
			weaponManager.Attack();
		}

		attack = false;
	}

	void AttackWhileJumping()
	{
		anim.SetBool("jump", true);
		anim.SetBool("attack", true);
		weaponManager.Attack();

		jump = false;

		attack = false;
	}

	// void AttackWhileJumpingBUG()
	// {
	//    velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

	//    action = Action.JumpAttack;

	//    jump = false;

	//    attack = false;
	// }

	void InitializeVelocity()
	{
		velocity = controller.velocity;
	}

	void CheckForFreefall()
	{
		// flush horizontal axis if player is falling while pressed against a wall
		if (touchingWall && !controller.isGrounded)
		{
			normalizedHorizontalSpeed = 0;
			velocity.x = 0f;
		}
	}

	bool MovingTooFast()
	{
		return transform.position.y - previousY > speedCheck;
	}

	void ApplyGravity()
	{
		velocity.y += gravity * Time.deltaTime;
	}

	void ComputeRepulse()
	{
		if (repulseLeft)
		{
			velocity.x = -repulseVelocity;
			velocity.y = 0f;
		}
		else if (repulseRight)
		{
			velocity.x = repulseVelocity;
			velocity.y = 0f;
		}
	}

	void ClampYMovement()
	{
		// clamp to maxRisingSpeed to eliminate jitteriness when rising too fast,
		// otherwise, clamp to maxFallingSpeed to prevent player leaving screen
		if (MovingTooFast() && ridingFastPlatform && !movingHorizontally)
		{
			velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxRisingSpeed);
		}
		else
		{
			velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxFallingSpeed);
		}
	}

	void ApplyMovement()
	{
		controller.move(velocity * Time.deltaTime);
	}

	void BroadcastCurrentPosition()
	{
		EventKit.Broadcast<float, float>("player position", transform.position.x, transform.position.y);
	}

	void ComputeMovement()
	{
		// compute x and y movements
		var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;

		velocity.x = Mathf.Lerp(
			velocity.x,
			normalizedHorizontalSpeed * runSpeed,
			Time.deltaTime * smoothedMovementFactor
			);
	}

	void SavePreviousPosition()
	{
		if (previousX != transform.position.x)
		{
			movingHorizontally = true;
		}
		else
		{
			movingHorizontally = false;
		}

		previousX = transform.position.x;
		previousY = transform.position.y;
	}

	void RepulseToLeft(float maxVelocity)
	{
		repulseLeft = true;
		repulseVelocity = Rand.Range(2f, maxVelocity);
		StartCoroutine(RepulseTimer());
	}

	void RepulseToRight(float maxVelocity)
	{
		repulseRight = true;
		repulseVelocity = Rand.Range(2f, maxVelocity);
		StartCoroutine(RepulseTimer());
	}

	IEnumerator RepulseTimer()
	{
		yield return new WaitForSeconds(Rand.Range(.1f, .3f));
		repulseLeft = false;
		repulseRight = false;
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
	{
		this.enabled = false;
	}

	void OnPlayerRidingFastPlatform(bool status)
	{
		ridingFastPlatform = status;
	}

	void OnPlayerTouchingWall(bool status)
	{
		touchingWall = status;
	}

	void OnEnable()
	{
		EventKit.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
		EventKit.Subscribe<bool>("player riding fast platform", OnPlayerRidingFastPlatform);
		EventKit.Subscribe<bool>("player touching wall", OnPlayerTouchingWall);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
		EventKit.Unsubscribe<bool>("player riding fast platform", OnPlayerRidingFastPlatform);
		EventKit.Unsubscribe<bool>("player touching wall", OnPlayerTouchingWall);
	}
}
