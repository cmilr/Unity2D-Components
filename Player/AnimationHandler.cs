using UnityEngine;

public class AnimationHandler : CacheBehaviour {

	void NewWeaponEquipped(int weaponType)
	{
		//reset weapon animations
		animator.Rebind();

		//activate/deactivate specific animator layers when new weapons get equipped
		if (weaponType == SWORD)
		{
			animator.SetLayerWeight(2, 1);
			animator.SetLayerWeight(3, 0);
			animator.SetLayerWeight(4, 0);
		}
		else if (weaponType == AXE)
		{
			animator.SetLayerWeight(2, 0);
			animator.SetLayerWeight(3, 1);
			animator.SetLayerWeight(4, 0);
		}
		else if (weaponType == HAMMER)
		{
			animator.SetLayerWeight(2, 0);
			animator.SetLayerWeight(3, 0);
			animator.SetLayerWeight(4, 1);
		}
	}
}
