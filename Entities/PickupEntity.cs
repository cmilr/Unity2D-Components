 using Matcha.Dreadful;
using UnityEngine.Assertions;
using UnityEngine;

public class PickupEntity : Entity
{
	public enum EntityType { prize, levelUp, save, load };
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

		if (!levelCompleted && !playerDead)
		{
			switch (entityType)
			{
				case EntityType.prize:
				{
					MFX.PickupPrize(gameObject);
					MFX.ExtinguishLight(glow, 0, .1f);
					Evnt.Broadcast<int>("prize collected", worth);
					break;
				}

				case EntityType.levelUp:
				{
					MFX.PickupPrize(gameObject);
					MFX.ExtinguishLight(glow, 0, .1f);
					levelCompleted = true;
					Evnt.Broadcast<int>("prize collected", worth);
					Evnt.Broadcast<bool>("level completed", true);
					break;
				}

				case EntityType.save:
				{
					Evnt.Broadcast<bool>("save player data", true);
					break;
				}

				case EntityType.load:
				{
					Evnt.Broadcast<bool>("load player data", true);
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
