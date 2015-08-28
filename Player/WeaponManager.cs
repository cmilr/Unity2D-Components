using UnityEngine;
using System.Collections;
using DG.Tweening;
using Matcha.Dreadful.FX;

public class WeaponManager : CacheBehaviour {

    private ArmAnimation arm;
    private Weapon equippedWeapon;
    private Weapon leftWeapon;
    private Weapon rightWeapon;

    private int left = 0;
    private int equipped = 1;
    private int right = 2;
    private GameObject[] weaponBelt;

    private ProjectileManager projectile;

    void Start()
    {
        arm = GetComponentInChildren<ArmAnimation>();
        projectile = transform.parent.GetComponent<ProjectileManager>();
    }

    void OnInitWeapons(GameObject eWeapon, GameObject lWeapon, GameObject rWeapon)
    {
        // WEAPON GAMEOBJECTS — keep track of weapon GameObjects as they're equipped/stashed
        if(weaponBelt == null)
            weaponBelt = new GameObject[3];

        weaponBelt[left]     = lWeapon;
        weaponBelt[equipped] = eWeapon;
        weaponBelt[right]    = rWeapon;

        CacheAndSetupWeapons();
        PassInitialWeaponsToHUD();
    }

    void OnSwitchWeapon(int shiftDirection)
    {
        switch (equipped)
        {
            case 0:
            {
                left = 1;
                equipped = 2;
                right = 0;
                break;
            }

            case 1:
            {
                left = 2;
                equipped = 0;
                right = 1;
                break;
            }

            case 2:
            {
                left = 0;
                equipped = 1;
                right = 2;
                break;
            }
        }

        CacheAndSetupWeapons();
        PassNewWeaponsToHUD();
    }

    void CacheAndSetupWeapons()
    {
        // WEAPON GAMEOBJECT'S 'WEAPON' COMPONENT
        // ~~~~~~~~~~~~~~~~~~~~~~~~
        // cache specific weapons (Sword, Hammer, etc) via parent class 'Weapon'
        // use to call currently equipped weapon animations
        leftWeapon       = weaponBelt[left].GetComponent<Weapon>();
        equippedWeapon   = weaponBelt[equipped].GetComponent<Weapon>();
        rightWeapon      = weaponBelt[right].GetComponent<Weapon>();

        // set weapons to player's Weapon Collider layer
        weaponBelt[left].layer = 9;
        weaponBelt[equipped].layer = 9;
        weaponBelt[right].layer = 9;

        // disable animations for weapons that are not equipped
        leftWeapon.EnableAnimation(false);
        equippedWeapon.EnableAnimation(true);
        rightWeapon.EnableAnimation(false);

        // fade in newly equipped weapon
        float fadeAfter = 0f;
        float fadeTime  = .2f;

        SpriteRenderer upperSprite  = equippedWeapon.transform.Find("Upper").GetComponent<SpriteRenderer>();
        SpriteRenderer centerSprite = equippedWeapon.transform.Find("Center").GetComponent<SpriteRenderer>();
        SpriteRenderer lowerSprite  = equippedWeapon.transform.Find("Lower").GetComponent<SpriteRenderer>();

        upperSprite.DOKill();
        centerSprite.DOKill();
        lowerSprite.DOKill();

        MFX.Fade(upperSprite, 1f, fadeAfter, fadeTime);
        MFX.Fade(centerSprite, 1f, fadeAfter, fadeTime);
        MFX.Fade(lowerSprite, 1f, fadeAfter, fadeTime);

        // fade out newly stashed weapons
        FadeOutStashedWeapons(leftWeapon);
        FadeOutStashedWeapons(rightWeapon);
    }

    void FadeOutStashedWeapons(Weapon stashedWeapon)
    {
        float fadeAfter = 0f;
        float fadeTime  = .2f;

        SpriteRenderer upperSprite  = stashedWeapon.transform.Find("Upper").GetComponent<SpriteRenderer>();
        SpriteRenderer centerSprite = stashedWeapon.transform.Find("Center").GetComponent<SpriteRenderer>();
        SpriteRenderer lowerSprite  = stashedWeapon.transform.Find("Lower").GetComponent<SpriteRenderer>();

        upperSprite.DOKill();
        centerSprite.DOKill();
        lowerSprite.DOKill();

        MFX.Fade(upperSprite, 0f, fadeAfter, fadeTime);
        MFX.Fade(centerSprite, 0f, fadeAfter, fadeTime);
        MFX.Fade(lowerSprite, 0f, fadeAfter, fadeTime);
    }

    void PassInitialWeaponsToHUD()
    {
        Messenger.Broadcast<GameObject, int>("init stashed weapon", weaponBelt[left], LEFT);
        Messenger.Broadcast<GameObject>("init equipped weapon", weaponBelt[equipped]);
        Messenger.Broadcast<GameObject, int>("init stashed weapon", weaponBelt[right], RIGHT);
    }

    void PassNewWeaponsToHUD()
    {
        Messenger.Broadcast<GameObject, int>("change stashed weapon", weaponBelt[left], LEFT);
        Messenger.Broadcast<GameObject>("change equipped weapon", weaponBelt[equipped]);
        Messenger.Broadcast<GameObject, int>("change stashed weapon", weaponBelt[right], RIGHT);
    }

