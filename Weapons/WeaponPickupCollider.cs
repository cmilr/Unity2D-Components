using Matcha.Dreadful;
using UnityEngine;

public class WeaponPickupCollider : CacheBehaviour
{
	void Start()
	{
		gameObject.layer = PICKUP_LAYER;
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == PLAYER_COLLIDER)
		{
			MFX.PickupWeapon(gameObject);
			Evnt.Broadcast<int>("prize collected", transform.parent.GetComponent<Weapon>().worth);
			Evnt.Broadcast<GameObject>("equip new weapon", transform.parent.gameObject);
		}
	}

	public void EnableWeaponPickupCollider()
	{
			collider2D.enabled = true;
	}

	public void DisableWeaponPickupCollider()
	{
			collider2D.enabled = false;
	}
}
