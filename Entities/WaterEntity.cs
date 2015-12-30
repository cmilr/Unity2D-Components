using UnityEngine;

public class WaterEntity : Entity
{
	private BoxCollider2D thisCollider;

	void Start()
	{
		thisCollider = GetComponent<BoxCollider2D>();
	}

	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		collidedWithBody = true;
		PlayerState.Dead = true;
		Messenger.Broadcast<string, Collider2D, int>("player dead", "drowned", thisCollider, 0);
	}

	override public void OnBodyCollisionStay() {}
	override public void OnBodyCollisionExit() {}
	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
