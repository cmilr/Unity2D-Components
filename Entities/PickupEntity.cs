using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;


public class PickupEntity : Entity
{
	public enum EntityType { prize, levelUp, weapon, save, load };
	public EntityType entityType;
	private Light glow;

	void Start()
	{
		glow = gameObject.GetComponent<Light>() as Light;

		if (entityType == EntityType.prize) { AutoAlign(); }
		if (entityType == EntityType.levelUp) { AutoAlign(); }

		base.Start();
	}

	override public void OnBodyCollisionEnter()
	{
		collidedWithBody = true;

		if (!sceneLoading && !player.Dead)
		{
			switch (entityType)
			{
				case EntityType.prize:
					MTween.PickupPrize(gameObject);
					MTween.ExtinguishLight(glow, 0, .1f);
					Messenger.Broadcast<int>("prize collected", worth);
				break;

				case EntityType.levelUp:
					MTween.PickupPrize(gameObject);
					MTween.ExtinguishLight(glow, 0, .1f);
					Messenger.Broadcast<int>("prize collected", worth);
			    	Messenger.Broadcast<bool>("level completed", true);
				break;

				case EntityType.weapon:
					MTween.PickupWeapon(gameObject);
				break;

				case EntityType.save:
					Messenger.Broadcast<bool>("save player data", true);
				break;

				case EntityType.load:
					Messenger.Broadcast<bool>("load player data", true);
				break;
			}
		}
	}

	override public void OnBodyCollisionStay() {}

	override public void OnBodyCollisionExit() {}

	override public void OnWeaponCollisionEnter() {}

	override public void OnWeaponCollisionStay() {}

	override public void OnWeaponCollisionExit() {}
}
