using Matcha.Unity;
using UnityEngine;

public class Weapon : CacheBehaviour
{
	public enum WeaponType { Axe, Sword, Hammer, Dagger, MagicProjectile };
	public WeaponType weaponType;
	public int worth;

	[HideInInspector]
	public bool alreadyCollided;

	[Tooltip("Is this weapon in someone's inventory, or is it just loose on the floor?")]
	public bool inPlayerInventory;
	public bool inEnemyInventory;


	[Header("SPRITES")]
	//~~~~~~~~~~~~~~~~~~~~~
	[Tooltip("This is the pickup/HUD icon.")]			//weapon sprite with black outline
	public Sprite iconSprite;

	[HideInInspector]
	[Tooltip("Actual sprite our hero carries.")]    //weapon sprite without outline
	public Sprite carriedSprite;							//loaded automatically from Resources/


	[Header("ALL WEAPONS")]
	//~~~~~~~~~~~~~~~~~~~~~
	[Tooltip("What title should be displayed when this weapon is equipped?")]
	public string title;

	[Tooltip("How much damage does this weapon do?")]
	public int damage;

	[Tooltip("How many hits can this weapon take before it's unuseable?")]
	public int hp;

	[Tooltip("How many times per second can this weapon be fired?")]
	public float rateOfAttack;


	[Header("RANGED WEAPONS ONLY")]
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	[Range(8, 20)]
	[Tooltip("How fast should projectile travel?")]
	public float speed = 12f;

	[Tooltip("How far should projectile travel before fading out?")]
	public float maxDistance = 40f;

	[Tooltip("Should projectile be lobbed?")]
	public bool lob;

	[Tooltip("ONLY EFFECTS PLAYER: If 'Lob' is true, how much should gravity effect projectile?")]
	public float lobGravity;

	[Tooltip("Should projectile fade-in when thrown?")]
	public bool fadeIn;

	[Tooltip("If Animated, ProjectileContainer will attempt to load an animation.")]
	public bool animatedProjectile;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		//all weapons have two sprites: one that's outlined in black (iconSprite), one that's not (carriedSprite.)
		//this routine automatically loads the  correct sprite (carriedSprite) for the hero's carried weapon.
		//if instead it's an enemy's magical projectile, we just use the main SpriteRenderer's sprite.
		if (weaponType == WeaponType.MagicProjectile)
		{
			carriedSprite = iconSprite;
		}
		else
		{
			carriedSprite = (Sprite)(Resources.Load(("Sprites/Pickups/NoOutlines/weapon/" + iconSprite.name), typeof(Sprite)));
		}


		//if weapon is loose on the floor, turn its pickup icon on so it can be seen
		if (inPlayerInventory)
		{
			spriteRenderer.sprite = carriedSprite;
			spriteRenderer.enabled = false;
			gameObject.GetComponentInChildren<Rigidbody2D>().isKinematic = true;
			gameObject.GetComponentInChildren<PhysicsCollider>().DisablePhysicsCollider();
			gameObject.GetComponentInChildren<MeleeCollider>().EnableMeleeCollider();
			gameObject.GetComponentInChildren<WeaponPickupCollider>().DisableWeaponPickupCollider();
		}
		else if (inEnemyInventory)
		{
			spriteRenderer.enabled = false;
			gameObject.GetComponentInChildren<Rigidbody2D>().isKinematic = true;
			gameObject.GetComponentInChildren<PhysicsCollider>().DisablePhysicsCollider();
			gameObject.GetComponentInChildren<WeaponPickupCollider>().DisableWeaponPickupCollider();
		}
		else if (name != "Projectile(Clone)") //else not a pooled weapon (ie: this weapon is equipped)
		{
			spriteRenderer.sprite = carriedSprite;
			spriteRenderer.enabled = true;
			gameObject.GetComponentInChildren<Rigidbody2D>().isKinematic = false;
			gameObject.GetComponentInChildren<PhysicsCollider>().EnablePhysicsCollider();
			gameObject.GetComponentInChildren<MeleeCollider>().DisableMeleeCollider();
			gameObject.GetComponentInChildren<WeaponPickupCollider>().EnableWeaponPickupCollider();
		}
		else //weapon is on the ground, so needs icon sprite (ie: the outlined one)
		{
			spriteRenderer.sprite = iconSprite;
		}
	}
}
