using UnityEngine;
using System.Collections;

public class MeleeManager : CacheBehaviour {

    private WeaponCollider weaponCollider;
    private BoxCollider2D boxCollider;
    private static bool inProgress;

	void Start()
    {
        boxCollider = transform.Find("WeaponCollider").GetComponent<BoxCollider2D>();
	}

    public void Attack()
    {
        if (!inProgress)
        {
            inProgress = true;
            boxCollider.enabled = true;

            StartCoroutine(Timer.Start(.3f, false, () =>
            {
                boxCollider.enabled = false;
                inProgress = false;
            }));
        }
    }
}
