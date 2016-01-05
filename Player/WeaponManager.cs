using System.Collections;
using UnityEngine.Assertions;
using UnityEngine;

public class WeaponManager : CacheBehaviour
{
	private ProjectileManager projectile;
	private MeleeManager melee;
	private Weapon equippedWeapon;

	void Start()
	{
		melee      = transform.parent.GetComponent<MeleeManager>();
		projectile = transform.parent.GetComponent<ProjectileManager>();
	}

	public void Attack()
	{
		switch (equippedWeapon.weaponType)
		{
			// swinging weapons
			case Weapon.WeaponType.Axe:
			case Weapon.WeaponType.Sword:
			{
				melee.Attack(equippedWeapon);
				break;
			}

			// hurled weapons
			case Weapon.WeaponType.Hammer:
			case Weapon.WeaponType.Dagger:
			{
				projectile.Fire(equippedWeapon);
				break;
			}

			// magic projectile weapons
			case Weapon.WeaponType.MagicProjectile:
			{
				projectile.Fire(equippedWeapon);
				break;
			}

			default:
			{
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
			}
		}
	}

	void OnNewEquippedWeapon(Weapon newEquippedWeapon)
	{
		equippedWeapon = newEquippedWeapon;
	}

	void OnEnable()
	{
		Evnt.Subscribe<Weapon>("new equipped weapon", OnNewEquippedWeapon);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<Weapon>("new equipped weapon", OnNewEquippedWeapon);
	}
}
