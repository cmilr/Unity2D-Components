using UnityEngine;
using System.Collections;

public class WallEntity : Entity
{
	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		Messenger.Broadcast<bool>("touching wall", true);
	}

	override public void OnBodyCollisionStay()
	{
		Messenger.Broadcast<bool>("touching wall", true);
	}

	override public void OnBodyCollisionExit()
	{
		Messenger.Broadcast<bool>("touching wall", false);
	}

	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
