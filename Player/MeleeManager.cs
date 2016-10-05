using UnityEngine;
using UnityEngine.Assertions;

public class MeleeManager : BaseBehaviour
{
	private MeleeCollider meleeCollider;
	private float nextAttack;

	public void Attack(Weapon equippedWeapon)
	{
		meleeCollider = equippedWeapon.GetComponentInChildren<MeleeCollider>();
		Assert.IsNotNull(meleeCollider);

		if (Time.time > nextAttack)
		{
			meleeCollider.AttemptAttack(equippedWeapon);
			nextAttack = Time.time + equippedWeapon.rateOfAttack;
		}
	}
}
