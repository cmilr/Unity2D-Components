using UnityEngine;
using System.Collections;

public class MeleeManager : CacheBehaviour {

    private float nextAttack;
    private BoxCollider2D boxCollider;

    // when it changes, grab a reference to the currently equipped weapon's collider
    void OnInitEquippedWeapon(GameObject weapon)
    {
        boxCollider = weapon.GetComponent<BoxCollider2D>();
    }

    void OnChangeEquippedWeapon(GameObject weapon)
    {
        boxCollider = weapon.GetComponent<BoxCollider2D>();
    }

    public void Attack(Weapon equippedWeapon)
    {
        boxCollider.enabled = false;

        if (Time.time > nextAttack)
        {
            boxCollider.enabled = true;
            nextAttack = Time.time + equippedWeapon.rateOfAttack;
        }
    }

    void OnEnable()
    {
        Messenger.AddListener<GameObject>("init equipped weapon", OnInitEquippedWeapon);
        Messenger.AddListener<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<GameObject>("init equipped weapon", OnInitEquippedWeapon);
        Messenger.RemoveListener<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
    }
}
