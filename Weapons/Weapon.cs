using Matcha.Unity;
using UnityEngine;

public class Weapon : AnimationBehaviour
{
	public enum WeaponType { Axe, Sword, Hammer, Dagger, MagicProjectile };
	public WeaponType weaponType;

	[HideInInspector]
	public bool alreadyCollided;

	[Tooltip("Is this weapon in someone's inventory, or is it just loose on the floor?")]
	public bool inInventory;


	[Header("SPRITES")]
	//~~~~~~~~~~~~~~~~~~~~~
	[Tooltip("This is the pickup/HUD icon.")]
	public Sprite iconSprite;

	[Tooltip("Projectile sprite that will actually be fired.")]
	public Sprite projectileSprite;

	[Tooltip("Color of the upper element in the animated version of the weapon.")]
	public string upperColor = "ffffff";

	[Tooltip("Color of the center element in the animated version of the weapon.")]
	public string centerColor = "ffffff";

	[Tooltip("Color of the lower element in the animated version of the weapon.")]
	public string lowerColor = "ffffff";


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


	// genericized weapon pieces
	private WeaponPiece upper;
	private WeaponPiece center;
	private WeaponPiece lower;

	private SpriteRenderer spritePickupIcon;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		//populate SpriteRender component from iconSprite in this script
		spritePickupIcon = gameObject.GetComponent<SpriteRenderer>();
		spritePickupIcon.sprite = iconSprite;

		//if weapon is loose on the floor, turn on its pickup icon so it can bee seen
		if (!inInventory)
		{
			spritePickupIcon.enabled = true;
		}
		else
		{
			spritePickupIcon.enabled = false;
		}

		if (transform.parent != null)
		{
			// if weapon is being carried by the player, animate it while player is walking, jumping, etc
			if (weaponType != WeaponType.MagicProjectile)
			{
				// set weapon components on initialization
				upper  = transform.FindChild("Upper").gameObject.GetComponent<WeaponPiece>();
				center = transform.FindChild("Center").gameObject.GetComponent<WeaponPiece>();
				lower  = transform.FindChild("Lower").gameObject.GetComponent<WeaponPiece>();

				// set weapon colors here
				upper.spriteRenderer.material.SetColor("_Color", M.HexToColor(upperColor));
				center.spriteRenderer.material.SetColor("_Color", M.HexToColor(centerColor));
				lower.spriteRenderer.material.SetColor("_Color", M.HexToColor(lowerColor));
			}
		}
	}

	// animation state methods
	public void PlayIdleAnimation(float xOffset, float yOffset)
	{
		upper.PlayIdleAnimation();
		center.PlayIdleAnimation();
		lower.PlayIdleAnimation();
		OffsetAnimation(xOffset, yOffset);
	}

	public void PlayRunAnimation(float xOffset, float yOffset)
	{
		upper.PlayRunAnimation();
		center.PlayRunAnimation();
		lower.PlayRunAnimation();
		OffsetAnimation(xOffset, yOffset);
	}

	public void PlayJumpAnimation(float xOffset, float yOffset)
	{
		upper.PlayJumpAnimation();
		center.PlayJumpAnimation();
		lower.PlayJumpAnimation();
		OffsetAnimation(xOffset, yOffset);
	}

	public void PlayAttackAnimation(float xOffset, float yOffset)
	{
		upper.PlayAttackAnimation();
		center.PlayAttackAnimation();
		lower.PlayAttackAnimation();
		OffsetAnimation(xOffset, yOffset);
	}

	public void EnableAnimation(bool status)
	{
		upper.spriteRenderer.enabled = status;
		center.spriteRenderer.enabled = status;
		lower.spriteRenderer.enabled = status;
	}
}
