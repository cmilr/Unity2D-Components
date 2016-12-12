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
	private SpriteRenderer projSpriteRenderer;
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

		projSpriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(projSpriteRenderer);

		animator = GetComponent<Animator>();
		Assert.IsNotNull(animator);

		(projectileFadeIn = MFX.FadeTween(projSpriteRenderer, 1f, .3f)).Pause();
		(projectileFadeInInstant = MFX.FadeTween(projSpriteRenderer, 1f, 0f)).Pause();
		(projectileFadeOut = MFX.FadeTween(projSpriteRenderer, 0, .15f)).Pause();

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

		//player sprite and weapon sprites actually face opposite directions,
		//this flips the weapon sprite to match that of the player
		transform.SetLocalScaleX(-direction);
		projSpriteRenderer.sortingLayerName = PROJECTILE_SORTING_LAYER;
		projSpriteRenderer.sprite = weapon.projectileSprite;

		if (weapon.fadeIn)
		{
			projectileFadeIn.Restart();
		}
		else
		{
			projectileFadeInInstant.Restart();
		}

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
		projSpriteRenderer.sortingLayerName = PROJECTILE_SORTING_LAYER;
		projSpriteRenderer.sprite = weapon.projectileSprite;

		if (weapon.fadeIn)
		{
			projectileFadeIn.Restart();
		}
		else
		{
			projectileFadeInInstant.Restart();
		}

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

	void OnTriggerEnter2D(Collider2D coll)
	{
		int layer = coll.gameObject.layer;

		if (layer == ENEMY_BODY_COLLIDER) {
			projectileFadeOut.Restart();
			Invoke("DisableProjectile", .2f);
		}
	}

	void CheckDistanceTraveled()
	{
		float distance = Vector3.Distance(origin, transform.position);

		if (distance > weapon.maxDistance) {
			Invoke("DisableProjectile", .1f);
		}
	}

	// upon recycling, clear previous references and fade gameObject to zero
	void DisableProjectile()
	{
		collider2D.enabled = false;
		rigidbody2D.isKinematic = true;
		transform.position = new Vector3(0f, 0f, 0f);
		//weapon.animController = null;
		//weapon = null;
		//animator.runtimeAnimatorController = null;
		//projSpriteRenderer.sprite = null;
		CancelInvoke();
		projectileFadeOut.Restart();

	}

	public void AllocateMemory()
	{
		// at time of instantiation in the pool, allocate memory for GetComponent() calls
		rigidbody2D.mass = rigidbody2D.mass;
		projSpriteRenderer.sprite = projSpriteRenderer.sprite;
		transform.position = transform.position;
		collider2D.enabled = true;
	}
}
