using UnityEngine;

public class WallEntity : Entity
{
	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		EventKit.Broadcast("player touching wall", true);
	}

	override public void OnBodyCollisionStay()
	{
		EventKit.Broadcast("player touching wall", true);
	}

	override public void OnBodyCollisionExit()
	{
		EventKit.Broadcast("player touching wall", false);
	}

	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
