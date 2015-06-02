using UnityEngine;
using System.Collections;

public class ProjectileManager : CacheBehaviour {

	public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float fireRate;
    private float nextFire;

    public void FireProjectile(bool facingRight)
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject go = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as GameObject;
            go.GetComponent<Mover>().StartProjectile(facingRight);
            // audio.Play ();
        }
    }
}
