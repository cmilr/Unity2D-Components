using Matcha.Unity;
using Rewired;
using System.Collections;
using System;
using UnityEngine.Assertions;
using UnityEngine;

public class DeathHandler : CacheBehaviour
{
	public float gravity                = -35f;                // set gravity for player
	public float runSpeed               = 8f;                  // set player's run speed
	public float groundDamping          = 20f;                 // how fast do we change direction? higher means faster
	public float inAirDamping           = 5f;                  // how fast do we change direction mid-air?
	public float jumpHeight             = 2.6f;                // player's jump height
	public float maxFallingSpeed        = 100f;                // for throttling falls, etc
	public float maxRisingSpeed         = 2f;                  // for throttling player on moving platforms, etc
	protected float previousX           = 0f;
	protected float previousY           = 0f;
	private bool physicsEnabled         = true;
	private bool alreadyDead;
	private bool facingRight;

	private Vector3 velocity;
	private BoxCollider2D boxCollider;
	private CharacterController2D controller;
	private RaycastHit2D lastControllerColliderHit;
	private float normalizedHorizontalSpeed;
	private string deathAnimation;
	private string struckdownAnimation;
	private string struckdownAnimation_face_down;
	private string drownedAnimation;
	private int hitFrom;

	void Start()
	{
		// component takes over player physics, so we don't enable until player dies
		this.enabled = false;
		controller   = GetComponent<CharacterController2D>();
		boxCollider  = GetComponent<BoxCollider2D>();
		AddListeners();

		SetCharacterAnimations("LAURA");
	}

	// set animations depending on which character is chosen
	void SetCharacterAnimations(string character)
	{
		// uses string literals over concatenation in order to reduce GC calls
		if (character == "LAURA")
		{
			struckdownAnimation = "LAURA_struckdown";
			struckdownAnimation_face_down = "LAURA_struckdown_face_down";
			drownedAnimation = "LAURA_drowned";
		}
		else
		{
			struckdownAnimation = "MAC_struckdown";
			struckdownAnimation_face_down = "MAC_struckdown_face_down";
			drownedAnimation = "MAC_drowned";
		}
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
	{
		this.enabled = true;
		this.hitFrom = hitFrom;
		rigidbody2D.velocity = Vector2.zero;

		switch (methodOfDeath)
		{
			case "struckdown":
			{
				animator.speed = STRUCKDOWN_SPEED;
				animator.Play(Animator.StringToHash(struckdownAnimation));
				PlayerStruckdown();
				break;
			}

			case "projectile":
			{
				animator.speed = STRUCKDOWN_SPEED;
				animator.Play(Animator.StringToHash(struckdownAnimation));
				PlayerHitByProjectile();
				break;
			}

			case "drowned":
			{
				animator.speed = DROWNED_SPEED;
				animator.Play(Animator.StringToHash(drownedAnimation));
				PlayerDrowned(coll);
				break;
			}

			case "out of bounds":
			{
				animator.speed = STRUCKDOWN_SPEED;
				animator.Play(Animator.StringToHash(struckdownAnimation));
				break;
			}

			default:
			{
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
			}
		}
	}

	void LateUpdate()
	{
		if (physicsEnabled)
		{
			velocity = controller.velocity;
			var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;

			velocity.x = Mathf.Lerp(
				velocity.x,
				normalizedHorizontalSpeed * runSpeed,
				Time.deltaTime * smoothedMovementFactor
				);

			velocity.y += gravity * Time.deltaTime;
			velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxFallingSpeed);
			controller.move(velocity * Time.deltaTime);
		}
	}

	void PlayerStruckdown()
	{
		// Repulse(intensity, distance)
		StartCoroutine(Repulse(1f, .15f));
		alreadyDead = true;
	}

	void PlayerHitByProjectile()
	{
		// Repulse(intensity, distance)
		StartCoroutine(Repulse(1.5f, .2f));
		alreadyDead = true;
	}

	void PlayerDrowned(Collider2D incomingColl)
	{
		physicsEnabled = false;

		if (!alreadyDead)
		{
			if (facingRight)
			{
				transform.SetLocalScaleX(-1f);
			}
			else
			{
				transform.SetLocalScaleX(1f);
			}
		}

		// get size and position of the water collider, then clamp player's
		// final resting position within those limits——
		// first we need to convert the collider to a BoxCollider2D
		BoxCollider2D coll = incomingColl.transform.GetComponent<BoxCollider2D>();

		Vector2 size = coll.size;
		Vector3 centerPoint = new Vector3(coll.offset.x, coll.offset.y, 0f);
		Vector3 worldPos = incomingColl.transform.TransformPoint(centerPoint);

		float left = worldPos.x - (size.x / 2f);
		float right = worldPos.x + (size.x / 2f);

		float playerPositionX = transform.position.x;
		float playerCenterOffset = (GetComponent<Renderer>().bounds.size.x / 2);

		playerPositionX = Mathf.Clamp(
			playerPositionX,
			left + playerCenterOffset,
			right - playerCenterOffset
			);

		transform.SetPositionXY(playerPositionX, transform.position.y - .2f);
	}


	IEnumerator Repulse(float intensity, float distance)
	{
		// intensity must be at least 1, otherwise things get graphically buggy
		intensity = (intensity >= 1) ? intensity : 1f;

		// sink player into platform by one pixel for a more immersed visual effect
		boxCollider.offset = new Vector2(0f, ONE_COLLIDER_PIXEL);

		// ANIMATION & INTENSITY
		// ~~~~~~~~~~~~~~~~~~~~~
		if (hitFrom == RIGHT)
		{
			// repulse to the left
			normalizedHorizontalSpeed = -intensity;

			// set sprite to facing up or down, depending on direction of hit, and direction player is facing
			if (!facingRight)
			{
				transform.SetLocalScaleX(-transform.localScale.x);
				animator.Play(Animator.StringToHash(struckdownAnimation_face_down));
			}
		}
		else
		{
			// repulse to the right
			normalizedHorizontalSpeed = intensity;

			// set sprite to facing up or down, depending on direction of hit, and direction player is facing
			if (facingRight)
			{
				transform.SetLocalScaleX(-transform.localScale.x);
				animator.Play(Animator.StringToHash(struckdownAnimation_face_down));
			}
		}

		// DISTANCE TO REPULSE
		// ~~~~~~~~~~~~~~~~~~~
		for (float f = 1f; f >= 0; f -= 0.1f)
		{
			if (f < 0) { f = 0f; }

			if (hitFrom == RIGHT)
			{
				normalizedHorizontalSpeed += .1f;
			}
			else
			{
				normalizedHorizontalSpeed -= .1f;
			}
			// tweak this paramater for greater or shorter repulse distance
			yield return new WaitForSeconds(distance);
		}
	}

	void OnDisable()
	{
		StopCoroutine(Repulse(0, 0));
	}

	void OnFacingRight(bool status)
	{
		facingRight = status;
	}

	void AddListeners()
	{
		Evnt.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
	}
}
