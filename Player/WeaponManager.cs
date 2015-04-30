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
        weapon1 = GameObject.Find("Player/WeaponManager/Slot1/Weapon");
        weapon2 = GameObject.Find("Player/WeaponManager/Slot2/Weapon");
        weapon3 = GameObject.Find("Player/WeaponManager/Slot3/Weapon");

        equippedWeapon = weapon1.GetComponent<Weapon>();
        leftWeapon = weapon2.GetComponent<Weapon>();
        rightWeapon = weapon3.GetComponent<Weapon>();

        leftWeapon.EnableAnimation(false);
        rightWeapon.EnableAnimation(false);
    }

    // mix & match animations for various activity states
    public void PlayAnimation(int animationAction)
    {
        switch (animationAction)
        {
            case IDLE:
            {
                arm.PlayIdleAnimation(0, 0);
                equippedWeapon.PlayIdleAnimation(0, 0);
                break;
            }

            case RUN:
            {
                arm.PlayRunAnimation(0, 0);
                equippedWeapon.PlayRunAnimation(0, 0);
                break;
            }

            case JUMP:
            {
                arm.PlayJumpAnimation(0, 0);
                equippedWeapon.PlayJumpAnimation(0, 0);
                break;
            }

            case FALL:
            {
                arm.PlayJumpAnimation(0, 0);
                equippedWeapon.PlayJumpAnimation(0, 0);
                break;
            }

            case ATTACK:
            {
                arm.PlaySwingAnimation(0, 0);
                equippedWeapon.PlaySwingAnimation(0, 0);
                break;
            }

            case RUN_ATTACK:
            {
                arm.PlaySwingAnimation(0, ONE_PIXEL);
                equippedWeapon.PlaySwingAnimation(0, ONE_PIXEL);
                break;
            }

            case JUMP_ATTACK:
            {
                arm.PlaySwingAnimation(0, ONE_PIXEL * 2);
                equippedWeapon.PlaySwingAnimation(0, ONE_PIXEL * 2);
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
