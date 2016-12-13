using UnityEngine;

public class BodyCollider : BaseBehaviour
{
    public Hit cachedHit;
	private int layer;
	private bool dead;
	private bool levelCompleted;
	private PlayerManager player;
	private Weapon enemyWeapon;
	private CreatureEntity enemy;

	void Awake()
	{
		cachedHit = new Hit();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == ENEMY_WEAPON_COLLIDER)
		{
			enemyWeapon = coll.GetComponent<Weapon>() ?? coll.GetComponentInParent<ProjectileContainer>().weapon;

			if (!enemyWeapon.alreadyCollided && !levelCompleted && !dead)
			{
				if (enemyWeapon.type == Weapon.Type.Hammer ||
					enemyWeapon.type == Weapon.Type.Dagger ||
					enemyWeapon.type == Weapon.Type.MagicProjectile)
				{
					enemyWeapon.alreadyCollided = true;
					cachedHit.Create(gameObject, coll);
					SendMessageUpwards("TakesHit", cachedHit);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == ENEMY_WEAPON_COLLIDER)
		{
			enemyWeapon = coll.GetComponent<Weapon>() ?? coll.GetComponentInParent<ProjectileContainer>().weapon;

			enemyWeapon.alreadyCollided = false;
		}
		else if (layer == ENEMY_BODY_COLLIDER)
		{
			enemy = coll.GetComponent<CreatureEntity>();

			enemy.alreadyCollided = false;
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("level completed", OnLevelCompleted);
		EventKit.Subscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnDisable()
	{
		EventKit.Unsubscribe<bool>("level completed", OnLevelCompleted);
		EventKit.Unsubscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnPlayerDead(Hit incomingHit)
	{
		dead = true;
	}

	void OnLevelCompleted(bool status)
	{
		levelCompleted = status;
	}
}
