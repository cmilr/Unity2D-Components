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
	private ArmAnimation arm;

    private string idleAnimation;
    private string runAnimation;
    private string jumpAnimation;
    private string swingAnimation;
	private enum AnimationAction { Idle, Run, Jump, Fall, Attack, Defend, RunAttack };
	private AnimationAction animationAction;


	void Start()
	{
		state = GetComponent<IPlayerStateFullAccess>();
		controller = GetComponent<CharacterController2D>();
		weapon = GetComponentInChildren<IWeapon>();
		arm = GetComponentInChildren<ArmAnimation>();
		SetCharacterAnimations(character);
	}

	// set animations depending on which character is chosen
	void SetCharacterAnimations(string character)
	{
		idleAnimation = character + "_Idle";
		runAnimation = character + "_Run";
		jumpAnimation = character + "_Jump";
		swingAnimation = character + "_Swing";
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
		InitializeVelocity();

		CheckIfStandingOrFalling();

		// attack state
		if (attack)
		{
			if (moveRight)
			{
				MovePlayerRight();
				AttackWhileRunning();
			}
			else if (moveLeft)
			{
				MovePlayerLeft();
				AttackWhileRunning();
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

		ComputeMovement();

		ApplyGravity();

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
		}
		else
		{
			animationAction = AnimationAction.Fall;
		}
	}

	void MovePlayerRight()
	{
		normalizedHorizontalSpeed = 1;

		if (transform.localScale.x < 0f)
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

		if (controller.isGrounded)
		{
			animationAction = AnimationAction.Run;
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
			animationAction = AnimationAction.Run;
		}

		moveLeft = false;

		state.FacingRight = false;
	}

	void AttackWhileIdle()
	{
		if (controller.isGrounded)
		{
			animationAction = AnimationAction.Attack;
			normalizedHorizontalSpeed = 0;
		}

		attack = false;
	}

	void AttackWhileRunning()
	{
		if (controller.isGrounded)
		{
			animationAction = AnimationAction.RunAttack;
		}

		attack = false;
	}

	void PlayerGrounded()
	{
		normalizedHorizontalSpeed = 0;

		if (controller.isGrounded)
		{
			animationAction = AnimationAction.Idle;
		}
	}

	void PlayerJump()
	{
		velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

		animationAction = AnimationAction.Jump;

		jump = false;
	}

	void PlayAnimation()
	{
		switch (animationAction)
		{
			case AnimationAction.Idle:
			{
				animator.speed = IDLE_SPEED;
				animator.Play(Animator.StringToHash(idleAnimation));
				arm.PlayIdleAnimation();
				weapon.PlayIdleAnimation();
				break;
			}

			case AnimationAction.Run:
			{
				animator.speed = RUN_SPEED;
				animator.Play(Animator.StringToHash(runAnimation));
				arm.PlayRunAnimation();
				weapon.PlayRunAnimation();
				break;
			}

			case AnimationAction.Jump:
			{
				animator.speed = JUMP_SPEED;
				animator.Play(Animator.StringToHash(jumpAnimation));
				arm.PlayJumpAnimation();
				weapon.PlayJumpAnimation();
				break;
			}

			case AnimationAction.Fall:
			{
				animator.speed = JUMP_SPEED;
				animator.Play(Animator.StringToHash(jumpAnimation));
				arm.PlayJumpAnimation();
				weapon.PlayJumpAnimation();
				break;
			}

			case AnimationAction.Attack:
			{
				animator.speed = SWING_SPEED;
				animator.Play(Animator.StringToHash(swingAnimation));
				arm.PlaySwingAnimation();
				weapon.PlaySwingAnimation();
				break;
			}

			case AnimationAction.RunAttack:
			{
				animator.speed = RUN_SPEED;
				animator.Play(Animator.StringToHash(runAnimation));
				arm.PlaySwingAnimation();
				weapon.PlaySwingAnimation();
				arm.OffsetY(ONE_PIXEL);
				weapon.OffsetY(ONE_PIXEL);
				break;
			}

			default:
			{
				Debug.Log("ERROR: No animationAction was set in PlayerMovement.cs >> PlayAnimation()");
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
		velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
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
