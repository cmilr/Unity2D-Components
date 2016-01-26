using Matcha.Unity;
using UnityEngine;

public class Hit
{
	public Collider2D coll;
	public Weapon weapon;
	public Weapon.WeaponType weaponType;
	public int hitFrom;

	public object Args(GameObject goThatReceivedHit, Collider2D coll)
	{
		hitFrom = M.HorizSideThatWasHit(goThatReceivedHit, coll);
		weapon = coll.GetComponent<Weapon>();
		weaponType = weapon.weaponType;

		return this;
	}
}
