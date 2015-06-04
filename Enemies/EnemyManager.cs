using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    private ProjectileManager projectile;
    private Weapon weapon;
    private GameObject target;

	void Start()
    {
        projectile = GetComponent<ProjectileManager>();
        weapon = GetComponentInChildren<Weapon>();
        target = GameObject.Find("Player");

        InvokeRepeating("Attack", 2f, 2f);
	}

    void Attack()
    {
        projectile.FireAtTarget(weapon, target);
    }
}
