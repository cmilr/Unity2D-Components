using UnityEngine;
using System.Collections;

public abstract class Weapon : AnimationBehaviour {

    public enum WeaponType { Sword, Bow, Hammer, Projectile };
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

    // animation state methods
    public abstract void PlayIdleAnimation(float xOffset, float yOffset);
    public abstract void PlayRunAnimation(float xOffset, float yOffset);
    public abstract void PlayJumpAnimation(float xOffset, float yOffset);
    public abstract void PlaySwingAnimation(float xOffset, float yOffset);
    public abstract void EnableAnimation(bool status);
}
