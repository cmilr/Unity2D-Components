using UnityEngine;
using System.Collections;

public class WeaponBehaviour : AnimationBehaviour {

    public enum WeaponType { Sword, Bow, Hammer };
    public WeaponType weaponType;
    public string title;
    public int hp;
    public int ac;
    public int damage;

}
