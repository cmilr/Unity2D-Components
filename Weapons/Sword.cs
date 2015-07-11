using UnityEngine;
using System.Collections;
using Matcha.Game.Colors;

public class Sword : Weapon {

    private WeaponPiece blade;
    private WeaponPiece hilt;
    private WeaponPiece handle;

	void Awake ()
    {
        // set weapon components on initialization
        blade  = transform.FindChild("Blade").gameObject.GetComponent<WeaponPiece>();
        hilt   = transform.FindChild("Hilt").gameObject.GetComponent<WeaponPiece>();
        handle = transform.FindChild("Handle").gameObject.GetComponent<WeaponPiece>();

        // set weapon colors here
        blade.spriteRenderer.material.SetColor("_Color", MColor.white);
        hilt.spriteRenderer.material.SetColor("_Color", MColor.defaultGrayHandle);
        handle.spriteRenderer.material.SetColor("_Color", MColor.defaultGrayHandle);
	}

    override public void PlayIdleAnimation(float xOffset, float yOffset)
    {
        blade.PlayIdleAnimation();
        hilt.PlayIdleAnimation();
        handle.PlayIdleAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    override public void PlayRunAnimation(float xOffset, float yOffset)
    {
        blade.PlayRunAnimation();
        hilt.PlayRunAnimation();
        handle.PlayRunAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    override public void PlayJumpAnimation(float xOffset, float yOffset)
    {
        blade.PlayJumpAnimation();
        hilt.PlayJumpAnimation();
        handle.PlayJumpAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    override public void PlaySwingAnimation(float xOffset, float yOffset)
    {
        blade.PlaySwingAnimation();
        hilt.PlaySwingAnimation();
        handle.PlaySwingAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    override public void EnableAnimation(bool status)
    {
        blade.spriteRenderer.enabled = status;
        hilt.spriteRenderer.enabled = status;
        handle.spriteRenderer.enabled = status;
    }
}
