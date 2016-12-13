using UnityEngine;
using UnityEngine.Assertions;

public class MeleeCollider : BaseBehaviour
{
    private Hit cachedHit;
	private new Collider2D collider2D;
	private Collider2D entity;

	void Awake()
	{
		collider2D = GetComponent<Collider2D>();
		Assert.IsNotNull(collider2D);

		gameObject.layer = PLAYER_WEAPON_COLLIDER;

		cachedHit = new Hit();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == ENEMY_BODY_COLLIDER )
		{
			entity = coll;
		}
		else
		{
			entity = null;
		}
	}

	void OnTriggerExit2D()
	{
		entity = null;
	}

	public void AttemptAttack(Weapon weapon)
	{
		Assert.IsNotNull(weapon);

		if (entity != null)
		{
			cachedHit.Create(entity.gameObject, collider2D);
			entity.gameObject.GetComponent<CreatureEntity>().TakesMeleeHit(cachedHit);
		}
	}

	public void EnableMeleeCollider()
	{
		collider2D.enabled = true;
	}

	public void DisableMeleeCollider()
	{
		collider2D.enabled = false;
	}
}
