using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class PickupEntity : Entity
{
	public enum Type { Invalid, Prize, LevelUp, Save, Load };
	public Type type;
	private Light glow;
	private Sequence pickupPrize;
	private Sequence extinguishLight;

	void Start()
	{
		glow = gameObject.GetComponentInChildren<Light>() as Light;

		Assert.IsFalse(type == Type.Invalid,
			("Invalid pickup type @ " + gameObject));
		
		(pickupPrize 	 = MFX.PickupPrize(gameObject)).Pause();
		(extinguishLight = MFX.ExtinguishLight(glow, 0, .1f)).Pause();
		
		if ((type == Type.Prize || type == Type.LevelUp) && autoAlign) {
			AutoAlign();
		}
	}

	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		if (!levelCompleted && !playerDead)
		{
			switch (type)
			{
				case Type.Prize:
					pickupPrize.Restart();
					if (glow != null) extinguishLight.Restart();
					EventKit.Broadcast("prize collected", worth);
					break;	
				case Type.LevelUp:
					pickupPrize.Restart();
					if (glow != null) extinguishLight.Restart();
					levelCompleted = true;
					EventKit.Broadcast("prize collected", worth);
					EventKit.Broadcast("level completed", true);
					break;
				case Type.Save:
					EventKit.Broadcast("save player data", true);
					break;
				case Type.Load:
					EventKit.Broadcast("load player data", true);
					break;
				default:
					Assert.IsTrue(false, ("Pickup type missing from switch @ " + gameObject));
					break;
			}
		}
	}

	override public void OnBodyCollisionStay() {}
	override public void OnBodyCollisionExit() {}
	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
