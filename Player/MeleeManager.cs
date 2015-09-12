using UnityEngine;
using System.Collections;

public class MeleeManager : CacheBehaviour {

    private float rateOfAttack = .3f;
    private BoxCollider2D boxCollider;
    private static bool inProgress;

    // when it changes, grab a reference to the currently equipped weapon's collider
    void OnInitEquippedWeapon(GameObject weapon)
    {
        boxCollider = weapon.GetComponent<BoxCollider2D>();
    }

    void OnChangeEquippedWeapon(GameObject weapon)
    {
        boxCollider = weapon.GetComponent<BoxCollider2D>();
    }

    public void Attack()
    {
        if (!inProgress)
        {

            Debug.Log("xxx DEBUG xxx");

            inProgress = true;
            boxCollider.enabled = true;

            StartCoroutine(Timer.Start(rateOfAttack, false, () =>
            {
                boxCollider.enabled = false;
                inProgress = false;
            }));
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
