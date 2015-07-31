using UnityEngine;
using System.Collections;
using Matcha.Game.Colors;

public class Weapon : AnimationBehaviour {

    public enum WeaponType { Axe, Sword, Hammer, HurledProjectile, MagicProjectile };
    public WeaponType weaponType;

    [HideInInspector]
    public bool alreadyCollided;

    [Tooltip("This is the pickup/HUD icon.")]
    public Sprite iconSprite;


    [Header("ALL WEAPONS")]
    //~~~~~~~~~~~~~~~~~~~~~
    [Tooltip("What title should be displayed when this weapon is equipped?")]
    public string title;

    [Tooltip("How much damage does this weapon do?")]
    public int damage;

    [Tooltip("How many hits can this weapon take before it's unuseable?")]
    public int hp;

    [Tooltip("How many times per second can this weapon be fired?")]
    public float rateOfAttack;


    [Header("RANGED WEAPONS ONLY")]
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    [Tooltip("Projectile sprite that will actually be fired.")]
    public Sprite projectileSprite;

    [Range (8, 20)]
    [Tooltip("How fast should projectiel travel?")]
    public float speed = 12f;

    [Tooltip("How far should projectile travel before fading out?")]
    public float maxDistance = 40f;

    [Tooltip("Should projectile be lobbed?")]
    public bool lob;

    [Tooltip("ONLY EFFECTS PLAYER: If 'Lob' is true, how much should gravity effect projectile?")]
    public float lobGravity;

    [Tooltip("Should projectile fade in when thrown?")]
    public bool fadeIn;

    [Tooltip("If Animated, ProjectileContainer will attempt to load an animation.")]
    public bool animatedProjectile;


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

    public void PlayAttackAnimation(float xOffset, float yOffset)
    {
        upper.PlayAttackAnimation();
        center.PlayAttackAnimation();
        lower.PlayAttackAnimation();
        OffsetAnimation(xOffset, yOffset);
    }

    public void EnableAnimation(bool status)
    {
        upper.spriteRenderer.enabled = status;
        center.spriteRenderer.enabled = status;
        lower.spriteRenderer.enabled = status;
    }
}
