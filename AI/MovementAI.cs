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
	public float movementSpeed      	 = 2f;
	public float walkAnimationSpeed 	 = .5f;
	public float chanceOfPause      	 = 1f;        // chance of pause during any given interval

	private float lookInterval 		 = .3f;
	private float playerOffset 		 = 3f;        // offset target so enemy doesn't end up exactly where player is
	private float xAxisOffset  		 = .3f;
	private float movementInterval;
	private string walkAnimation;
	private int sideHit;
	private bool hesitant;
	private bool blockedLeft;
	private bool blockedRight;
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

	// MASTER CONTROLLER
	void OnBecameVisible()
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

	void LookAtTarget()
	{
		int direction = (target.position.x > transform.position.x) ? RIGHT : LEFT;

		transform.SetLocalScaleX((float)direction);
	}

	void FollowTarget()
	{
		if (!enabled) { return; }

		// get the proper direction for the enemy to move, then send him moving
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

			// ensure that actor is always facing in the direction it is moving
			transform.SetLocalScaleX((float)walkingDirection);

			// add some random pauses
			if (hesitant && Rand.Range(0f, 100f) <= chanceOfPause)
			{
				rigidbody2D.velocity = Vector2.zero;
				StartCoroutine(PauseFollowTarget());
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
		walkingDirection = (target.position.x > transform.position.x) ? RIGHT : LEFT;

		if (blockedRight && walkingDirection == RIGHT)
		{
			movementPaused = true;
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.angularVelocity = 0f;

			if (transform.position.x > blockedRightAt) {
				transform.SetXPosition(blockedRightAt);
			}
		}
		else if (blockedLeft && walkingDirection == LEFT)
		{
			movementPaused = true;
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.angularVelocity = 0f;

			if (transform.position.x < blockedLeftAt) {
				transform.SetXPosition(blockedLeftAt);
			}
		}
		// if enemy and player are on roughly same x axis, movementPaused
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

	void RotateTowardsTarget()
	{
		Vector3 vel = GetForceFrom(transform.position, target.position);
		float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;

		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	static Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
	{
		float power = 1;

		return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
	}

	// check for edge blockers
	void OnTriggerEnter2D(Collider2D coll)
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

	// check if cleared edge blocker
	void OnTriggerExit2D(Collider2D coll)
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

	void OnPlayerDead(string causeOfDeath, Collider2D coll, int directionHit)
	{
		// causes enemy to periodically do a victory dance
		xAxisOffset = .005f;
	}

	void OnDisable()
	{
		CancelInvoke();
		StopAllCoroutines();
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
