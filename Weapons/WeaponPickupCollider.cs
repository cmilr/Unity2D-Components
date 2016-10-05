using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponPickupCollider : BaseBehaviour
{
	private new Collider2D collider2D;
	private Sequence pickupWeapon;

	void Awake()
	{
		collider2D = GetComponent<Collider2D>();
		Assert.IsNotNull(collider2D);
		
		gameObject.layer = PICKUP_LAYER;
	}
	
	void Start()
	{
		(pickupWeapon = MFX.PickupWeapon(gameObject)).Pause();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == PLAYER_DEFAULT_LAYER)
		{
			pickupWeapon.Restart();
			EventKit.Broadcast("prize collected", transform.parent.GetComponent<Weapon>().worth);
			EventKit.Broadcast("equip new weapon", transform.parent.gameObject);
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
