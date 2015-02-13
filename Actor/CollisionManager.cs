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

	public void OnWeaponTriggerEnter(GameObject coll)
	{
		Debug.Log("Weapon Hit!");
	}

	public void OnWeaponTriggerStay(GameObject coll)
	{
		Debug.Log("Weapon Stay!");
	}

	public void OnWeaponTriggerExit(GameObject coll)
	{
		Debug.Log("Weapon Unhit!");
	}

	public void OnBodyTriggerEnter(GameObject coll)
	{
		Debug.Log("Body Hit!");
	}

	public void OnBodyTriggerStay(GameObject coll)
	{
		Debug.Log("Body Stay!");
	}

	public void OnBodyTriggerExit(GameObject coll)
	{
		Debug.Log("Body Unhit!");
	}
}