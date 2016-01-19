using Matcha.Unity;
using UnityEngine;

public class Hit
{
	public Collider2D coll;
	public Weapon weapon;
	public Weapon.WeaponType weaponType;
	public int hitFrom;

	public object Args(GameObject go, Collider2D coll)
	{
		hitFrom = M.HorizSideThatWasHit(go, coll);
		weapon = coll.GetComponent<Weapon>();
		weaponType = weapon.weaponType;

		return this;
	}
}

//for sending multiple arguments in a unity SendMessage call
//
//example usage:
//private Hit hit;
//hit = new Hit();
//
//gameObject.SendMessage("PlayerHit", hit.Args(gameObject, coll));
