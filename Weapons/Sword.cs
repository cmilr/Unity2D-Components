using UnityEngine;
using System.Collections;
using Matcha.Game.Colors;

public class Sword : WeaponBehaviour, IWeapon {

    private WeaponComponent blade;
    private WeaponComponent hilt;
    private WeaponComponent handle;

	void Start ()
    {
        // set weapon components on initialization
        blade  = transform.FindChild("Blade").gameObject.GetComponent<WeaponComponent>();
        hilt   = transform.FindChild("Hilt").gameObject.GetComponent<WeaponComponent>();
        handle = transform.FindChild("Handle").gameObject.GetComponent<WeaponComponent>();

        // set weapon colors here
        blade.spriteRenderer.material.SetColor("_Color", MColor.white);
        hilt.spriteRenderer.material.SetColor("_Color", MColor.defaultGrayHandle);
        handle.spriteRenderer.material.SetColor("_Color", MColor.defaultGrayHandle);
	}

    public void PlayIdleAnimation(float xOffset, float yOffset)
    {
        blade.PlayIdleAnimation();
        hilt.PlayIdleAnimation();
        handle.PlayIdleAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlayRunAnimation(float xOffset, float yOffset)
    {
        blade.PlayRunAnimation();
        hilt.PlayRunAnimation();
        handle.PlayRunAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlayJumpAnimation(float xOffset, float yOffset)
    {
        blade.PlayJumpAnimation();
        hilt.PlayJumpAnimation();
        handle.PlayJumpAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlaySwingAnimation(float xOffset, float yOffset)
    {
        blade.PlaySwingAnimation();
        hilt.PlaySwingAnimation();
        handle.PlaySwingAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void EnableAnimation(bool status)
    {
        blade.spriteRenderer.enabled = status;
        hilt.spriteRenderer.enabled = status;
        handle.spriteRenderer.enabled = status;
    }
}
