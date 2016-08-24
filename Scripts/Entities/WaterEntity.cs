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
		EventKit.Broadcast<Collider2D>("player drowned", thisCollider);
		EventKit.Broadcast<int, Weapon.WeaponType>("player dead", -1, Weapon.WeaponType.Ignore);
	}

	override public void OnBodyCollisionStay() {}
	override public void OnBodyCollisionExit() {}
	override public void OnWeaponCollisionEnter(Collider2D coll) {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
