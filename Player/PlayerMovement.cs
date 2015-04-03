using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(CharacterController2D))]


public class PlayerMovement : CacheBehaviour, ICreatureController
{
	public float gravity         = -35f;                // set gravity for player
	public float runSpeed        = 8f;                  // set player's run speed
	public float groundDamping   = 20f;                 // how fast do we change direction? higher means faster
	public float inAirDamping    = 5f;                  // how fast do we change direction mid-air?
	public float jumpHeight      = 2.6f;                // player's jump height
	public float maxFallingSpeed = 100f;                // max falling speed, for throttling falls, etc
	public float maxRisingSpeed  = 2f;                  // max rising speed, for throttling player on moving platforms, etc
	private float speedCheck     = .08f;                // compare against to see if we need to throttle rising speed

	private float normalizedHorizontalSpeed;
	private float previousX;
	private float previousY;
	private bool moveRight;
	private bool moveLeft;
	private bool jump;
	private bool attack;
	private bool defend;
	private RaycastHit2D lastControllerColliderHit;
	private Vector3 velocity;
	private CharacterController2D controller;
	private IPlayerStateFullAccess state;
	private IWeapon weapon;

    private string character;
    private string idleAnimation;
    private string runAnimation;
    private string jumpAnimation;
    private string swingAnimation;
	private enum Action {Idle, Run, Jump, Fall, Attack, Defend};
	private Action action;


	void Start()
	{
		state = GetComponent<IPlayerStateFullAccess>();
		controller = GetComponent<CharacterController2D>();
		weapon = GetComponentInChildren<IWeapon>();
		SetCharacterAnimations("LAURA");
	}

	// set animations depending on which character is chosen
	void SetCharacterAnimations(string character)
	{
		idleAnimation = character + "_Idle";
		runAnimation = character + "_Run";
		jumpAnimation = character + "_Jump";
		swingAnimation = character + "_Swing";
	}

	// required methods for ICreatureController
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
		if (controller.isGrounded)
			jump = true;
    }

    public void Attack()
    {
    	attack = true;
    }

    public void Defend()
    {
    	defend = true;
    }

    // main movement loop — keep in LateUpdate() to prevent player falling through edge colliders
	void LateUpdate()
	{
		velocity = controller.velocity;

		CheckIfStandingOrFalling();

		// attack state
		if (attack)
		{
			if (moveRight)
			{
				MovePlayerRight();
				AttackWhileMoving();
			}
			else if (moveLeft)
			{
				MovePlayerLeft();
				AttackWhileMoving();
			}
			else
			{
				AttackWhileIdle();
			}
		}

		// movement state
		else if (moveRight)
		{
			MovePlayerRight();
		}
		else if (moveLeft)
		{
			MovePlayerLeft();
		}

		// idle state
		else if (controller.isGrounded)
		{
			PlayerGrounded();
		}

		// jump state
		if (jump)
		{
			PlayerJump();
		}

		CheckForFreefall();

		PlayAnimation();

		SaveCurrentPosition();

		// compute x and y movements
		var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;
		velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

		ApplyGravity();

		ClampYMovement();

		controller.move(velocity * Time.deltaTime);

		SavePreviousPosition();
	}

	void CheckIfStandingOrFalling()
	{
		if (controller.isGrounded)
		{
			velocity.y = 0;
			state.Grounded = true;
		}
		else
		{
			action = Action.Fall;
		}
	}

	void MovePlayerRight()
	{
		normalizedHorizontalSpeed = 1;

		if (transform.localScale.x < 0f)
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

		if (controller.isGrounded)
		{
			action = Action.Run;
		}

		moveRight = false;

		state.FacingRight = true;
	}

	void MovePlayerLeft()
	{
		normalizedHorizontalSpeed = -1;

		if (transform.localScale.x > 0f)
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}

		if (controller.isGrounded)
		{
			action = Action.Run;
		}

		moveLeft = false;

		state.FacingRight = false;
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

	void AttackWhileMoving()
	{
		if (controller.isGrounded)
		{
			action = Action.Attack;
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
	}

	void PlayerJump()
	{
		velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

		action = Action.Jump;

		jump = false;
	}

	void PlayAnimation()
	{
		switch (action)
		{
			case Action.Idle:
			{
				animator.speed = IDLE_SPEED;
				animator.Play(Animator.StringToHash(idleAnimation));
				weapon.PlayIdleAnimation();
				break;
			}

			case Action.Run:
			{
				animator.speed = RUN_SPEED;
				animator.Play(Animator.StringToHash(runAnimation));
				weapon.PlayRunAnimation();
				break;
			}

			case Action.Jump:
			{
				animator.speed = JUMP_SPEED;
				animator.Play(Animator.StringToHash(jumpAnimation));
				weapon.PlayJumpAnimation();
				break;
			}

			case Action.Fall:
			{
				animator.speed = JUMP_SPEED;
				animator.Play(Animator.StringToHash(jumpAnimation));
				weapon.PlayJumpAnimation();
				break;
			}

			case Action.Attack:
			{
				animator.speed = SWING_SPEED;
				animator.Play(Animator.StringToHash(swingAnimation));
				weapon.PlaySwingAnimation();
				break;
			}

			default:
			{
				Debug.Log("ERROR: No action was set in PlayerMovement.cs >> PlayAnimation()");
				break;
			}
		}
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
		state.X = transform.position.x;
		state.Y = transform.position.y;
	}

	void SavePreviousPosition()
	{
		previousX = transform.position.x;
		previousY = transform.position.y;
		state.PreviousX = previousX;
		state.PreviousY = previousY;
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		this.enabled = false;
	}

	void OnEnable()
	{
		Messenger.AddListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
	}
}
