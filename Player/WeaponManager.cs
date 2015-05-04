using UnityEngine;
using System.Collections;

public class WeaponManager : CacheBehaviour {

    private GameObject weapon1;
    private GameObject weapon2;
    private GameObject weapon3;

    private GameObject equippedWeapon;
    private GameObject leftWeapon;
    private GameObject rightWeapon;

    private Weapon equippedComp;
    private Weapon leftComp;
    private Weapon rightComp;
    private ArmAnimation arm;

    void Start()
    {
        arm = GetComponentInChildren<ArmAnimation>();
    }

    void OnInitWeapons(GameObject eWeapon, GameObject lWeapon, GameObject rWeapon)
    {
        // keep track of weapons via their permanent slots
        weapon1 = eWeapon;
        weapon2 = lWeapon;
        weapon3 = rWeapon;

        // keep track of weapons as they're equipped/stashed
        equippedWeapon = weapon1;
        leftWeapon     = weapon2;
        rightWeapon    = weapon3;

        // cache specific weapon components (Sword, etc) via parent class 'Weapon'
        equippedComp   = equippedWeapon.GetComponent<Weapon>();
        leftComp       = leftWeapon.GetComponent<Weapon>();
        rightComp      = rightWeapon.GetComponent<Weapon>();

        // disable animations for weapons that are not equipped
        leftComp.EnableAnimation(false);
        rightComp.EnableAnimation(false);

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
                equippedComp.PlayIdleAnimation(0, 0);
                arm.PlayIdleAnimation(0, 0);
                break;
            }

            case RUN:
            {
                equippedComp.PlayRunAnimation(0, 0);
                arm.PlayRunAnimation(0, 0);
                break;
            }

            case JUMP:
            {
                equippedComp.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case FALL:
            {
                equippedComp.PlayJumpAnimation(0, 0);
                arm.PlayJumpAnimation(0, 0);
                break;
            }

            case ATTACK:
            {
                equippedComp.PlaySwingAnimation(0, 0);
                arm.PlaySwingAnimation(0, 0);
                break;
            }

            case RUN_ATTACK:
            {
                equippedComp.PlaySwingAnimation(0, ONE_PIXEL);
                arm.PlaySwingAnimation(0, ONE_PIXEL);
                break;
            }

            case JUMP_ATTACK:
            {
                equippedComp.PlaySwingAnimation(0, ONE_PIXEL * 2);
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
