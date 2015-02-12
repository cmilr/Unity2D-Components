using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]


public class CollisionManager : CacheBehaviour
{
	private GameObject coll;
	private InteractiveEntity interEntity;
	private CharacterEntity charEntity;

	void Start()
	{
		base.CacheComponents();
		Messenger.MarkAsPermanent("prize collected");
		Messenger.MarkAsPermanent("player dead");
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		coll = col.gameObject;
		interEntity = coll.GetComponent<InteractiveEntity>() as InteractiveEntity;
		charEntity = coll.GetComponent<CharacterEntity>() as CharacterEntity;

		if (coll.tag == "Prize" && !interEntity.AlreadyCollided)
		{
			Messenger.Broadcast<int>("prize collected", interEntity.worth);
			interEntity.React();
		}

		if (coll.tag == "Enemy" && !charEntity.AlreadyCollided)
		{
		    Messenger.Broadcast<bool>("player dead", true);
		}
	}
}