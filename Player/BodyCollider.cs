using Matcha.Unity;
using System.Collections;
using UnityEngine;

public class BodyCollider : CacheBehaviour
{
	private int layer;
	private int hitFrom;
	private bool dead;
	private bool levelCompleted;
	private PlayerManager player;
	private Weapon enemyWeapon;
	private CreatureEntity enemy;

	void Start()
	{
		player = transform.parent.GetComponent<PlayerManager>();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == ENEMY_WEAPON)
		{
			enemyWeapon = coll.GetComponent<Weapon>();

			if (!enemyWeapon.alreadyCollided && !levelCompleted && !dead)
			{
				hitFrom = M.HorizSideThatWasHit(gameObject, coll);

				if (enemyWeapon.weaponType == Weapon.WeaponType.Hammer ||
						enemyWeapon.weaponType == Weapon.WeaponType.Dagger ||
						enemyWeapon.weaponType == Weapon.WeaponType.MagicProjectile)
				{
					enemyWeapon.alreadyCollided = true;

					player.TakesHit("projectile", enemyWeapon, coll, hitFrom);
				}
			}
		}
		else if (layer == ENEMY_COLLIDER)
		{
			enemy = coll.GetComponent<CreatureEntity>();

			if (!enemy.alreadyCollided && !levelCompleted && !dead)
			{
				hitFrom = M.HorizSideThatWasHit(gameObject, coll);

				if (enemy.entityType == CreatureEntity.EntityType.Enemy)
				{
					enemy.alreadyCollided = true;

					// player.TouchesEnemy("touch", enemy, coll, hitFrom);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == ENEMY_WEAPON)
		{
			enemyWeapon = coll.GetComponent<Weapon>();

			enemyWeapon.alreadyCollided = false;
		}
		else if (layer == ENEMY_COLLIDER)
		{
			enemy = coll.GetComponent<CreatureEntity>();

			enemy.alreadyCollided = false;
		}
	}

	void OnEnable()
	{
		Evnt.Subscribe<bool>("level completed", OnLevelCompleted);
		Evnt.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnDisable()
	{
		Evnt.Unsubscribe<bool>("level completed", OnLevelCompleted);
		Evnt.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
	{
		dead = true;
	}

	void OnLevelCompleted(bool status)
	{
		levelCompleted = status;
	}
}
