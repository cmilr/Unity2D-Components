using UnityEngine;
using System.Collections;

public class WeaponManager : CacheBehaviour {

<<<<<<< HEAD
=======
    private GameObject equippedWeapon;
    private GameObject leftWeapon;
    private GameObject rightWeapon;
    private GameObject tempWeapon;

    private Weapon equippedWeaponComponent;
    private Weapon leftWeaponComponent;
    private Weapon rightWeaponComponent;
>>>>>>> origin/master
    private ArmAnimation arm;
    private Weapon equippedWeaponComponent;
    private Weapon leftWeaponComponent;
    private Weapon rightWeaponComponent;

    private int left = 0;
    private int equipped = 1;
    private int right = 2;
    private GameObject[] weaponBelt;

    void Start()
    {
        arm = GetComponentInChildren<ArmAnimation>();
    }

    void OnInitWeapons(GameObject eWeapon, GameObject lWeapon, GameObject rWeapon)
    {
<<<<<<< HEAD
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
        leftWeaponComponent       = weaponBelt[left].GetComponent<Weapon>();
        equippedWeaponComponent   = weaponBelt[equipped].GetComponent<Weapon>();
        rightWeaponComponent      = weaponBelt[right].GetComponent<Weapon>();

        // disable animations for weapons that are not equipped
        leftWeaponComponent.EnableAnimation(false);
        equippedWeaponComponent.EnableAnimation(true);
        rightWeaponComponent.EnableAnimation(false);
    }
=======

        // WEAPON GAMEOBJECTS
        // ~~~~~~~~~~~~~~~~~~
        // keep track of weapon GameObjects as they're equipped/stashed
        equippedWeapon = eWeapon;
        leftWeapon     = lWeapon;
        rightWeapon    = rWeapon;
        tempWeapon     = rWeapon;


        // PLAYER WEAPON ANIMATIONS
        // ~~~~~~~~~~~~~~~~~~~~~~~~
        // cache specific weapons (Sword, Hammer, etc) via parent class 'Weapon'
        equippedWeaponComponent   = equippedWeapon.GetComponent<Weapon>();
        leftWeaponComponent       = leftWeapon.GetComponent<Weapon>();
        rightWeaponComponent      = rightWeapon.GetComponent<Weapon>();


        // disable animations for weapons that are not equipped
        leftWeaponComponent.EnableAnimation(false);
        rightWeaponComponent.EnableAnimation(false);

>>>>>>> origin/master

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
        switch (animationAction)
        {
            case IDLE:
            {
                equippedWeaponComponent.PlayIdleAnimation(0, 0);
                arm.PlayIdleAnimation(0, 0);
                break;
            }

            case RUN:
            {
                equippedWeaponComponent.PlayRunAnimation(0, 0);
                arm.PlayRunAnimation(0, 0);
                break;
            }

            case JUMP:
            {
                equippedWeaponComponent.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case FALL:
            {
                equippedWeaponComponent.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case ATTACK:
            {
                equippedWeaponComponent.PlaySwingAnimation(0, 0);
                arm.PlaySwingAnimation(0, 0);
                break;
            }

            case RUN_ATTACK:
            {
                equippedWeaponComponent.PlaySwingAnimation(0, ONE_PIXEL);
                arm.PlaySwingAnimation(0, ONE_PIXEL);
                break;
            }

            case JUMP_ATTACK:
            {
                equippedWeaponComponent.PlaySwingAnimation(0, ONE_PIXEL * 2);
                arm.PlaySwingAnimation(0, ONE_PIXEL * 2);
                break;
            }

            default:
            {
                Debug.Log("ERROR: No animationAction was set in WeaponManager.cs >> PlayAnimation()");
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
