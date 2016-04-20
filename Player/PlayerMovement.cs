using Matcha.Unity;
using System.Collections;
using UnityEngine;

public class PlayerMovement : CacheBehaviour, ICreatureController
{
	private float gravity         = -35f;           //set gravity for player
	private float runSpeed        = 7f;             //set player's run speed
	private float groundDamping   = 20f;            //how fast do we change direction? higher means faster
	private float inAirDamping    = 5f;             //how fast do we change direction mid-air?
	private float jumpHeight      = 3.50f;          //player's jump height
	private float maxFallingSpeed = 100f;           //max falling speed, for throttling falls, etc
	private float jumpModifier    = 30f;            //increase jump height if traveling upwards, platforms etc
	private float yVelocity;
	private float normalizedHorizontalSpeed;
	private float repulseVelocity;
	private float previousY;
	private bool facingRight;
	private bool moveRight;
	private bool moveLeft;
	private bool jump;
	private bool attack;
	private bool repulseRight;
	private bool repulseLeft;
	private Vector3 velocity;
	private RaycastHit2D lastControllerColliderHit;
	private CharacterController2D controller;
	private WeaponManager weaponManager;

	void Start()
	{
		controller    = GetComponent<CharacterController2D>();
		weaponManager = GetComponentInChildren<WeaponManager>();
	}

	//input methods required by ICreatureController
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
	//keep in LateUpdate() to prevent player falling through edge colliders, bouncing, etc
	void LateUpdate()
	{
		GetYVelocity();

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

		ComputeMovement();

		ApplyGravity();

		ComputeRepulse();

		ClampYMovement();

		ApplyMovement();

		SavePreviousPosition();
	}

	void GetYVelocity()
	{
		if (transform.position.y > previousY)
		{
			yVelocity = transform.position.y - previousY;
		}
		else
		{
			yVelocity = 0f;
		}
	}

	void CheckIfStandingOrFalling()
	{
		//player grounded
		if (controller.isGrounded)
		{
			velocity.y = 0;

			animator.SetBool("jump", false);
		}
		else
		{
			//player jumping or falling
			animator.SetBool("jump", true);
			animator.SetBool("attack", false);
		}
	}

	void PlayerIdle()
	{
		normalizedHorizontalSpeed = 0;

		animator.SetBool("jump", false);
		animator.SetBool("run", false);
		animator.SetBool("attack", false);
	}

	void PlayerJump()
	{
		velocity.y = Mathf.Sqrt(2f * (jumpHeight +
						(yVelocity * jumpModifier)) *    //jump higher if already traveling upwards
						-gravity);

		animator.SetBool("jump", true);

		jump = false;
	}

	void MovePlayerRight()
	{
		normalizedHorizontalSpeed = 1;

		if (transform.localScale.x < 0f)
		{
			//reverse sprite direction
			transform.SetLocalScaleX(-transform.localScale.x);

			//offset so player isn't pushed too far forward when sprite flips
			transform.SetPositionX(transform.position.x - ABOUTFACE_OFFSET);
		}

		if (controller.isGrounded)
		{
			//player running right
			animator.SetBool("run", true);
		}
		else
		{
			//player flying right
			animator.SetBool("run", false);
		}

		animator.SetBool("attack", false);

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
			//reverse sprite direction
			transform.SetLocalScaleX(-transform.localScale.x);

			//offset so player isn't pushed too far forward when sprite flips
			transform.SetPositionX(transform.position.x + ABOUTFACE_OFFSET);
		}

		if (controller.isGrounded)
		{
			//player running left
			animator.SetBool("run", true);
		}
		else
		{
			//player flying left
			animator.SetBool("run", false);
		}

		animator.SetBool("attack", false);

		//only broadcast message once, each time player turns
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
			animator.SetBool("attack", true);
			animator.SetBool("run", false);
			weaponManager.Attack();
		}

		attack = false;
	}

	void AttackWhileRunning()
	{
		if (controller.isGrounded)
		{
			animator.SetBool("attack", true);
			animator.SetBool("run", true);
			weaponManager.Attack();
		}

		attack = false;
	}

	void AttackWhileJumping()
	{
		animator.SetBool("jump", true);
		animator.SetBool("attack", true);
		weaponManager.Attack();

		jump = false;

		attack = false;
	}

	//void AttackWhileJumpingBUG()
	//{
	//velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

	//action = Action.JumpAttack;

	//jump = false;

	//attack = false;
	//}

	void InitializeVelocity()
	{
		velocity = controller.velocity;
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
		velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxFallingSpeed);
	}

	void ComputeMovement()
	{
		var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;

		velocity.x = Mathf.Lerp(
			velocity.x,
			normalizedHorizontalSpeed * runSpeed,
			Time.deltaTime * smoothedMovementFactor
			);
	}

	void ApplyMovement()
	{
		controller.move(velocity * Time.deltaTime);
	}

	void SavePreviousPosition()
	{
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

	void OnPlayerDead(int hitFrom, Weapon.WeaponType weaponType)
	{
		this.enabled = false;
	}

	void OnEnable()
	{
		EventKit.Subscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
	}
}
