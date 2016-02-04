using UnityEngine;

public class MeleeManager : CacheBehaviour
{
	private MeleeCollider meleeCollider;
	private float nextAttack;

	public void Attack(Weapon equippedWeapon)
	{
		meleeCollider = equippedWeapon.GetComponentInChildren<MeleeCollider>();
		meleeCollider.DisableMeleeCollider();

		if (Time.time > nextAttack)
		{
			meleeCollider.EnableMeleeCollider();
			nextAttack = Time.time + equippedWeapon.rateOfAttack;
		}
	}
}
