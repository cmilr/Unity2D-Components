using UnityEngine;
using System.Collections;
using Matcha.Game.Colors;

public class Sword : CacheBehaviour, IWeapon {

    private WeaponComponent blade;
    private WeaponComponent hilt;
    private WeaponComponent handle;
    private bool alreadyOffset;

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

    public void OffsetAnimationBy(float offset)
    {
        if (!alreadyOffset)
        {
            transform.position = new Vector3(transform.position.x, (transform.position.y + ONE_PIXEL), transform.position.z);
        }

        alreadyOffset = true;
    }
}
