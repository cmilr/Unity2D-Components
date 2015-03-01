using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class BodyCollider : CacheBehaviour
{
	private PickupEntity pickup;
	private CreatureEntity creature;
	private WaterEntity water;
	private PlayerState state;
	private bool colliderDisabled;
	

	void Start()
	{
		base.CacheComponents();
		state = transform.parent.GetComponent<PlayerState>();

		MLib2D.IgnoreLayerCollisionWith(gameObject, "One-Way Platform", true);
		MLib2D.IgnoreLayerCollisionWith(gameObject, "Platform", true);
	}

	void GetColliderComponents(Collider2D coll)
	{
		pickup = coll.GetComponent<PickupEntity>() as PickupEntity;
		creature = coll.GetComponent<CreatureEntity>() as CreatureEntity;
		water = coll.GetComponent<WaterEntity>() as WaterEntity;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{
			GetColliderComponents(coll);

			if (coll.tag == "Prize" && !pickup.HasCollidedWithBody() && !state.Dead)
			{
				pickup.SetCollidedWithBody(true);
				Messenger.Broadcast<int>("prize collected", pickup.Worth());
				pickup.ReactToCollision();
			}

			if (coll.tag == "Enemy" && !creature.HasCollidedWithBody() && !state.Dead)
			{
				creature.SetCollidedWithBody(true);
			    Messenger.Broadcast<string, Collider2D>("has died", "StruckDown", coll);
			}

			if (coll.tag == "LevelUp" && !pickup.HasCollidedWithBody() && !state.Dead)
			{
				pickup.SetCollidedWithBody(true);
				Messenger.Broadcast<int>("prize collected", pickup.Worth());
				pickup.ReactToCollision();
			    Messenger.Broadcast<bool>("level completed", true);

			    colliderDisabled = true;
			}

			if (coll.tag == "Water" && !water.HasCollidedWithBody())
			{
				water.SetCollidedWithBody(true);
			    Messenger.Broadcast<string, Collider2D>("has died", "Drowned", coll);
			}

			if (coll.tag == "Wall")
			{
			    Messenger.Broadcast<bool>("touching wall", true);
			}
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{
			if (coll.tag == "Wall")
			{
			    Messenger.Broadcast<bool>("touching wall", true);
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{
			GetColliderComponents(coll);

			if (coll.tag == "Enemy")
			{
				creature.SetCollidedWithBody(false);
			}

			if (coll.tag == "Water")
			{
				water.SetCollidedWithBody(false);
			}

			if (coll.tag == "Wall")
			{
			    Messenger.Broadcast<bool>("touching wall", false);
			}
		}
	}
}