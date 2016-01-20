using UnityEngine;

public class AnimationHandler : CacheBehaviour {

	void NewWeaponEquipped(Weapon.WeaponType weaponType)
	{
		//reset weapon animations
		animator.Rebind();

		//activate/deactivate specific animator layers when new weapons get equipped
		if (weaponType == Weapon.WeaponType.Sword)
		{
			animator.SetLayerWeight(2, 1);
			animator.SetLayerWeight(3, 0);
			animator.SetLayerWeight(4, 0);
		}
		else if (weaponType == Weapon.WeaponType.Axe)
		{
			animator.SetLayerWeight(2, 0);
			animator.SetLayerWeight(3, 1);
			animator.SetLayerWeight(4, 0);
		}
		else if (weaponType == Weapon.WeaponType.Hammer)
		{
			animator.SetLayerWeight(2, 0);
			animator.SetLayerWeight(3, 0);
			animator.SetLayerWeight(4, 1);
		}
	}
}
