using UnityEngine;
using System.Collections;

public class ProjectileManager : CacheBehaviour {

	public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float fireRate;
    private float nextFire;

    public void FireProjectile()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject go = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as GameObject;
            // flip projectile sprite so it's pointing the same direction as the actor
            go.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            go.GetComponent<Mover>().StartProjectile(transform.localScale.x);
            // audio.Play ();
        }
    }
}
