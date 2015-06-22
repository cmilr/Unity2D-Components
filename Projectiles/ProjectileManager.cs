using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : CacheBehaviour {

	private GameObject projectilePrefab;
    private Transform projectileSpawnPoint;
    private ProjectileContainer projectile;
    private float fireRate;
    private float nextFire;
    private bool player;

    void Start()
    {
        projectileSpawnPoint = GetComponentInChildren<SpawnPointTrace>().transform;
        player = (gameObject.layer == PLAYER_LAYER) ? true : false;
    }

    // fire in direction actor is facing
    public void Fire(Weapon equippedWeapon)
    {
        fireRate = equippedWeapon.rateOfAttack;

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject obj = ProjectilePool.current.Spawn();

            if (obj == null)
            {
                Debug.Log("ERROR in ProjectileManager.Fire()");
            }

            obj.transform.position = projectileSpawnPoint.position;
            obj.transform.rotation = projectileSpawnPoint.rotation;
            obj.SetActive(true);

            // flip projectile sprite so it's pointing the same direction as the actor
            obj.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            obj.layer = (player) ? WEAPON_COLLIDER : ENEMY_WEAPON;
            obj.GetComponent<ProjectileContainer>().Fire(equippedWeapon, transform.localScale.x);
        }

    }

    // fire specifically at target
    public void FireAtTarget(Weapon equippedWeapon, Transform target)
    {
        fireRate = equippedWeapon.rateOfAttack;

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject obj = ProjectilePool.current.Spawn();

            if (obj == null)
            {
                Debug.Log("ERROR in ProjectileManager.FireAtTarget()");
            }

            obj.transform.position = projectileSpawnPoint.position;
            obj.transform.rotation = projectileSpawnPoint.rotation;
            obj.SetActive(true);

            // flip projectile sprite so it's pointing the same direction as the actor
            obj.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            obj.layer = (player) ? WEAPON_COLLIDER : ENEMY_WEAPON;
            obj.GetComponent<ProjectileContainer>().Fire(equippedWeapon, target);
        }
    }
}