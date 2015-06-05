using UnityEngine;
using System.Collections;

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
        // WEAPON GAMEOBJECTS
        // ~~~~~~~~~~~~~~~~~~
        // keep track of weapon GameObjects as they're equipped/stashed
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

        // disable animations for weapons that are not equipped
        leftWeapon.EnableAnimation(false);
        equippedWeapon.EnableAnimation(true);
        rightWeapon.EnableAnimation(false);
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

    // mix & match animations for various activity states
    public void PlayAnimation(int animationAction)
    {
        switch (equippedWeapon.weaponType)
        {
            case Weapon.WeaponType.Sword:
            {
                PlaySwordAnimation(animationAction);
                break;
            }

            case Weapon.WeaponType.Projectile:
            {
                PlayProjectileAnimation(animationAction);
                break;
            }

            default:
            {
                Debug.Log("ERROR: No animationAction was set in WeaponManager.cs >> PlayAnimation()");
                break;
            }
        }
    }

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
                equippedWeapon.PlaySwingAnimation(0, 0);
                arm.PlaySwingAnimation(0, 0);
                break;
            }

            case RUN_ATTACK:
            {
                equippedWeapon.PlaySwingAnimation(0, ONE_PIXEL);
                arm.PlaySwingAnimation(0, ONE_PIXEL);
                break;
            }

            case JUMP_ATTACK:
            {
                equippedWeapon.PlaySwingAnimation(0, ONE_PIXEL * 2);
                arm.PlaySwingAnimation(0, ONE_PIXEL * 2);
                break;
            }

            default:
            {
                Debug.Log("ERROR: No animationAction was set in WeaponManager.cs >> PlayWeaponAnimation()");
                break;
            }
        }
    }

    // mix & match animations for various activity states
    public void PlayProjectileAnimation(int animationAction)
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
                arm.PlayIdleAnimation(0, 0);
                projectile.Fire(equippedWeapon);
                break;
            }

            case RUN_ATTACK:
            {
                equippedWeapon.PlaySwingAnimation(0, ONE_PIXEL);
                arm.PlaySwingAnimation(0, ONE_PIXEL);
                break;
            }

            case JUMP_ATTACK:
            {
                equippedWeapon.PlaySwingAnimation(0, ONE_PIXEL * 2);
                arm.PlaySwingAnimation(0, ONE_PIXEL * 2);
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
