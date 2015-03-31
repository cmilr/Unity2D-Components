using UnityEngine;
using System.Collections;
using Matcha.Game.Colors;

public class Sword : CacheBehaviour, IWeapon {

    private WeaponComponent blade;
    private WeaponComponent hilt;
    private WeaponComponent handle;

	void Start ()
    {
        blade  = transform.FindChild("Blade").gameObject.GetComponent<WeaponComponent>();
        hilt   = transform.FindChild("Hilt").gameObject.GetComponent<WeaponComponent>();
        handle = transform.FindChild("Handle").gameObject.GetComponent<WeaponComponent>();

        blade.spriteRenderer.material.SetColor("_Color", MColor.orange);
        hilt.spriteRenderer.material.SetColor("_Color", MColor.deepBrightBlue);
        handle.spriteRenderer.material.SetColor("_Color", MColor.black);
	}

    public void PlayIdleAnimation()
    {
        blade.PlayIdleAnimation();
        hilt.PlayIdleAnimation();
        handle.PlayIdleAnimation();
    }

    public void PlayRunAnimation()
    {
        blade.PlayRunAnimation();
        hilt.PlayRunAnimation();
        handle.PlayRunAnimation();
    }

    public void PlayJumpAnimation()
    {
        blade.PlayJumpAnimation();
        hilt.PlayJumpAnimation();
        handle.PlayJumpAnimation();
    }

    public void PlaySwingAnimation()
    {
        blade.PlaySwingAnimation();
        hilt.PlaySwingAnimation();
        handle.PlaySwingAnimation();
    }
}
