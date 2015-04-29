using UnityEngine;
using System.Collections;

public class WeaponManager : CacheBehaviour {

    private GameObject weapon1;
    private GameObject weapon2;
    private GameObject weapon3;

    void Start()
    {
        weapon1 = GameObject.Find("Player/WeaponManager/Slot1/Weapon");
        weapon2 = GameObject.Find("Player/WeaponManager/Slot2/Weapon");
        weapon3 = GameObject.Find("Player/WeaponManager/Slot3/Weapon");


        Debug.Log(weapon1.GetComponent<WeaponBehaviour>().title);
    }
}
