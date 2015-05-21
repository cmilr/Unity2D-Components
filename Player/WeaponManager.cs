using UnityEngine;
using System.Collections;

public class WeaponManager : CacheBehaviour {

    private GameObject equippedWeapon;
    private GameObject leftWeapon;
    private GameObject rightWeapon;
    private GameObject tempWeapon;

    private Weapon equippedWeaponComponent;
    private Weapon leftWeaponComponent;
    private Weapon rightWeaponComponent;
    private ArmAnimation arm;

    void Start()
    {
        arm = GetComponentInChildren<ArmAnimation>();
    }

    void OnInitWeapons(GameObject eWeapon, GameObject lWeapon, GameObject rWeapon)
    {

        // WEAPON GAMEOBJECTS
        // ~~~~~~~~~~~~~~~~~~
        // keep track of weapon GameObjects as they're equipped/stashed
        equippedWeapon = eWeapon;
        leftWeapon     = lWeapon;
        rightWeapon    = rWeapon;
        tempWeapon     = rWeapon;


        // WEAPON GAMEOBJECT'S 'WEAPON' COMPONENT
        // ~~~~~~~~~~~~~~~~~~~~~~~~
        // cache specific weapons (Sword, Hammer, etc) via parent class 'Weapon'
        equippedWeaponComponent   = equippedWeapon.GetComponent<Weapon>();
        leftWeaponComponent       = leftWeapon.GetComponent<Weapon>();
        rightWeaponComponent      = rightWeapon.GetComponent<Weapon>();


        // disable animations for weapons that are not equipped
        leftWeaponComponent.EnableAnimation(false);
        rightWeaponComponent.EnableAnimation(false);


        PassWeaponObjectsToHUD();
    }

    void PassWeaponObjectsToHUD()
    {
        Messenger.Broadcast<GameObject>("init equipped weapon", equippedWeapon);
        Messenger.Broadcast<GameObject>("init stashed weapon left", leftWeapon);
        Messenger.Broadcast<GameObject>("init stashed weapon right", rightWeapon);
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
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<GameObject, GameObject, GameObject>( "init weapons", OnInitWeapons);
    }
}
