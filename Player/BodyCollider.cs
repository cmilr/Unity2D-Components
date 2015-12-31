using Matcha.Unity;
using System.Collections;
using UnityEngine;

public class BodyCollider : CacheBehaviour
{
	private int layer;
	private int hitFrom;
	private bool dead;
	private PlayerManager player;
	private IGameStateReadOnly game;
	private Weapon enemyWeapon;
	private CreatureEntity enemy;

	void Start()
	{
		player = transform.parent.GetComponent<PlayerManager>();
		game = GameObject.Find(GAME_STATE).GetComponent<IGameStateReadOnly>();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == ENEMY_WEAPON)
		{
			enemyWeapon = coll.GetComponent<Weapon>();

			if (!enemyWeapon.alreadyCollided && !game.LevelLoading && !dead)
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

			if (!enemy.alreadyCollided && !game.LevelLoading && !dead)
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
		Evnt.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnDisable()
	{
		Evnt.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
	{
		dead = true;
	}
}
