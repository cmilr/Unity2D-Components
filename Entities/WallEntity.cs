using UnityEngine;

public class WallEntity : Entity
{
	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		PlayerState.TouchingWall = true;
	}

	override public void OnBodyCollisionStay()
	{
		PlayerState.TouchingWall = true;
	}

	override public void OnBodyCollisionExit()
	{
		PlayerState.TouchingWall = false;
	}

	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
