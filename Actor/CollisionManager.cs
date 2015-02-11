using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]


public class CollisionManager : CacheBehaviour
{
	private GameObject coll;
	private InteractiveEntity interEntity;

	void Start()
	{
		base.CacheComponents();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		coll = col.gameObject;

		interEntity = coll.GetComponent<InteractiveEntity>() as InteractiveEntity;

		if (coll.tag == "Prize" && !interEntity.AlreadyCollided)
		{
			Messenger.Broadcast<int>("prize collected", interEntity.worth);
			interEntity.React();
		}
	}
}