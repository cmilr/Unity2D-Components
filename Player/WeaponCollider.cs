using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class WeaponCollider : CacheBehaviour
{
	private PickupEntity pickup;
	private CreatureEntity creature;
	private PlayerState state;
	private bool colliderDisabled;

	void Start()
	{
		base.CacheComponents();
		state = transform.parent.GetComponent<PlayerState>();

		MLib2D.IgnoreLayerCollisionWith(gameObject, "Pickups", true);
	}

	void GetColliderComponents(Collider2D coll)
	{
		pickup = coll.GetComponent<PickupEntity>() as PickupEntity;
		creature = coll.GetComponent<CreatureEntity>() as CreatureEntity;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{
			GetColliderComponents(coll);

			if (coll.tag == "Enemy" && !creature.HasCollidedWithWeapon() && !state.Dead)
			{
				creature.SetCollidedWithWeapon(true);
			}
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{

		}

	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{
			GetColliderComponents(coll);

			if (coll.tag == "Enemy")
			{
				creature.SetCollidedWithWeapon(false);
			}
		}
	}
}