    // ANIMATION DISPATCHER
    public void AnimationDispatcher(int animationAction)
    {
        switch (equippedWeapon.weaponType)
        {
            // swinging weapons
            case Weapon.WeaponType.Axe:
            case Weapon.WeaponType.Sword:
            {
                PlaySwordAnimation(animationAction);
                break;
            }

            // hurled weapons
            case Weapon.WeaponType.Hammer:
            case Weapon.WeaponType.Dagger:
            {
                PlayHurledProjectileAnimation(animationAction);
                break;
            }

            // magic projectile weapons
            case Weapon.WeaponType.MagicProjectile:
            {
                PlayMagicProjectileAnimation(animationAction);
                break;
            }

            default:
            {
                Debug.Log("ERROR: No animationAction was set in WeaponManager.cs >> AnimationDispatcher()");
                break;
            }
        }
    }

    // SWINGING WEAPONS — swords, axes, etc
    // mix & match animations for various activity states
    public void PlaySwordAnimation(int animationAction)
    {
        switch (animationAction)
        {
            case IDLE:
            {
                equippedWeapon.PlayIdleAnimation(0, 0);
                arm.PlayIdleAnimation(0, 0);
                break;
            }

            case RUN:
            {
                equippedWeapon.PlayRunAnimation(0, 0);
                arm.PlayRunAnimation(0, 0);
                break;
            }

            case JUMP:
            {
                equippedWeapon.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case FALL:
            {
                equippedWeapon.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case ATTACK:
            {
                equippedWeapon.PlayAttackAnimation(0, 0);
                arm.PlayAttackAnimation(0, 0);
                break;
            }

            case RUN_ATTACK:
            {
                equippedWeapon.PlayAttackAnimation(0, ONE_PIXEL);
                arm.PlayAttackAnimation(0, ONE_PIXEL);
                break;
            }

            case JUMP_ATTACK:
            {
                equippedWeapon.PlayAttackAnimation(0, ONE_PIXEL * 2);
                arm.PlayAttackAnimation(0, ONE_PIXEL * 2);
                break;
            }

            default:
            {
                Debug.Log("ERROR: No animationAction was set in WeaponManager.cs >> PlayWeaponAnimation()");
                break;
            }
        }
    }

    // HURLED WEAPONS — hammers, daggers, etc
    // mix & match animations for various activity states
    public void PlayHurledProjectileAnimation(int animationAction)
    {
        switch (animationAction)
        {
            case IDLE:
            {
                equippedWeapon.PlayIdleAnimation(0, 0);
                arm.PlayIdleAnimation(0, 0);
                break;
            }

            case RUN:
            {
                equippedWeapon.PlayRunAnimation(0, 0);
                arm.PlayRunAnimation(0, 0);
                break;
            }

            case JUMP:
            {
                equippedWeapon.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case FALL:
            {
                equippedWeapon.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case ATTACK:
            {
                equippedWeapon.PlayAttackAnimation(0, 0);
                arm.PlayHurlAnimation(0, 0);
                projectile.Fire(equippedWeapon);
                break;
            }

            case RUN_ATTACK:
            {
                equippedWeapon.PlayAttackAnimation(0, ONE_PIXEL);
                arm.PlayHurlAnimation(0, ONE_PIXEL);
                projectile.Fire(equippedWeapon);
                break;
            }

            case JUMP_ATTACK:
            {
                equippedWeapon.PlayAttackAnimation(0, ONE_PIXEL * 2);
                arm.PlayHurlAnimation(0, ONE_PIXEL * 2);
                projectile.Fire(equippedWeapon);
                break;
            }

            default:
            {
                Debug.Log("ERROR: No animationAction was set in WeaponManager.cs >> PlayProjectileAnimation()");
                break;
            }
        }
    }

    // MAGIC PROJECTILE WEAPONS — fireballs, flaming skulls, etc
    // mix & match animations for various activity states
    public void PlayMagicProjectileAnimation(int animationAction)
    {
        switch (animationAction)
        {
            case IDLE:
            {
                equippedWeapon.PlayIdleAnimation(0, 0);
                arm.PlayIdleAnimation(0, 0);
                break;
            }

            case RUN:
            {
                equippedWeapon.PlayRunAnimation(0, 0);
                arm.PlayRunAnimation(0, 0);
                break;
            }

            case JUMP:
            {
                equippedWeapon.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case FALL:
            {
                equippedWeapon.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case ATTACK:
            {
                equippedWeapon.PlayIdleAnimation(0, 0);
                arm.PlayAttackAnimation(0, 0);
                projectile.Fire(equippedWeapon);
                break;
            }

            case RUN_ATTACK:
            {
                equippedWeapon.PlayAttackAnimation(0, ONE_PIXEL);
                arm.PlayAttackAnimation(0, ONE_PIXEL);
                projectile.Fire(equippedWeapon);
                break;
            }

            case JUMP_ATTACK:
            {
                equippedWeapon.PlayAttackAnimation(0, ONE_PIXEL * 2);
                arm.PlayAttackAnimation(0, ONE_PIXEL * 2);
                projectile.Fire(equippedWeapon);
                break;
            }

            default:
            {
                Debug.Log("ERROR: No animationAction was set in WeaponManager.cs >> PlayProjectileAnimation()");
                break;
            }
        }
    }

    void OnEnable()
    {
        Messenger.AddListener<GameObject, GameObject, GameObject>( "init weapons", OnInitWeapons);
        Messenger.AddListener<int>( "switch weapon", OnSwitchWeapon);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<GameObject, GameObject, GameObject>( "init weapons", OnInitWeapons);
        Messenger.RemoveListener<int>( "switch weapon", OnSwitchWeapon);
    }
}
