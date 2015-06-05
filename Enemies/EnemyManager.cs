using UnityEngine;
using System.Collections;

public class EnemyManager : CacheBehaviour {

    private ProjectileManager projectile;
    private Weapon weapon;
    private GameObject target;

	void Start()
    {
        projectile = GetComponent<ProjectileManager>();
        weapon = GetComponentInChildren<Weapon>();
        target = GameObject.Find("Player");
	}

    void OnBecameVisible()
    {
        InvokeRepeating("LookAtTarget", 1f, .3f);
        InvokeRepeating("AttackRandomly", 2f, 1f);
    }

    void OnBecameInvisible()
    {
        CancelInvoke("LookAtTarget");
        CancelInvoke("AttackRandomly");
    }

    void LookAtTarget()
    {
        int direction = (target.transform.position.x > transform.position.x) ? RIGHT : LEFT;
        transform.localScale = new Vector3((float)direction, transform.localScale.y, transform.localScale.z);
    }

    void AttackRandomly()
    {
        if (Random.Range(1, 11) <= 4)
            projectile.FireAtTarget(weapon, target);
    }
}
