
using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileContainer : BaseBehaviour
{
	public Weapon weapon;
	private Vector3 origin;
	private new Transform transform;
	private new Collider2D collider2D;
	private new Rigidbody2D rigidbody2D;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private Tween projectileFadeIn;
	private Tween projectileFadeInInstant;
	private Tween projectileFadeOut;

	public void CacheRefsThenDisable()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);

		collider2D = GetComponent<Collider2D>();
		Assert.IsNotNull(collider2D);

		rigidbody2D = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(rigidbody2D);

		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);

		animator = GetComponent<Animator>();
		Assert.IsNotNull(animator);

		(projectileFadeIn 			= MFX.FadeTween(spriteRenderer, 1f, .3f)).Pause();
		(projectileFadeInInstant 	= MFX.FadeTween(spriteRenderer, 1f, 0f)).Pause();
		(projectileFadeOut 			= MFX.FadeTween(spriteRenderer, 0, .1f)).Pause();

		gameObject.SetActive(false);
	}

	void Init(Weapon incoming)
	{
		weapon = incoming;

		weapon.alreadyCollided = false;

		// assign animation controller if projectile is animated
		if (weapon.animController != null)
		{
			animator.runtimeAnimatorController = weapon.animController;
			animator.speed = .5f;
		}

		collider2D.enabled = true;
		origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		rigidbody2D.isKinematic = false;

		InvokeRepeating("CheckDistanceTraveled", 1, 1F);
	}

	// fire directionally
	public void Fire(bool firedByPlayer, Weapon weapon, float direction)
	{
		Init(weapon);
		spriteRenderer.sprite = weapon.actualSprite;
		//player sprite and weapon sprites actually face opposite directions,
		//this flips the weapon sprite to match that of the player
		transform.SetLocalScaleX(-direction);
		SetSortOrderAndFadeIn(firedByPlayer, weapon);

		// lob projectile like a cannon ball
		if (weapon.lob)
		{
			// if fired by the player, use the custom gravity supplied by the weapon (lobGravity,)
			// otherwise, for projectiles fired by an enemy, use the default gravity of .5f
			if (firedByPlayer)
			{
				rigidbody2D.gravityScale = weapon.lobGravity;
			}
			else
			{
				rigidbody2D.gravityScale = .5f;
			}
		}
		// otherwise, fire projectile linearally
		else
		{
			rigidbody2D.gravityScale = 0;
		}

		rigidbody2D.velocity = transform.right * weapon.speed * direction;
	}

	// fire at target
	public void Fire(bool firedByPlayer, Weapon weapon, Transform target)
	{
		Init(weapon);
		spriteRenderer.sprite = weapon.actualSprite;
		SetSortOrderAndFadeIn(firedByPlayer, weapon);

		// lob projectile like a cannon ball
		if (weapon.lob)
		{
			// if fired by the player, use the custom gravity supplied by the weapon (lobGravity,)
			// otherwise, for projectiles fired by an enemy, use the default gravity of .5f
			if (firedByPlayer)
			{
				rigidbody2D.gravityScale = weapon.lobGravity;
			}
			else
			{
				rigidbody2D.gravityScale = .5f;
			}

			rigidbody2D.velocity = M.LobProjectile(weapon, transform, target);
		}
		// otherwise, fire projectile linearally
		else
		{
			rigidbody2D.gravityScale = 0;
			rigidbody2D.velocity = (target.position - transform.position).normalized * weapon.speed;
		}
	}

	void SetSortOrderAndFadeIn(bool firedByPlayer, Weapon theWeapon)
	{
		if (firedByPlayer)
		{
			transform.SetPositionZ(BEHIND_PLAYER_Z);
			spriteRenderer.sortingOrder = PLAYER_PROJECTILE_ORDER;
		}
		else
		{
			transform.SetPositionZ(IN_FRONT_OF_PLAYER_Z);
			spriteRenderer.sortingOrder = ENEMY_PROJECTILE_ORDER;
		}

		if (theWeapon.fadeIn)
		{
			projectileFadeIn.Restart();
		}
		else
		{
			projectileFadeInInstant.Restart();
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		int layer = coll.gameObject.layer;

		if (layer == ENEMY_BODY_COLLIDER) {
			DisableProjectile();
		}
	}

	void CheckDistanceTraveled()
	{
		float distance = Vector3.Distance(origin, transform.position);

		if (distance > weapon.maxDistance) {
			DisableProjectile();
		}
	}

	// upon recycling, clear previous references and fade gameObject to zero
	void DisableProjectile()
	{
		collider2D.enabled = false;
		projectileFadeOut.Restart();
		Invoke("DisableRenderer", .1f);
		Invoke("RecycleProjectile", .5f);
	}

	void DisableRenderer()
	{
		spriteRenderer.enabled = false;
	}

	void RecycleProjectile()
	{
		transform.position = new Vector3(0f, 0f, 0f);
		rigidbody2D.isKinematic = true;
		CancelInvoke();
	}

	public void AllocateMemory()
	{
		// at time of instantiation in the pool, allocate memory for GetComponent() calls
		rigidbody2D.mass = rigidbody2D.mass;
		spriteRenderer.sprite = spriteRenderer.sprite;
		transform.position = transform.position;
		collider2D.enabled = true;
	}
}
