using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : CacheBehaviour {

	private GameObject projectilePrefab;
    private GameObject pooledProjectile;
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
            GetPooledProjectile(equippedWeapon);
            InitPooledProjectile();

            // call correct Fire method, based on type of weapon selected (magical or not)
            if (equippedWeapon.GetComponent<Weapon>().magicWeapon)
                pooledProjectile.GetComponent<MagicProjectileContainer>().Fire(equippedWeapon, transform.localScale.x);
            else
                pooledProjectile.GetComponent<ProjectileContainer>().Fire(equippedWeapon, transform.localScale.x);
        }

    }

    // fire specifically at target
    public void FireAtTarget(Weapon equippedWeapon, Transform target)
    {
        fireRate = equippedWeapon.rateOfAttack;

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GetPooledProjectile(equippedWeapon);
            InitPooledProjectile();

            // call correct Fire method, based on type of weapon selected (magical or not)
            if (equippedWeapon.GetComponent<Weapon>().magicWeapon)
                pooledProjectile.GetComponent<MagicProjectileContainer>().Fire(equippedWeapon, target);
            else
                pooledProjectile.GetComponent<ProjectileContainer>().Fire(equippedWeapon, target);
        }
    }

    void GetPooledProjectile(Weapon equippedWeapon)
    {
        // set correct Object Pool type based on type of weapon (magical or not)
        if (equippedWeapon.GetComponent<Weapon>().magicWeapon)
            pooledProjectile = MagicProjectilePool.current.Spawn();
        else
            pooledProjectile = ProjectilePool.current.Spawn();

        if (pooledProjectile == null)
        {
            Debug.Log("ERROR in ProjectileManager.Fire()");
        }
    }

    void InitPooledProjectile()
    {
        pooledProjectile.transform.position = projectileSpawnPoint.position;
        pooledProjectile.transform.rotation = projectileSpawnPoint.rotation;
        pooledProjectile.SetActive(true);

        // ensure projectile sprite is pointing the same direction as the shooter
        pooledProjectile.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // set weapon to proper collider layer——enemy or player
        pooledProjectile.layer = (player) ? WEAPON_COLLIDER : ENEMY_WEAPON;
    }
}