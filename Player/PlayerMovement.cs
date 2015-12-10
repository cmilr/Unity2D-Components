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
	private string idleAnimation;
	private string runAnimation;
	private string jumpAnimation;
	private string attackAnimation;
	private Vector3 velocity;
	private RaycastHit2D lastControllerColliderHit;
	private CharacterController2D controller;
	private IPlayerStateFullAccess state;
	private Animator anim;
	private WeaponManager weaponManager;
	private enum Action { Idle, Run, Jump, Fall, Attack, Defend, RunAttack, JumpAttack };
	private Action action;

	void Start()
	{
		state = GetComponent<IPlayerStateFullAccess>();
		controller = GetComponent<CharacterController2D>();
		weaponManager = GetComponentInChildren<WeaponManager>();
		anim = GameObject.Find("BodyParts").GetComponent<Animator>();
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
		if (controller.isGrounded) {
			jump = true;
		}
	}

	public void Attack()
	{
		attack = true;
	}

	// main movement loop — keep in LateUpdate() to prevent player falling through edge colliders
	void LateUpdate()
	{
		InitializeVelocity();

		CheckIfStandingOrFalling();

		// // attack state
		// if (attack)
		// {
		// 	if (moveRight)
		// 	{
		// 		MovePlayerRight();
		// 		AttackWhileRunning();
		// 	}
		// 	else if (moveLeft)
		// 	{
		// 		MovePlayerLeft();
		// 		AttackWhileRunning();
		// 	}
		// 	else if (controller.isGrounded)
		// 	{
		// 		AttackWhileIdle();
		// 	}
		//
		// 	if (!controller.isGrounded)
		// 	{
		// 		AttackWhileJumping();
		// 	}
		// }

		// grounded state
		if (controller.isGrounded)
		{
			PlayerGrounded();
			anim.SetBool("jump", false);
		}
		else
		{
			anim.SetBool("jump", true);
		}

		// movement state
		if (moveRight)
		{
			MovePlayerRight();
			anim.SetBool("run", true);
		}
		else if (moveLeft)
		{
			MovePlayerLeft();
			anim.SetBool("run", true);
		}
		else	// if not moving right or left
		{
			anim.SetBool("run", false);
		}

		// jump state
		if (jump)
		{
			PlayerJump();
			anim.SetBool("jump", true);
		}

		CheckForFreefall();

		ActionDispatcher();

		SaveCurrentPosition();

		ComputeMovement();

		ApplyGravity();

		ComputeRepulse();

		ClampYMovement();

		ApplyMovement();

		SavePreviousPosition();
	}

	void CheckIfStandingOrFalling()
	{
		if (controller.isGrounded)
		{
			velocity.y = 0;
			state.Grounded = true;
			state.JumpedFromFastPlatform = false;
		}
		// falling state
		else
		{
			action = Action.Fall;
			state.Grounded = false;
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

		if (controller.isGrounded)
		{
			action = Action.Run;
		}

		// only broadcast message once, each time player turns
		if (!facingRight)
		{
			facingRight = true;
			state.FacingRight = true;
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

		if (controller.isGrounded)
		{
			action = Action.Run;
		}

		// only broadcast message once, each time player turns
		if (facingRight)
		{
			facingRight = false;
			state.FacingRight = false;
			BroadcastMessage("OnFacingRight", false);
		}

		moveLeft = false;
	}

	void AttackWhileIdle()
	{
		if (controller.isGrounded)
		{
			action = Action.Attack;
			normalizedHorizontalSpeed = 0;
		}

		attack = false;
	}

	void AttackWhileRunning()
	{
		if (controller.isGrounded)
		{
			action = Action.RunAttack;
		}

		attack = false;
	}

	void PlayerGrounded()
	{
		normalizedHorizontalSpeed = 0;

		if (controller.isGrounded)
		{
			action = Action.Idle;
		}

		state.Grounded = true;
		state.JumpedFromFastPlatform = false;
	}

	void PlayerJump()
	{
		velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

		action = Action.Jump;

		jump = false;

		state.Grounded = false;

		if (state.RidingFastPlatform && state.MovingHorizontally)
		{
			state.JumpedFromFastPlatform = true;
		}
	}

	void AttackWhileJumping()
	{
		action = Action.JumpAttack;

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

	// mix & match animations for various activity states,
	// and pass instructions on to WeaponManager
	void ActionDispatcher()
	{
		switch (action)
		{
			case Action.Idle:
			{
				weaponManager.ActionDispatcher(IDLE);
				break;
			}

			case Action.Run:
			{
				weaponManager.ActionDispatcher(RUN);
				break;
			}

			case Action.Jump:
			{
				weaponManager.ActionDispatcher(JUMP);
				break;
			}

			case Action.Fall:
			{
				weaponManager.ActionDispatcher(FALL);
				break;
			}

			case Action.Attack:
			{
				weaponManager.ActionDispatcher(ATTACK);
				break;
			}

			case Action.RunAttack:
			{
				weaponManager.ActionDispatcher(RUN_ATTACK);
				break;
			}

			case Action.JumpAttack:
			{
				weaponManager.ActionDispatcher(JUMP_ATTACK);
				break;
			}

			default:
			{
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
			}
		}
	}

	void InitializeVelocity()
	{
		velocity = controller.velocity;
	}

	void CheckForFreefall()
	{
		// flush horizontal axis if player is falling while pressed against a wall
		if (state.TouchingWall && !controller.isGrounded)
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
		if (MovingTooFast() && state.RidingFastPlatform && !state.MovingHorizontally)
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

	void SaveCurrentPosition()
	{
		state.X = transform.position.x;
		state.Y = transform.position.y;
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
			state.MovingHorizontally = true;
		}
		else
		{
			state.MovingHorizontally = false;
		}

		previousX = transform.position.x;
		previousY = transform.position.y;
		state.PreviousX = previousX;
		state.PreviousY = previousY;
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

	void OnEnable()
	{
		Messenger.AddListener<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, Collider2D, int>("player dead", OnPlayerDead);
	}
}
