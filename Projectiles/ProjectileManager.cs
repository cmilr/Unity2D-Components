using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileManager : BaseBehaviour
{
	private GameObject pooledProjectile;
	private Transform projectileSpawnPoint;
	private bool firedByPlayer;
	private float fireRate;
	private float nextFire;
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}
	
	void Start()
	{
		projectileSpawnPoint = GetComponentInChildren<SpawnPointTrace>().transform;
		Assert.IsNotNull(projectileSpawnPoint);

		// check if this is the player or an enemy
		firedByPlayer = (gameObject.layer == PLAYER_DEFAULT_LAYER);
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
			pooledProjectile.GetComponent<ProjectileContainer>().Fire(firedByPlayer, equippedWeapon, transform.localScale.x);
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
			pooledProjectile.GetComponent<ProjectileContainer>().Fire(firedByPlayer, equippedWeapon, target);
		}
	}

	void GetPooledProjectile(Weapon equippedWeapon)
	{
		pooledProjectile = ProjectilePool.current.Spawn();
		Assert.IsNotNull(pooledProjectile);
	}

	void InitPooledProjectile()
	{
		pooledProjectile.transform.position = projectileSpawnPoint.position;
		pooledProjectile.transform.rotation = projectileSpawnPoint.rotation;
		pooledProjectile.SetActive(true);

		// ensure projectile sprite is pointing the same direction as the shooter
		pooledProjectile.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

		// set weapon to proper collider layer——enemy or player
		pooledProjectile.layer = firedByPlayer ? PLAYER_WEAPON_COLLIDER : ENEMY_WEAPON_COLLIDER;
	}
}
