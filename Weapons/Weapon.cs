using UnityEngine;
using System.Collections;
using Matcha.Game.Colors;

public class Weapon : AnimationBehaviour {

    public enum WeaponType { Axe, Sword, Hammer, HurledProjectile, MagicProjectile };
    public WeaponType weaponType;
    public Sprite sprite;

    // note: ProjectileContainers contain simple dummy values since they
    // receive data for these fields via passed-in projectile objects

    [HideInInspector]
    public bool alreadyCollided;

    [Header("All Weapons")]
    public string title;
    public int hp;
    public int ac;
    public int damage;
    public float rateOfAttack;

    [Header("Ranged Weapons")]
    [Range (8, 20)]
    public float speed = 12f;
    public float maxDistance = 40f;

    [Tooltip("zero mass will be fired linearly, positive mass will be lobbed at its target")]
    public float mass = 1f;

    [Tooltip("If Animated, ProjectileContainer will attempt to load an animation.")]
    public bool animatedProjectile;
    [Tooltip("If Magic Weapon, projectile will fade in when thrown.")]
    public bool magicWeapon;

    // genericized weapon pieces
    private WeaponPiece upper;
    private WeaponPiece center;
    private WeaponPiece lower;

    void Awake ()
    {
        // if projectile is being carried by the player (as opposed to an enemy,)
        // animate the weapon while player is walking, jumping, etc
        if (transform.parent != null)
        {
            if (transform.parent.name == "Inventory")
            {
                // set weapon components on initialization
                upper  = transform.FindChild("Upper").gameObject.GetComponent<WeaponPiece>();
                center = transform.FindChild("Center").gameObject.GetComponent<WeaponPiece>();
                lower  = transform.FindChild("Lower").gameObject.GetComponent<WeaponPiece>();

                // set weapon colors here
                upper.spriteRenderer.material.SetColor("_Color", MColor.white);
                center.spriteRenderer.material.SetColor("_Color", MColor.defaultGrayHandle);
                lower.spriteRenderer.material.SetColor("_Color", MColor.defaultGrayHandle);
            }
        }
    }

    // animation state methods
    public void PlayIdleAnimation(float xOffset, float yOffset)
    {
        upper.PlayIdleAnimation();
        center.PlayIdleAnimation();
        lower.PlayIdleAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlayRunAnimation(float xOffset, float yOffset)
    {
        upper.PlayRunAnimation();
        center.PlayRunAnimation();
        lower.PlayRunAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlayJumpAnimation(float xOffset, float yOffset)
    {
        upper.PlayJumpAnimation();
        center.PlayJumpAnimation();
        lower.PlayJumpAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlaySwingAnimation(float xOffset, float yOffset)
    {
        upper.PlaySwingAnimation();
        center.PlaySwingAnimation();
        lower.PlaySwingAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void EnableAnimation(bool status)
    {
        upper.spriteRenderer.enabled = status;
        center.spriteRenderer.enabled = status;
        lower.spriteRenderer.enabled = status;
    }
}
