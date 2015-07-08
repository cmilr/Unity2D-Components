using UnityEngine;
using System.Collections;

public class Projectile : Weapon {

    // used by Projectile GameObjects (ie, "Fireball," "Iceball," etc.)
    [Tooltip("If Animated, ProjectileContainer will attempt to load an animation.")]
    public bool animatedProjectile;
    [Tooltip("If Magic Weapon, projectile will fade in when thrown.")]
    public bool magicWeapon;

    override public void PlayIdleAnimation(float xOffset, float yOffset){}
    override public void PlayRunAnimation(float xOffset, float yOffset){}
    override public void PlayJumpAnimation(float xOffset, float yOffset){}
    override public void PlaySwingAnimation(float xOffset, float yOffset){}
    override public void EnableAnimation(bool status){}
}
