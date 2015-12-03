using Matcha.Dreadful;
using UnityEngine.Assertions;
using UnityEngine;


public class PickupEntity : Entity
{
	public enum EntityType { prize, levelUp, weapon, save, load };
	public EntityType entityType;
	private Light glow;

	void Start()
	{
		glow = gameObject.GetComponent<Light>() as Light;

		if ((entityType == EntityType.prize || entityType == EntityType.levelUp) && autoAlign) {
			AutoAlign();
		}
	}

	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		collidedWithBody = true;

		if (!game.LevelLoading && !player.Dead)
		{
			switch (entityType)
			{
				case EntityType.prize:
				{
					MFX.PickupPrize(gameObject);
					MFX.ExtinguishLight(glow, 0, .1f);
					Messenger.Broadcast<int>("prize collected", worth);
					break;
				}

				case EntityType.levelUp:
				{
					MFX.PickupPrize(gameObject);
					MFX.ExtinguishLight(glow, 0, .1f);
					Messenger.Broadcast<int>("prize collected", worth);
					Messenger.Broadcast<bool>("level completed", true);
					break;
				}

				case EntityType.weapon:
				{
					MFX.PickupWeapon(gameObject);
					break;
				}

				case EntityType.save:
				{
					Messenger.Broadcast<bool>("save player data", true);
					break;
				}

				case EntityType.load:
				{
					Messenger.Broadcast<bool>("load player data", true);
					break;
				}

				default:
				{
					Assert.IsTrue(false, "** Default Case Reached **");
					break;
				}
			}
		}
	}

	override public void OnBodyCollisionStay() {}
	override public void OnBodyCollisionExit() {}
	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
