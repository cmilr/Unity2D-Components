using UnityEngine;
using UnityEngine.Assertions;

public class WaterEntity : Entity
{
	private BoxCollider2D thisCollider;

	void Awake()
	{
		thisCollider = GetComponent<BoxCollider2D>();
		Assert.IsNotNull(thisCollider);
	}

	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		collidedWithBody = true;
		EventKit.Broadcast("player drowned", thisCollider);
	}

	override public void OnBodyCollisionStay() {}
	override public void OnBodyCollisionExit() {}
	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
