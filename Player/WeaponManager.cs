using UnityEngine;
using System.Collections;

public class WeaponManager : CacheBehaviour {

    private GameObject weapon1;
    private GameObject weapon2;
    private GameObject weapon3;

    private Weapon equippedWeapon;
    private Weapon leftWeapon;
    private Weapon rightWeapon;

    private ArmAnimation arm;

    void Start()
    {
        InitWeapons();
        arm = GetComponentInChildren<ArmAnimation>();
    }

    void InitWeapons()
    {
        // populate weapon gameObject variables
        weapon1 = GameObject.Find("Player/WeaponManager/Slot1/Weapon");
        weapon2 = GameObject.Find("Player/WeaponManager/Slot2/Weapon");
        weapon3 = GameObject.Find("Player/WeaponManager/Slot3/Weapon");

        // get specific weapon components (Sword, etc) via parent class Weapon
        equippedWeapon = weapon1.GetComponent<Weapon>();
        leftWeapon = weapon2.GetComponent<Weapon>();
        rightWeapon = weapon3.GetComponent<Weapon>();

        // disable animations for weapons that are not equipped
        leftWeapon.EnableAnimation(false);
        rightWeapon.EnableAnimation(false);

        Invoke("PassWeaponObjectsToHUD", .01f);
    }

    void PassWeaponObjectsToHUD()
    {
        Messenger.Broadcast<GameObject>("init equipped weapon", weapon1);
        Messenger.Broadcast<GameObject>("init stashed weapon left", weapon2);
        Messenger.Broadcast<GameObject>("init stashed weapon right", weapon3);
    }

    // mix & match animations for various activity states
    public void PlayAnimation(int animationAction)
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
                Debug.Log("ERROR: No animationAction was set in WeaponManager.cs >> PlayAnimation()");
                break;
            }
        }
    }
}
