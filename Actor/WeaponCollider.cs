using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class WeaponCollider : CacheBehaviour
{
	private GameObject coll;
	private InteractiveEntity interEntity;
	private CharacterEntity charEntity;
	

	void Start()
	{
		base.CacheComponents();

		MLib2D.IgnoreLayerCollisionWith(gameObject, "Collectables", true);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		coll = col.gameObject;
		// interEntity = coll.GetComponent<InteractiveEntity>() as InteractiveEntity;
		charEntity = coll.GetComponent<CharacterEntity>() as CharacterEntity;

		// if (coll.tag == "Prize")
		// {
		// 	Messenger.Broadcast<int>("prize collected", interEntity.worth);
		// 	interEntity.React();
		// }

		if (coll.tag == "Enemy" )
		{
		    Debug.Log("Weapon hits enemy!");
		}
	}
}