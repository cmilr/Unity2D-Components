using UnityEngine;
using System.Collections;

public class ProjectileManager : CacheBehaviour {

	private GameObject projectilePrefab;
    private Transform projectileSpawnPoint;
    private Projectile projectile;
    private float fireRate;
    private float nextFire;

    void Start()
    {
        projectileSpawnPoint = GetComponentInChildren<SpawnPointTrace>().transform;

        // instantiate correct projectile depending on who is firing: Player or Enemy
        // the only difference between projectiles is the layer they reside on
        if(gameObject.layer == PLAYER_LAYER)
        {
            projectilePrefab = (GameObject)Resources.Load("Prefabs/Projectiles/PlayerProjectile", typeof(GameObject));
        }
        else
        {
            projectilePrefab = (GameObject)Resources.Load("Prefabs/Projectiles/EnemyProjectile", typeof(GameObject));
        }
    }

    // fire in direction actor is facing
    public void Fire(Weapon equippedWeapon)
    {
        fireRate = equippedWeapon.rateOfAttack;

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as GameObject;
            projectile = go.GetComponent<Projectile>();

            // flip projectile sprite so it's pointing the same direction as the actor
            go.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            // projectile.Init(equippedWeapon);
            projectile.Fire(equippedWeapon, transform.localScale.x);
        }
    }

    // fire specifically at target
    public void FireAtTarget(Weapon equippedWeapon, GameObject target)
    {
        fireRate = equippedWeapon.rateOfAttack;

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as GameObject;
            projectile = go.GetComponent<Projectile>();

            // flip projectile sprite so it's pointing the same direction as the actor
            go.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            // projectile.Init(equippedWeapon);
            projectile.Fire(equippedWeapon, target);
        }
    }
}
