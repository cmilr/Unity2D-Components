using UnityEngine;

public class MeleeManager : CacheBehaviour
{
	private float nextAttack;

	public void Attack(Weapon equippedWeapon)
	{
		equippedWeapon.GetComponentInChildren<MeleeCollider>().DisableMeleeCollider();

		if (Time.time > nextAttack)
		{
			equippedWeapon.GetComponentInChildren<MeleeCollider>().EnableMeleeCollider();
			nextAttack = Time.time + equippedWeapon.rateOfAttack;
		}
	}
}
