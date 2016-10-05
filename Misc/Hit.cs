using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class Hit
{
	public Collider2D coll;
	public Weapon weapon;
	public Weapon.Type weaponType;
	public int hitSideHoriz;
	public int hitSideVert;

	public object PackageUp(GameObject objectThatWasHit, Collider2D wasHitBy)
	{
		weapon       = wasHitBy.GetComponent<Weapon>();
		Assert.IsNotNull(weapon);
		weaponType   = weapon.type;
		hitSideHoriz = M.HorizontalSideHit(objectThatWasHit, wasHitBy);
		hitSideVert  = M.VerticalSideHit(objectThatWasHit, wasHitBy);

		return this;
	}
}
