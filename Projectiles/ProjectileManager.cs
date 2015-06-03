using UnityEngine;
using System.Collections;

public class ProjectileManager : CacheBehaviour {

	public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    private float fireRate;
    private float nextFire;

    public void FireProjectile(Weapon equippedWeapon)
    {
        fireRate = equippedWeapon.rateOfAttack;

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as GameObject;

            // flip projectile sprite so it's pointing the same direction as the actor
            go.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            go.GetComponent<Projectile>().InitAndFire(equippedWeapon.sprite, transform.localScale.x, equippedWeapon.projectileSpeed);

            // audio.Play ();
        }
    }
}
