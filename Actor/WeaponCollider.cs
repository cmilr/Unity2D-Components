using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class WeaponCollider : CacheBehaviour
{
	private bool alreadyCollided;
	private CharacterEntity charEntity;
	

	void Start()
	{
		base.CacheComponents();
		MLib2D.IgnoreLayerCollisionWith(gameObject, "Collectables", true);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		GetColliderComponents(coll);

		if (coll.tag == "Enemy" && !alreadyCollided)
		{
		    Debug.Log("Weapon hits enemy!");
		}
	}

	void GetColliderComponents(Collider2D coll)
	{
		charEntity = coll.GetComponent<CharacterEntity>() as CharacterEntity;

		if (coll.GetComponent<EntityBehaviour>())
			alreadyCollided = coll.GetComponent<EntityBehaviour>().AlreadyCollided;
	}
}