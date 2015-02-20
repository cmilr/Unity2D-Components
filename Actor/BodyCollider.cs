using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class BodyCollider : CacheBehaviour
{
	private bool alreadyCollided;
	private PickupEntity pickupEntity;
	private EntityBehaviour entityBehaviour;
	// private CharacterEntity charEntity;
	

	void Start()
	{
		base.CacheComponents();
		MLib2D.IgnoreLayerCollisionWith(gameObject, "One-Way Platform", true);
		MLib2D.IgnoreLayerCollisionWith(gameObject, "Platform", true);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		GetColliderComponents(coll);

		if (coll.tag == "Prize" && !alreadyCollided)
		{
			Messenger.Broadcast<int>("prize collected", pickupEntity.worth);
			pickupEntity.React();
		}

		if (coll.tag == "Enemy" && !alreadyCollided)
		{
		    Messenger.Broadcast<string, Collider2D>("player dead", "StruckDown", coll);
		}

		if (coll.tag == "Water" && !alreadyCollided)
		{
		    Messenger.Broadcast<string, Collider2D>("player dead", "Drowned", coll);
		}

		if (coll.tag == "Wall")
		{
		    Messenger.Broadcast<bool>("touching wall", true);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
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
			entityBehaviour = coll.GetComponent<EntityBehaviour>();
			alreadyCollided = entityBehaviour.alreadyCollided;
		}
	}
}