using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class Weapon : BaseBehaviour
{
	public enum Type { Invalid, Axe, Sword, Hammer, Dagger, MagicProjectile, Touch };
	public Type type;

	public enum Style { Melee, Ranged }
	[HideInInspector]
	public Style style;
	[HideInInspector]
	public bool alreadyCollided;
	[HideInInspector]
	public Sprite iconSprite;
	[HideInInspector]
	public Sprite projectileSprite;
	public Sprite actualSprite;

	public int worth;

	[Tooltip("Is this weapon in someone's inventory, or is it just loose on the floor?")]
	public bool inPlayerInventory;
	public bool inEnemyInventory;


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
	[Tooltip("If animated — Animator Controller .")]
	public RuntimeAnimatorController animController;

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

	[HideInInspector]
	public SpriteRenderer spriteRenderer;

	// tween sequences and their related variables.
	private float whenEquippedFadeAfter = 0f;
	private float whenEquippedFadeOver = .2f;
	private float whenStashedFadeAfter = 0f;
	private float whenStashedFadeOver = .2f;
	private Sequence fadeInOnEquip;
	private Sequence fadeOutOnDiscard;
	private Sequence fadeBackInAfterDiscard;
	private Sequence fadeWhenStashed;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);

		Assert.IsFalse(type == Type.Invalid,
			("Invalid weapon type @ " + gameObject));

		iconSprite = spriteRenderer.sprite;
		projectileSprite = spriteRenderer.sprite;
	}

	void Start()
	{
		// cache & pause tweens.
		(fadeInOnEquip 			= MFX.Fade(spriteRenderer, 1f, whenEquippedFadeAfter, whenEquippedFadeOver)).Pause();
		(fadeOutOnDiscard 		= MFX.Fade(spriteRenderer, .2f, 0f, 0f)).Pause();
		(fadeBackInAfterDiscard = MFX.FadeInWeapon(spriteRenderer, 1f, 2f, 1f)).Pause();
		(fadeWhenStashed 		= MFX.Fade(spriteRenderer, 0f, whenStashedFadeAfter, whenStashedFadeOver)).Pause();

		EstablishAttackStyle();
		Init();
	}

	void Init()
	{
		if (inPlayerInventory)
		{
			GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponentInChildren<Rigidbody2D>().isKinematic = true;
			gameObject.GetComponentInChildren<PhysicsCollider>().DisablePhysicsCollider();
			gameObject.GetComponentInChildren<WeaponPickupCollider>().DisableWeaponPickupCollider();
		}
		else if (inEnemyInventory)
		{
			GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponentInChildren<Rigidbody2D>().isKinematic = true;
			gameObject.GetComponentInChildren<PhysicsCollider>().DisablePhysicsCollider();
			gameObject.GetComponentInChildren<WeaponPickupCollider>().DisableWeaponPickupCollider();
		}
		else // weapon is loose on the floor.
		{ 
			GetComponent<SpriteRenderer>().enabled = true;
		}
	}

	void EstablishAttackStyle()
	{
		switch (type)
		{
			case Type.Axe:
				style = Style.Melee;
				break;
			case Type.Sword:
				style = Style.Melee;
				break;
			case Type.Hammer:
				style = Style.Ranged;
				break;
			case Type.Dagger:
				style = Style.Ranged;
				break;
			case Type.MagicProjectile:
				style = Style.Ranged;
				break;
			case Type.Touch:
				style = Style.Melee;
				break;
			default:
				Assert.IsTrue(false, ("Error @ " + gameObject));
				break;
		}
	}

	public void OnEquip()
	{
		fadeInOnEquip.Restart();
	}

	public void OnDiscard()
	{
		fadeOutOnDiscard.Restart();
		fadeBackInAfterDiscard.Restart();
	}

	public void OnStashed()
	{
		fadeWhenStashed.Restart();
	}
}
