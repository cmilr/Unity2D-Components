using UnityEngine;
using System.Collections;
using Rotorz.Tile;
using Matcha.Extensions;
using Matcha.Lib;

public class MovementAI : CacheBehaviour
{
	[HideInInspector]  								// current state
	public int walkingDirection;

	public enum MovementStyle { Sentinel, Scout, HesitantScout, Wanderer };
	public MovementStyle movementStyle;
	public float movementSpeed      = 2f;
	public float walkAnimationSpeed = .5f;
	public float chanceOfPause      = 1f;           // chance of pause during any given interval
	public bool movementPaused;

	private string walkAnimation;
	private float movementInterval;
	private float lookInterval      = .3f;
	private float xAxisOffset       = .3f;
	private float playerOffset      = 1.50f;		// offset target so enemy doesn't end up exactly where player is
	private int sideHit;
	private bool blockedLeft;
	private bool blockedRight;
	private bool hesitant;
	private Transform target;

	void Start()
	{
		target         = GameObject.Find(PLAYER).transform;
		walkAnimation  = name + "_WALK_";
		animator.speed = walkAnimationSpeed;
		animator.Play(Animator.StringToHash(walkAnimation));

		movementInterval = Random.Range(.15f, 1f);

		if (movementStyle == MovementStyle.HesitantScout)
			hesitant = true;
	}

	void LateUpdate()
	{
		switch (movementStyle)
		{
		case MovementStyle.Scout:
		case MovementStyle.HesitantScout:
			StopCheck();
			break;
		}
	}

	// MASTER CONTROLLER
	void OnBecameVisible()
	{
		switch (movementStyle)
		{
		case MovementStyle.Sentinel:
			InvokeRepeating("LookAtTarget", 1f, lookInterval);
			break;

		case MovementStyle.Scout:
		case MovementStyle.HesitantScout:
			InvokeRepeating("LookAtTarget", 1f, lookInterval);
			InvokeRepeating("FollowTarget", 1f, movementInterval);
			break;
		}
	}

	void LookAtTarget()
	{
		int direction = (target.position.x > transform.position.x) ? RIGHT : LEFT;
		transform.SetLocalScaleX((float)direction);
	}

	void FollowTarget()
	{
		if (!this.enabled) return;

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
			rigidbody2D.velocity = Vector2.zero;
			movementPaused = true;
		}

		if (!movementPaused)
		{
			rigidbody2D.velocity = transform.right * movementSpeed * walkingDirection;

			// ensure that actor is always facing in the direction it is moving
			transform.SetLocalScaleX((float)walkingDirection);

			// add some random pauses
			if (hesitant && Random.Range(0f, 100f) <= chanceOfPause)
			{
				rigidbody2D.velocity = Vector2.zero;
				StartCoroutine(PauseFollowTarget());
			}
		}
	}

	IEnumerator PauseFollowTarget()
	{
		CancelInvoke("FollowTarget");
		yield return new WaitForSeconds(Random.Range(2, 5));
		InvokeRepeating("FollowTarget", 1f, movementInterval);
	}

	void StopCheck()
	{
		walkingDirection = (target.position.x > transform.position.x) ? RIGHT : LEFT;

		if ((blockedRight && walkingDirection == RIGHT) || (blockedLeft && walkingDirection == LEFT))
		{
			// transform.SetXPosition(blockedAt);
			rigidbody2D.velocity = Vector2.zero;
			movementPaused = true;
		}
		// if enemy and player are on roughly same x axis, movementPaused
		else if (transform.position.x.FloatEquals(target.position.x, xAxisOffset))
		{
			rigidbody2D.velocity = Vector2.zero;
			movementPaused = true;
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

	Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
	{
		float power = 1;
		return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y))*power;
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
				blockedRight = true;
				gameObject.BroadcastMessage("SetBlockedRightState", true);
			}
			else if (sideHit == LEFT)
			{
				blockedLeft = true;
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
			int sideHit = M.HorizSideThatWasHit(gameObject, coll);

			if (sideHit == RIGHT)
			{
				blockedRight = false;
				gameObject.BroadcastMessage("SetBlockedRightState", false);
			}
			else if (sideHit == LEFT)
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
		Messenger.AddListener<string, Collider2D, int>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, Collider2D, int>( "player dead", OnPlayerDead);
	}
}
