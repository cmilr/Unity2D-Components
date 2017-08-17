using UnityEngine;
using UnityEngine.Assertions;

public class WeaponManager : BaseBehaviour
{
	private ProjectileManager projectile;
	private MeleeManager melee;
	private Weapon equippedWeapon;
	private SpriteRenderer spriteRenderer;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
	}

	void Start()
	{
		melee = transform.parent.GetComponent<MeleeManager>();
		Assert.IsNotNull(melee);
		
		projectile = transform.parent.GetComponent<ProjectileManager>();
		Assert.IsNotNull(projectile);
	}

	public void Attack()
	{
		switch (equippedWeapon.type)
		{
			// swinging weapons
			case Weapon.Type.Axe:
			case Weapon.Type.Sword:
				melee.Attack(equippedWeapon);
				break;
			// hurled weapons
			case Weapon.Type.Hammer:
			case Weapon.Type.Dagger:
				projectile.Fire(equippedWeapon);
				break;
			// magic projectile weapons
			case Weapon.Type.MagicProjectile:
				projectile.Fire(equippedWeapon);
				break;
			default:
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
		}
	}

	// swap out animation sprite references in LateUpdate.
	void LateUpdate()
	{
		Assert.IsNotNull(equippedWeapon.actualSprite, "Weapon is missing a sprite_actual.");
		spriteRenderer.sprite = equippedWeapon.actualSprite;
	}

	void OnNewEquippedWeapon(Weapon newEquippedWeapon)
	{
		equippedWeapon = newEquippedWeapon;
	}

	void OnEnable()
	{
		EventKit.Subscribe<Weapon>("new equipped weapon", OnNewEquippedWeapon);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<Weapon>("new equipped weapon", OnNewEquippedWeapon);
	}
}
