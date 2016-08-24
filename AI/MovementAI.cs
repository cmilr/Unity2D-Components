using Matcha.Unity;
using System.Collections;
using UnityEngine;

public class MovementAI : CacheBehaviour
{
	[HideInInspector]
	public int walkingDirection;

	public enum Style { Sentinel, Scout, HesitantScout, Wanderer };
	public Style style;
	public bool movementPaused;
	public float movementSpeed        = 2f;
	public float walkAnimationSpeed   = .5f;
	public float chanceOfPause        = 1f;        //chance of pause during any given interval

	private float lookInterval        = .3f;
	private float playerOffset        = 3f;        //offset target so enemy doesn't end up exactly where player is
	private float xAxisOffset         = .3f;
	private float movementInterval;
	private string walkAnimation;
	private int sideHit;
	private bool hesitant;
	private bool blockedLeft;
	private bool blockedRight;
	private bool dead;
	private float blockedRightAt;
	private float blockedLeftAt;
	private Transform target;

	void Start()
	{
		movementInterval = Rand.Range(.15f, 1f);
		target           = GameObject.Find(PLAYER).transform;
		walkAnimation    = name + "_WALK_";
		animator.speed   = walkAnimationSpeed;
		animator.Play(Animator.StringToHash(walkAnimation));

		if (style == Style.HesitantScout) {
			hesitant = true;
		}
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

			transform.SetLocalScaleX((float)direction);
		}
	}

	void FollowTarget()
	{
		if (!dead)
		{
			if (!enabled) { return; }

<<<<<<< HEAD
			// get the proper direction for the enemy to move, then send him moving
=======
			//get the proper direction for the enemy to move, then send him moving
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
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

<<<<<<< HEAD
				// ensure that actor is always facing in the direction it is moving
				transform.SetLocalScaleX((float)walkingDirection);

				// add some random pauses
=======
				//ensure that actor is always facing in the direction it is moving
				transform.SetLocalScaleX((float)walkingDirection);

				//add some random pauses
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
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
<<<<<<< HEAD
			// if enemy and player are on roughly same x axis, movementPaused
=======
			//if enemy and player are on roughly same x axis, movementPaused
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
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
				sideHit = M.HorizSideThatWasHit(gameObject, coll);

				if (sideHit == RIGHT)
				{
					blockedRight   = true;
					blockedRightAt = transform.position.x;
					gameObject.BroadcastMessage("SetBlockedRightState", true);
				}
				else if (sideHit == LEFT)
				{
					blockedLeft   = true;
					blockedLeftAt = transform.position.x;
					gameObject.BroadcastMessage("SetBlockedLeftState", true);
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
				int sideThatWasHit = M.HorizSideThatWasHit(gameObject, coll);

				if (sideThatWasHit == RIGHT)
				{
					blockedRight = false;
					gameObject.BroadcastMessage("SetBlockedRightState", false);
				}
				else if (sideThatWasHit == LEFT)
				{
					blockedLeft = false;
					gameObject.BroadcastMessage("SetBlockedLeftState", false);
				}

				movementPaused = false;
			}
		}
	}

	void CreatureDead()
	{
<<<<<<< HEAD
		CancelInvoke("FollowTarget");
		CancelInvoke("LookAtTarget");
		dead = true;
	}

	void OnPlayerDead(string causeOfDeath, Collider2D coll, int directionHit)
=======
		dead = true;
		movementPaused = true;
		this.enabled = false;
	}

	void OnPlayerDead(int hitFrom, Weapon.WeaponType weaponType)
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
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
<<<<<<< HEAD
		EventKit.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
=======
		EventKit.Subscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
	}

	void OnDestroy()
	{
<<<<<<< HEAD
		EventKit.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
=======
		EventKit.Unsubscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
	}
}
