using UnityEngine;

public class BodyCollider : CacheBehaviour
{
	public Hit hit;

	private int layer;
	private bool dead;
	private bool levelCompleted;
	private PlayerManager player;
	private Weapon enemyWeapon;
	private CreatureEntity enemy;

	void Start()
	{
		hit = new Hit();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == ENEMY_WEAPON)
		{
			enemyWeapon = coll.GetComponent<Weapon>();

			if (!enemyWeapon.alreadyCollided && !levelCompleted && !dead)
			{
				if (enemyWeapon.weaponType == Weapon.WeaponType.Hammer ||
						enemyWeapon.weaponType == Weapon.WeaponType.Dagger ||
						enemyWeapon.weaponType == Weapon.WeaponType.MagicProjectile)
				{
					enemyWeapon.alreadyCollided = true;

					SendMessageUpwards("TakesHit", hit.Args(gameObject, coll));
				}
			}
		}
		else if (layer == ENEMY_COLLIDER)
		{
			enemy = coll.GetComponent<CreatureEntity>();

			if (!enemy.alreadyCollided && !levelCompleted && !dead)
			{
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
		EventKit.Subscribe<bool>("level completed", OnLevelCompleted);
<<<<<<< HEAD
		EventKit.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
=======
		EventKit.Subscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
	}

	void OnDisable()
	{
		EventKit.Unsubscribe<bool>("level completed", OnLevelCompleted);
<<<<<<< HEAD
		EventKit.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
=======
		EventKit.Unsubscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
	}

	void OnPlayerDead(int hitFrom, Weapon.WeaponType weaponType)
	{
		dead = true;
	}

	void OnLevelCompleted(bool status)
	{
		levelCompleted = status;
	}
}
