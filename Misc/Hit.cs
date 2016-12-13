using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class Hit
{
	public Collider2D coll;
	public Weapon weapon;
	public Weapon.Type weaponType;
	public Side horizontalSide;
	public Side verticalSide;

	public object Create(GameObject objectThatWasHit, Collider2D wasHitBy)
	{
		weapon = wasHitBy.GetComponentInParent<Weapon>() ?? wasHitBy.GetComponentInParent<ProjectileContainer>().weapon;
		Assert.IsNotNull(weapon);

		weaponType     = weapon.type;
		horizontalSide = M.HorizontalSideHit(objectThatWasHit, wasHitBy);
		verticalSide   = M.VerticalSideHit(objectThatWasHit, wasHitBy);

		return this;
	}
}
