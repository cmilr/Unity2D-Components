using Matcha.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMovement : BaseBehaviour, ICreatureController
{
	private float gravity               = -35f;     // set gravity for player.
	private float runSpeed              = 7f;       // set player's run speed.
	private float groundDamping         = 20f;      // how fast do we change direction? higher means faster.
	private float inAirDamping          = 5f;       // how fast do we change direction mid-air?
	private float jumpHeight            = 2.25f;    // player's jump height.
	private float maxFallingSpeed       = 100f;     // max falling speed, for throttling falls, etc.
	private float jumpModifier          = 30f;      // increase jump height if traveling upwards on platforms etc.
	private float multiClickBuffer      = .2f;
	private float nextAttack            = 0f;
	private float aboutfaceOffset 		= .25f;
	private float yVelocity;
	private float normalizedHorizontalSpeed;
	private float repulseVelocity;
	private float previousY;
	private bool facingRight;
	private bool repulseRight;
	private bool repulseLeft;
	private Vector3 velocity;
	private RaycastHit2D lastControllerColliderHit;
	private CharacterController2D controller;
	private WeaponManager weaponManager;
	private AnimationState animState;
	private new Transform transform;

	// input from controls.
	bool inputMoveRight;
	bool inputMoveLeft;
	bool inputJump;
	bool inputAttack;

	// animation states.
	bool idle;
	bool running;
	bool jumping;
	bool attacking;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
		
		controller = GetComponent<CharacterController2D>();
		Assert.IsNotNull(controller);
	}
	
	void Start()
	{
		weaponManager = GetComponentInChildren<WeaponManager>();
		Assert.IsNotNull(weaponManager);

		animState = GetComponentInChildren<AnimationState>();
		Assert.IsNotNull(animState);
	}

	// main movement loop — keep in LateUpdate() to prevent player
	// falling through edge colliders, bouncing, etc.
	void Update()
	{
		GetYVelocity();
		InitializeVelocity();
		MovementStateMachine();
		CheckForAttack();
		ComputeMovement();
		ApplyGravity();
		ComputeRepulse();
		ClampYMovement();
		ApplyMovement();
		SavePreviousPosition();
	}

	// input methods required by ICreatureController.
	public void MoveRight()
	{
		inputMoveRight = true;
	}

	public void MoveLeft()
	{
		inputMoveLeft = true;
	}

	public void Jump()
	{
		inputJump = true;
	}

	public void Attack()
	{
		inputAttack = true;
	}

	// input methods used by touch controls.
	public void MoveRightCancel()
	{
		inputMoveRight = false;
	}

	public void MoveLeftCancel()
	{
		inputMoveLeft = false;
	}

	public void JumpCancel()
	{
		inputJump = false;
	}

	void MovementStateMachine()
	{
		if (inputMoveRight)
		{
			MovePlayerRight();
		}
		else if (inputMoveLeft)
		{
			MovePlayerLeft();
		}
		else
		{
			DontMovePlayer();
		}

		if (inputJump)
		{
			PlayerJump();
		}
	}

	void MovePlayerRight()
	{
		normalizedHorizontalSpeed = 1;

		// if also switching direction.
		if (transform.localScale.x < 0f)
		{
			transform.SetLocalScaleX(-transform.localScale.x);
			transform.SetPositionX(transform.position.x - aboutfaceOffset);
		}

		if (!facingRight)
		{
			facingRight = true;
		}

		animState.moving = true;
		animState.airborne = !controller.isGrounded;
		inputMoveRight = false;
	}

	void MovePlayerLeft()
	{
		normalizedHorizontalSpeed = -1;

		// if also switching direction.
		if (transform.localScale.x > 0f)
		{
			transform.SetLocalScaleX(-transform.localScale.x);
			transform.SetPositionX(transform.position.x + aboutfaceOffset);
		}

		if (facingRight)
		{
			facingRight = false;
		}

		animState.moving = true;
		animState.airborne = !controller.isGrounded;
		inputMoveLeft = false;
	}

	void DontMovePlayer()
	{
		// if grounded, our player is standing idle, so we set her speed to zero. otherwise she's
		// mid-air and we don't want to alter her trajectory, so we don't mess with the speed.
		if (controller.isGrounded)
		{
			normalizedHorizontalSpeed = 0;
			animState.moving = false;
		}

		animState.airborne = !controller.isGrounded;
	}

	void PlayerJump()
	{
		if (controller.isGrounded)
			velocity.y = Mathf.Sqrt(2f * (jumpHeight + (yVelocity * jumpModifier)) * -gravity);

		animState.airborne = true;
		inputJump = false;
	}

	void CheckForAttack()
	{
		if (Time.time > nextAttack)
		{
			if (inputAttack)
			{
				weaponManager.Attack();
				animState.attacking = true;
				inputAttack = false;
			}
			else
			{
				animState.attacking = false;
			}

			nextAttack = Time.time + multiClickBuffer;
		}
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

	void OnPlayerDead(Hit incomingHit)
	{
		enabled = false;
	}

	void OnEnable()
	{
		EventKit.Subscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<Hit>("player dead", OnPlayerDead);
	}
}
