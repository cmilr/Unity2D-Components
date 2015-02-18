using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;


public class PickupEntity : EntityBehaviour
{
	public enum EntityType { none, prize, weapon };
	public EntityType entityType;
	public int worth;

	void Start()
	{
		base.CacheComponents();

		if (entityType == EntityType.prize)
			AutoAlign();
	}

	public void React()
	{
		alreadyCollided = true;

		switch (entityType)
		{
		case EntityType.none:
			break;

		case EntityType.prize:
			MTween.PickupPrize(gameObject);
			break;

		case EntityType.weapon:
			MTween.PickupWeapon(gameObject);
			break;
		}
	}
}
