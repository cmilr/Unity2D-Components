using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class BodyCollider : CacheBehaviour
{
	private PickupEntity pickupEntity;
	private EntityBehaviour entity;
	private PlayerState state;
	// private CharacterEntity charEntity;
	private bool colliderDisabled;
	

	void Start()
	{
		base.CacheComponents();
		state = transform.parent.GetComponent<PlayerState>();

		MLib2D.IgnoreLayerCollisionWith(gameObject, "One-Way Platform", true);
		MLib2D.IgnoreLayerCollisionWith(gameObject, "Platform", true);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		GetColliderComponents(coll);

		if (coll.tag == "Prize" && !entity.alreadyCollided && !state.Dead && !colliderDisabled)
		{
			entity.alreadyCollided = true;
			Messenger.Broadcast<int>("prize collected", pickupEntity.worth);
			pickupEntity.ReactToCollision();
		}

		if (coll.tag == "Enemy" && !entity.alreadyCollided && !state.Dead && !colliderDisabled)
		{
			entity.alreadyCollided = true;
		    Messenger.Broadcast<string, Collider2D>("has died", "StruckDown", coll);
		}

		if (coll.tag == "LevelUp" && !entity.alreadyCollided && !state.Dead && !colliderDisabled)
		{
			entity.alreadyCollided = true;
			colliderDisabled = true;
			Messenger.Broadcast<int>("prize collected", pickupEntity.worth);
			pickupEntity.ReactToCollision();
		    Messenger.Broadcast<bool>("level completed", true);
		}

		if (coll.tag == "Water" && !entity.alreadyCollided && !colliderDisabled)
		{
			entity.alreadyCollided = true;
		    Messenger.Broadcast<string, Collider2D>("has died", "Drowned", coll);
		}

		if (coll.tag == "Wall")
		{
		    Messenger.Broadcast<bool>("touching wall", true);
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.tag == "Wall")
		{
		    Messenger.Broadcast<bool>("touching wall", true);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		GetColliderComponents(coll);

		if (coll.tag == "Enemy")
		{
			entity.alreadyCollided = true;
		}

		if (coll.tag == "Water")
		{
			entity.alreadyCollided = true;
		}

		if (coll.tag == "Wall")
		{
		    Messenger.Broadcast<bool>("touching wall", false);
		}
	}

	void GetColliderComponents(Collider2D coll)
	{
		pickupEntity = coll.GetComponent<PickupEntity>() as PickupEntity;
		// charEntity = coll.GetComponent<CharacterEntity>() as CharacterEntity;

		if (coll.GetComponent<EntityBehaviour>())
		{
			entity = coll.GetComponent<EntityBehaviour>();
		}
	}
}