using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileContainer : Weapon
{
	private Vector3 origin;
	private Weapon weapon;
	private RuntimeAnimatorController anim;
	private new Transform transform;
	private new Collider2D collider2D;
	private new Rigidbody2D rigidbody2D;
	private SpriteRenderer projSpriteRenderer;
	private Animator animator;
	private Sequence projectileFadeIn;
	private Sequence projectileFadeInInstant;
	private Sequence projectileFadeOut;

	void Awake()
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
	}

	void Start()
	{
		// cache & pause tween sequences.
		(projectileFadeIn = MFX.Fade(projSpriteRenderer, 1f, 0f, .3f)).Pause();
		(projectileFadeInInstant = MFX.Fade(projSpriteRenderer, 1f, 0f, 0f)).Pause();
		(projectileFadeOut = MFX.Fade(projSpriteRenderer, 0, 0, .15f)).Pause();
	}

	// note: ProjectileContainers contain simple dummy values, which are
	// replaced by data that's passed-in via projectile objects during init.
	void Init(Weapon incoming)
	{
		weapon = incoming;
		type = incoming.type;
		alreadyCollided = false;
		iconSprite = incoming.iconSprite;
		title = incoming.title;
		damage = incoming.damage;
		hp = incoming.hp;
		rateOfAttack = incoming.rateOfAttack;
		projectileSprite = incoming.projectileSprite;
		speed = incoming.speed;
		maxDistance = incoming.maxDistance;
		lob = incoming.lob;
		lobGravity = incoming.lobGravity;
		fadeIn = incoming.fadeIn;
		collider2D.enabled = true;
		origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		// initialize animation controller if projectile is animated
		if (incoming.GetComponent<Weapon>().animatedProjectile)
		{
			anim = (RuntimeAnimatorController)Instantiate(
				Resources.Load(("Sprites/Projectiles/" + incoming.name + "_0"), 
                typeof(RuntimeAnimatorController))
			);
			Assert.IsNotNull(anim);
			animator.runtimeAnimatorController = anim;
			animator.speed = .5f;
		}

		InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
	}

	// fire directionally
	public void Fire(bool firedByPlayer, Weapon weapon, float direction)
	{
		Init(weapon);

		//player sprite and weapon sprites actually face opposite directions,
		//this flips the weapon sprite to match that of the player
		transform.SetLocalScaleX(-direction);
		projSpriteRenderer.sortingLayerName = PROJECTILE_SORTING_LAYER;
		projSpriteRenderer.sprite = projectileSprite;

		if (fadeIn)
		{
			projectileFadeIn.Restart();
		}
		else
		{
			projectileFadeInInstant.Restart();
		}

		// lob projectile like a cannon ball
		if (lob)
		{
			// if fired by the player, use the custom gravity supplied by the weapon (lobGravity,)
			// otherwise, for projectiles fired by an enemy, use the default gravity of .5f
			if (firedByPlayer)
			{
				rigidbody2D.gravityScale = lobGravity;
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
		projSpriteRenderer.sprite = projectileSprite;

		if (fadeIn)
		{
			projectileFadeIn.Restart();
		}
		else
		{
			projectileFadeInInstant.Restart();
		}

		// lob projectile like a cannon ball
		if (lob)
		{
			// if fired by the player, use the custom gravity supplied by the weapon (lobGravity,)
			// otherwise, for projectiles fired by an enemy, use the default gravity of .5f
			if (firedByPlayer)
			{
				rigidbody2D.gravityScale = lobGravity;
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
			Invoke("DeactivateEntireObject", .16f);
		}
	}

	void CheckDistanceTraveled()
	{
		float distance = Vector3.Distance(origin, transform.position);

		if (distance > weapon.maxDistance) {
			Invoke("DeactivateCollider", .1f);
			Invoke("DeactivateEntireObject", 1.5f);
		}
	}

	void DeactivateCollider()
	{
		collider2D.enabled = false;
	}

	void DeactivateEntireObject()
	{
		gameObject.SetActive(false);
	}

	// upon recycling, clear previous references and fade gameObject to zero
	void OnDisable()
	{
		anim = null;
		weapon = null;
		animator.runtimeAnimatorController = null;
		projSpriteRenderer.sprite = null;
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
