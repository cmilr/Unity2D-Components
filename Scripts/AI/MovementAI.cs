using Matcha.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class MovementAI : BaseBehaviour
{
	[HideInInspector]
	public int walkingDirection;
	public enum Style { Invalid, Sentinel, Scout, HesitantScout, Wanderer };
	public Style style;
	public bool movementPaused;
	public float movementSpeed          = 2f;
	public float walkAnimationSpeed     = .5f;
	public float chanceOfPause          = 1f;	//chance of pause during any given interval
	private float lookInterval          = .3f;
	private float playerOffset          = 3f;	//offset target so enemy doesn't end up exactly where player is
	private float xAxisOffset           = .3f;
	private float movementInterval;
	private string walkAnimation;
	private Side sideHit;
	private bool hesitant;
	private bool blockedLeft;
	private bool blockedRight;
	private bool dead;
	private float blockedRightAt;
	private float blockedLeftAt;
	private Transform target;
	private new Transform transform;
	private new Rigidbody2D rigidbody2D;
	private Animator animator;
    private const int RIGHT = 1;
    private const int LEFT = -1;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);

		rigidbody2D = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(transform);

		animator = GetComponent<Animator>();
		Assert.IsNotNull(transform);

		Assert.IsFalse(style == Style.Invalid, 
           ("Invalid movement style @ " + gameObject));
	}
	
	void Start()
	{	
		target = GameObject.Find(PLAYER).transform;
		Assert.IsNotNull(target);

		movementInterval = Rand.Range(.15f, 1f);
		
		walkAnimation = name + "_WALK_";
		animator.speed = walkAnimationSpeed;
		animator.Play(Animator.StringToHash(walkAnimation));
		
		hesitant |= style == Style.HesitantScout;
	}

	void LateUpdate()
	{
		if (!dead)
		{
			switch (style)
			{
				case Style.Scout:
				case Style.HesitantScout:
				{
					StopCheck();
					break;
				}
			}
		}
	}

	//master controller
	void OnBecameVisible()
	{
		if (!dead)
		{
			switch (style)
			{
				case Style.Sentinel:
				{
					InvokeRepeating("LookAtTarget", 1f, lookInterval);
					break;
				}

				case Style.Scout:
				case Style.HesitantScout:
				{
					InvokeRepeating("LookAtTarget", 1f, lookInterval);
					InvokeRepeating("FollowTarget", 1f, movementInterval);
					break;
				}
			}
		}
	}

	void LookAtTarget()
	{
		if (!dead)
		{
			int direction = (target.position.x > transform.position.x) ? RIGHT : LEFT;

			transform.SetLocalScaleX(direction);
		}
	}

	void FollowTarget()
	{
		if (!dead && !debug_MovementDisabled)
		{
			if (!enabled) { return; }

			//get the proper direction for the enemy to move, then send him moving
			if (target.position.x > transform.position.x + playerOffset)
			{
				walkingDirection = RIGHT;
			}
			else if (target.position.x < transform.position.x - playerOffset)
			{
				walkingDirection = LEFT;
			}
			else
			{
				movementPaused = true;
				rigidbody2D.velocity = Vector2.zero;
			}

			if (!movementPaused)
			{
				rigidbody2D.velocity = transform.right * movementSpeed * walkingDirection;

				//ensure that actor is always facing in the direction it is moving
				transform.SetLocalScaleX(walkingDirection);

				//add some random pauses
				if (hesitant && Rand.Range(0f, 100f) <= chanceOfPause)
				{
					rigidbody2D.velocity = Vector2.zero;
					StartCoroutine(PauseFollowTarget());
				}
			}
		}
	}

	IEnumerator PauseFollowTarget()
	{
		CancelInvoke("FollowTarget");
		yield return new WaitForSeconds(Rand.Range(2, 5));
		InvokeRepeating("FollowTarget", 1f, movementInterval);
	}

	void StopCheck()
	{
		if (!dead)
		{
			walkingDirection = (target.position.x > transform.position.x) ? RIGHT : LEFT;

			if (blockedRight && walkingDirection == RIGHT)
			{
				movementPaused = true;
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0f;

				if (transform.position.x > blockedRightAt) {
					transform.SetPositionX(blockedRightAt);
				}
			}
			else if (blockedLeft && walkingDirection == LEFT)
			{
				movementPaused = true;
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0f;

				if (transform.position.x < blockedLeftAt) {
					transform.SetPositionX(blockedLeftAt);
				}
			}
			//if enemy and player are on roughly same x axis, movementPaused
			else if (transform.position.x.FloatEquals(target.position.x, xAxisOffset))
			{
				movementPaused = true;
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0f;
			}
			else
			{
				movementPaused = false;
			}
		}
	}

	void RotateTowardsTarget()
	{
		if (!dead)
		{
			Vector3 vel = GetForceFrom(transform.position, target.position);
			float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;

			transform.eulerAngles = new Vector3(0, 0, angle);
		}
	}

	static Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
	{
		float power = 1;

		return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
	}

	//check for edge blockers
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (!dead)
		{
			int layer = coll.gameObject.layer;

			if (layer == EDGE_BLOCKER)
			{
				sideHit = M.HorizontalSideHit(gameObject, coll);

				if (sideHit == Side.Right)
				{
					blockedRight   = true;
					blockedRightAt = transform.position.x;
					gameObject.SendEventDown("SetBlockedRightState", true);
				}
				else if (sideHit == Side.Left)
				{
					blockedLeft   = true;
					blockedLeftAt = transform.position.x;
					gameObject.SendEventDown("SetBlockedLeftState", true);
				}

				rigidbody2D.velocity = Vector2.zero;
			}
		}
	}

	//check if cleared edge blocker
	void OnTriggerExit2D(Collider2D coll)
	{
		if (!dead)
		{
			int layer = coll.gameObject.layer;

			if (layer == EDGE_BLOCKER)
			{
				Side sideThatWasHit = M.HorizontalSideHit(gameObject, coll);

				if (sideThatWasHit == Side.Right)
				{
					blockedRight = false;
					gameObject.SendEventDown("SetBlockedRightState", false);
				}
				else if (sideThatWasHit == Side.Left)
				{
					blockedLeft = false;
					gameObject.SendEventDown("SetBlockedLeftState", false);
				}

				movementPaused = false;
			}
		}
	}

	void CreatureDead()
	{
		dead = true;
		movementPaused = true;
		enabled = false;
	}

	void OnPlayerDead(Hit incomingHit)
	{
		//causes enemy to periodically do a victory dance
		xAxisOffset = .005f;
	}

	void OnDisable()
	{
		CancelInvoke();
		StopAllCoroutines();
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
