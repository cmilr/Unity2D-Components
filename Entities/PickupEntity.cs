using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;


public class PickupEntity : EntityBehaviour
{
	public enum EntityType { none, prize, weapon };
	public EntityType entityType;
	public int worth;
	public Light glow;

	void Start()
	{
		base.CacheComponents();
		glow = gameObject.GetComponent<Light>() as Light;

		if (entityType == EntityType.prize) { AutoAlign(); }
	}

	public void ReactToCollision()
	{
		switch (entityType)
		{
		case EntityType.none:
			break;

		case EntityType.prize:
			MTween.PickupPrize(gameObject);
			MTween.ExtinguishLight(glow, 0, .1f);
			break;

		case EntityType.weapon:
			MTween.PickupWeapon(gameObject);
			break;
		}
	}

	public int Worth()
	{
		return worth;
	}

	public void SetCollidedWithBody(bool status)
	{
		hasCollidedWithBody = status;
	}

	public bool HasCollidedWithBody()
	{
		return hasCollidedWithBody;
	}

	public void SetCollidedWithWeapon(bool status)
	{
		hasCollidedWithWeapon = status;
	}

	public bool HasCollidedWithWeapon()
	{
		return hasCollidedWithWeapon;
	}
}
