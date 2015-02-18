using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class BodyCollider : CacheBehaviour
{
	private bool alreadyCollided;
	private PickupEntity pickupEntity;
	private CharacterEntity charEntity;
	

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
			alreadyCollided = true;
		    Messenger.Broadcast<string, bool>("player dead", "StruckDown", true);
		}

		if (coll.tag == "Water" && !alreadyCollided)
		{
			alreadyCollided = true;
		    Messenger.Broadcast<string, bool>("player dead", "Drowned", true);
		}
	}

	void GetColliderComponents(Collider2D coll)
	{
		pickupEntity = coll.GetComponent<PickupEntity>() as PickupEntity;
		charEntity = coll.GetComponent<CharacterEntity>() as CharacterEntity;

		if (coll.GetComponent<EntityBehaviour>())
			alreadyCollided = coll.GetComponent<EntityBehaviour>().alreadyCollided;
	}
}