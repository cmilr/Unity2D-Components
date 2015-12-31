using UnityEngine;

public class WallEntity : Entity
{
	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		Evnt.Broadcast<bool>("player touching wall", true);
	}

	override public void OnBodyCollisionStay()
	{
		Evnt.Broadcast<bool>("player touching wall", true);
	}

	override public void OnBodyCollisionExit()
	{
		Evnt.Broadcast<bool>("player touching wall", false);
	}

	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
