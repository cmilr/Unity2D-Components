using Matcha.Unity;
using UnityEngine;

public class Hit
{
	public Collider2D coll;
	public Weapon weapon;
	public Weapon.WeaponType weaponType;
	public int hitFrom;

	public object Args(GameObject collidee, Collider2D collider)
	{
		hitFrom = M.HorizSideThatWasHit(collidee, collider);
		weapon = collider.GetComponent<Weapon>();
		weaponType = weapon.weaponType;

		return this;
	}
}
