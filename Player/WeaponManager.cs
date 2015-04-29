using UnityEngine;
using System.Collections;

public class WeaponManager : CacheBehaviour {

    private GameObject weapon1;
    private GameObject weapon2;
    private GameObject weapon3;

    private IWeapon equipped;
    private WeaponBehaviour leftWeapon;
    private WeaponBehaviour rightWeapon;

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

        equipped = weapon1.GetComponent<IWeapon>();
        leftWeapon = weapon2.GetComponent<WeaponBehaviour>();
        rightWeapon = weapon3.GetComponent<WeaponBehaviour>();

        Debug.Log(leftWeapon.title);
        Debug.Log(rightWeapon.title);
        // leftWeapon.EnableAnimation(false);
        // rightWeapon.EnableAnimation(false);
    }



    // mix & match animations for various activity states
    public void PlayAnimation(int animationAction)
    {
        switch (animationAction)
        {
            case IDLE:
            {
                arm.PlayIdleAnimation(0, 0);
                equipped.PlayIdleAnimation(0, 0);
                break;
            }

            case RUN:
            {
                arm.PlayRunAnimation(0, 0);
                equipped.PlayRunAnimation(0, 0);
                break;
            }

            case JUMP:
            {
                arm.PlayJumpAnimation(0, 0);
                equipped.PlayJumpAnimation(0, 0);
                break;
            }

            case FALL:
            {
                arm.PlayJumpAnimation(0, 0);
                equipped.PlayJumpAnimation(0, 0);
                break;
            }

            case ATTACK:
            {
                arm.PlaySwingAnimation(0, 0);
                equipped.PlaySwingAnimation(0, 0);
                break;
            }

            case RUN_ATTACK:
            {
                arm.PlaySwingAnimation(0, ONE_PIXEL);
                equipped.PlaySwingAnimation(0, ONE_PIXEL);
                break;
            }

            case JUMP_ATTACK:
            {
                arm.PlaySwingAnimation(0, ONE_PIXEL * 2);
                equipped.PlaySwingAnimation(0, ONE_PIXEL * 2);
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
