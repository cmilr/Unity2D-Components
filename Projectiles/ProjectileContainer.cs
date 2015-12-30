using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;

public class ProjectileContainer : Weapon
{
	private Vector3 origin;
	private Weapon weapon;
	private RuntimeAnimatorController anim;

	// note: ProjectileContainers contain simple dummy values, which are
	// then replaced by data that's passed-in via projectile objects
	void Init(Weapon incoming)
	{
		weapon                = incoming;
		weaponType            = incoming.weaponType;
		alreadyCollided       = false;
		iconSprite            = incoming.iconSprite;
		title                 = incoming.title;
		damage                = incoming.damage;
		hp                    = incoming.hp;
		rateOfAttack          = incoming.rateOfAttack;
		spriteRenderer.sprite = incoming.carriedSprite;
		speed                 = incoming.speed;
		maxDistance           = incoming.maxDistance;
		lob                   = incoming.lob;
		lobGravity            = incoming.lobGravity;
		fadeIn                = incoming.fadeIn;
		collider2D.enabled    = true;
		origin                = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		//flip projectile
		transform.SetLocalScaleX(-1f);

		// initialize animation controller if projectile is animated
		if (incoming.GetComponent<Weapon>().animatedProjectile)
		{
			anim = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate
						(Resources.Load(("Sprites/Projectiles/" + incoming.name + "_0"), typeof(RuntimeAnimatorController)));
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

		if (fadeIn)
		{
			MFX.Fade(spriteRenderer, 1f, 0f, .3f);
		}
		else
		{
			MFX.Fade(spriteRenderer, 1f, 0f, 0f);
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

		if (fadeIn)
		{
			MFX.Fade(spriteRenderer, 1f, 0f, .3f);
		}
		else
		{
			MFX.Fade(spriteRenderer, 1f, 0f, 0f);
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

	// fade and deactivate on impact
	void OnTriggerEnter2D(Collider2D coll)
	{
		int layer = coll.gameObject.layer;

		if (layer == ENEMY_COLLIDER)
		{
			MFX.Fade(spriteRenderer, 0, 0, .15f);
			Invoke("DeactivateEntireObject", .16f);
		}
	}

	void CheckDistanceTraveled()
	{
		float distance = Vector3.Distance(origin, transform.position);

		if (distance > weapon.maxDistance)
		{
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
		spriteRenderer.sprite = null;
		CancelInvoke();
		MFX.Fade(spriteRenderer, 0f, 0f, 0f);
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






// using UnityEngine;
// using System;
// using System.Collections;
// using Matcha.Dreadful;
// using Matcha.Unity;


// public class ProjectileContainer : Weapon {

//     private Weapon weapon;
//     private Vector3 origin;
//     private RuntimeAnimatorController anim;

//     void Init(Weapon weapon)
//     {
//         this.weapon           = weapon;
//         weaponType            = weapon.weaponType;
//         alreadyCollided       = false;
//         iconSprite            = weapon.iconSprite;
//         title                 = weapon.title;
//         damage                = weapon.damage;
//         hp                    = weapon.hp;
//         rateOfAttack          = weapon.rateOfAttack;
//         spriteRenderer.sprite = weapon.projectileSprite;
//         speed                 = weapon.speed;
//         maxDistance           = weapon.maxDistance;
//         lob                   = weapon.lob;
//         fadeIn                = weapon.fadeIn;
//         collider2D.enabled    = true;
//         origin                = new Vector3(transform.position.x, transform.position.y, transform.position.z);

//         // initialize animation controller
//         if (weapon.GetComponent<Weapon>().animatedProjectile)
//         {
//             anim = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate
//                 (Resources.Load(("AnimControllers/Projectiles/" + weapon.name + "_0"), typeof(RuntimeAnimatorController )));
//             animator.runtimeAnimatorController = anim;
//             animator.speed = .5f;
//             // animator.Play("Flaming Skull");
//         }

//         InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
//     }

//     // FIRE DIRECTIONALLY
//     public void Fire(Weapon weapon, float direction)
//     {
//         Init(weapon);
//         rigidbody2D.gravityScale = .15f;
//         rigidbody2D.velocity = transform.right * weapon.speed * direction;
//     }

//     // FIRE AT TARGET
//     public void Fire(Weapon weapon, Transform target)
//     {
//         // Init(weapon);

//         // if (lob)
//         // {   // otherwise, lob projectile like a cannon ball
//         //     rigidbody2D.velocity = M.LobProjectile(weapon, transform, target);
//         // }
//         // else
//         // {   // if weapon has no mass, fire projectile linearally
//         //     rigidbody2D.gravityScale = 0;
//         //     rigidbody2D.velocity = (target.position - transform.position).normalized * weapon.speed;
//         // }
//     }

//     void CheckDistanceTraveled()
//     {
//         float distance = Vector3.Distance(origin, transform.position);
//         if (distance > weapon.maxDistance)
//         {
//             Invoke("DeactivateCollider", .1f);
//             Invoke("DeactivateEntireObject", 1.5f);
//         }
//     }

//     void DeactivateCollider()
//     {
//         collider2D.enabled = false;
//     }

//     void DeactivateEntireObject()
//     {
//         gameObject.SetActive(false);
//     }

//     // upon recycling, clear previous references and fade gameObject to zero
//     void OnDisable()
//     {
//         anim = null;
//         weapon = null;
//         animator.runtimeAnimatorController = null;
//         spriteRenderer.sprite = null;
//         CancelInvoke();
//     }

//     public void AllocateMemory()
//     {
//         // at time of instantiation in the pool, allocate memory for GetComponent() calls
//         rigidbody2D.mass = rigidbody2D.mass;
//         spriteRenderer.sprite = spriteRenderer.sprite;
//         transform.position = transform.position;
//         collider2D.enabled = true;
//     }
// }
