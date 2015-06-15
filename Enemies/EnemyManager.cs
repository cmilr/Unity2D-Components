using UnityEngine;
using System.Collections;

public class EnemyManager : CacheBehaviour {

    public float attackInterval = 1f;
    public float chanceOfAttack = 40f;
    public float attackRange = 20f;

    private ProjectileManager projectile;
    private Weapon weapon;
    private Transform target;

	void Start()
    {
        projectile = GetComponent<ProjectileManager>();
        weapon = GetComponentInChildren<Weapon>();
        target = GameObject.Find("Player").transform;
	}

    void OnBecameVisible()
    {
        InvokeRepeating("LookAtTarget", 1f, .3f);
        InvokeRepeating("AttackRandomly", 2f, attackInterval);
        // InvokeRepeating("RotateTowardsTarget", 1f, .01f);
    }

    void OnBecameInvisible()
    {
        CancelInvoke("LookAtTarget");
        CancelInvoke("AttackRandomly");
        // CancelInvoke("RotateTowardsTarget");
    }

    void LookAtTarget()
    {
        int direction = (target.position.x > transform.position.x) ? RIGHT : LEFT;
        transform.localScale = new Vector3((float)direction, transform.localScale.y, transform.localScale.z);
    }

    void RotateTowardsTarget()
    {
        Vector3 vel = GetForceFrom(transform.position,target.position);
        float angle = Mathf.Atan2(vel.y,vel.x)* Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0,angle);
    }

    Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        float power = 1;
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y))*power;
    }

    void AttackRandomly()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= attackRange)
        {
            if (Random.Range(1, 101) <= chanceOfAttack)
                projectile.FireAtTarget(weapon, target);
        }
    }

}
