using UnityEngine;

public class MeleeManager : CacheBehaviour
{
	private float nextAttack;

	public void Attack(Weapon equippedWeapon)
	{
		equippedWeapon.GetComponent<WeaponCollider>().DisableWeaponCollider();

		if (Time.time > nextAttack)
		{
			equippedWeapon.GetComponent<WeaponCollider>().EnableWeaponCollider();
			nextAttack = Time.time + equippedWeapon.rateOfAttack;
		}
	}
}
