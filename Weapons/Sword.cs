using UnityEngine;
using System.Collections;

public class Sword : CacheBehaviour, IWeapon {

    private WeaponComponent blade;
    private WeaponComponent hilt;
    private WeaponComponent handle;

	void Start ()
    {
        blade  = transform.FindChild("Blade").gameObject.GetComponent<WeaponComponent>();
        hilt   = transform.FindChild("Hilt").gameObject.GetComponent<WeaponComponent>();
        handle = transform.FindChild("Handle").gameObject.GetComponent<WeaponComponent>();
	}

    public void PlayIdleAnimation()
    {
        blade.IdleAnimation();
        hilt.IdleAnimation();
        handle.IdleAnimation();
    }

    public void PlayRunAnimation()
    {
        blade.RunAnimation();
        hilt.RunAnimation();
        handle.RunAnimation();
    }

    public void PlayJumpAnimation()
    {
        blade.JumpAnimation();
        hilt.JumpAnimation();
        handle.JumpAnimation();
    }
}
