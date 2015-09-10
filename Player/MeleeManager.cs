using UnityEngine;
using System.Collections;

public class MeleeManager : CacheBehaviour {

    private WeaponCollider weaponCollider;
    private static bool inProgress;

	void Start()
    {
        weaponCollider = GetComponentInChildren<WeaponCollider>();
	}

    public void Attack()
    {
        if (!inProgress)
        {
            inProgress = true;
            weaponCollider.enabled = true;

            StartCoroutine(Timer.Start(.3f, false, () =>
            {
                Debug.Log("Attack Called");
                weaponCollider.enabled = false;
                inProgress = false;
            }));
        }
    }
}
