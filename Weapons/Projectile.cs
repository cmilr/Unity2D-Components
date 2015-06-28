using UnityEngine;
using System.Collections;

public class Projectile : Weapon {

    // used by Projectile GameObjects (ie, "Fireball," "Iceball," etc.)

    public bool animatedProjectile;

    override public void PlayIdleAnimation(float xOffset, float yOffset){}
    override public void PlayRunAnimation(float xOffset, float yOffset){}
    override public void PlayJumpAnimation(float xOffset, float yOffset){}
    override public void PlaySwingAnimation(float xOffset, float yOffset){}
    override public void EnableAnimation(bool status){}
}
