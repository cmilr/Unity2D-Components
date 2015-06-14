using UnityEngine;
using System.Collections;

public class EnemyManager : CacheBehaviour {

    public float attackInterval = 1f;
    public float chanceOfAttack = 40f;
    public float attackRange = 20f;

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
        InvokeRepeating("AttackRandomly", 2f, attackInterval);
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
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance <= attackRange)
        {
            if (Random.Range(1, 101) <= chanceOfAttack)
                projectile.FireAtTarget(weapon, target);
        }
    }
}
