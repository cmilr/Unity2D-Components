using UnityEngine;
using System.Collections;
using Matcha.Game.Colors;

public class Projectile : Weapon {

    override public void PlayIdleAnimation(float xOffset, float yOffset) {}
    override public void PlayRunAnimation(float xOffset, float yOffset) {}
    override public void PlayJumpAnimation(float xOffset, float yOffset) {}
    override public void PlaySwingAnimation(float xOffset, float yOffset) {}
    override public void EnableAnimation(bool status) {}
}
