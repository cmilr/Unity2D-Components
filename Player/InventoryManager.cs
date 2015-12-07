using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;

public class InventoryManager : CacheBehaviour
{
	private Weapon equippedWeapon;
	private Weapon leftWeapon;
	private Weapon rightWeapon;

	private int left = 0;
	private int equipped = 1;
	private int right = 2;
	private GameObject[] weaponBelt;
	private GameObject outgoingWeapon;
	private bool levelLoading;

	void OnInitWeapons(GameObject eWeapon, GameObject lWeapon, GameObject rWeapon)
	{
		// WEAPON GAMEOBJECTS — keep track of weapon GameObjects as they're equipped/stashed
		if (weaponBelt == null) {
			weaponBelt = new GameObject[3];
		}

		weaponBelt[left]     = lWeapon;
		weaponBelt[equipped] = eWeapon;
		weaponBelt[right]    = rWeapon;

		CacheAndSetupWeapons();
		PassInitialWeaponsToHUD();
		PassEquippedWeaponToWeaponManager();
	}

	void OnEquipNewWeapon(GameObject newWeapon)
	{
		CloseoutCurrentEquippedWeapon();
		InitNewEquippedWeapon(newWeapon);
	}

	void CloseoutCurrentEquippedWeapon()
	{
		outgoingWeapon = weaponBelt[equipped];
		outgoingWeapon.layer = PICKUPS_LAYER;
		outgoingWeapon.GetComponentInChildren<PhysicsCollider>().EnablePhysicsCollider();
		outgoingWeapon.GetComponentInChildren<MeleeCollider>().DisableMeleeCollider();
		outgoingWeapon.GetComponentInChildren<Rigidbody2D>().isKinematic = false;
		outgoingWeapon.GetComponent<SpriteRenderer>().enabled = true;
		outgoingWeapon.transform.SetLocalPositionXY(0f, .5f);
		outgoingWeapon.transform.SetAbsLocalScaleX(1f);
		outgoingWeapon.transform.parent = null;
		equippedWeapon.inPlayerInventory = false;
		equippedWeapon.inEnemyInventory = false;
		equippedWeapon.EnableAnimation(false);
		TossOutgoingWeapon();
		Invoke("EnablePickupCollider", 1f);
	}

	void InitNewEquippedWeapon(GameObject newWeapon)
	{
		weaponBelt[equipped] = newWeapon;
		weaponBelt[equipped].layer = PLAYER_LAYER;
		weaponBelt[equipped].transform.parent = gameObject.transform;
		weaponBelt[equipped].transform.localPosition = new Vector3(0f, 0f, .1f);
		weaponBelt[equipped].transform.SetLocalScaleXYZ(1f, 1f, 1f);
		weaponBelt[equipped].GetComponent<SpriteRenderer>().enabled = false;
		weaponBelt[equipped].GetComponentInChildren<PhysicsCollider>().DisablePhysicsCollider();
		weaponBelt[equipped].GetComponentInChildren<MeleeCollider>().EnableMeleeCollider();
		weaponBelt[equipped].GetComponentInChildren<WeaponPickupCollider>().DisableWeaponPickupCollider();
		weaponBelt[equipped].GetComponentInChildren<Rigidbody2D>().isKinematic = true;
		weaponBelt[equipped].GetComponent<Weapon>().inPlayerInventory = true;
		weaponBelt[equipped].GetComponent<Weapon>().inEnemyInventory = false;
		CacheAndSetupWeapons();
		PassEquippedWeaponToHUD();
		PassEquippedWeaponToWeaponManager();
	}

	void EnablePickupCollider()
	{
		//turn dropped weapon's pickup collider back on after a short delay
		outgoingWeapon.GetComponentInChildren<WeaponPickupCollider>().EnableWeaponPickupCollider();
	}

	void TossOutgoingWeapon()
	{
		if (transform.lossyScale.x > 0f) {
			outgoingWeapon.GetComponent<Rigidbody2D>().AddForce(new Vector3(-5, 5, 1), ForceMode2D.Impulse);
		}
		else {
			outgoingWeapon.GetComponent<Rigidbody2D>().AddForce(new Vector3(5, 5, 1), ForceMode2D.Impulse);
		}
	}

	void OnSwitchWeapon(int shiftDirection)
	{
		if (!levelLoading)
		{
			switch (equipped)
			{
				case 0:
				{
					left = 1;
					equipped = 2;
					right = 0;
					break;
				}

				case 1:
				{
					left = 2;
					equipped = 0;
					right = 1;
					break;
				}

				case 2:
				{
					left = 0;
					equipped = 1;
					right = 2;
					break;
				}
			}

			CacheAndSetupWeapons();
			PassNewWeaponsToHUD();
			PassEquippedWeaponToWeaponManager();
		}
	}

	void CacheAndSetupWeapons()
	{
		// WEAPON GAMEOBJECT'S 'WEAPON' COMPONENT
		// ~~~~~~~~~~~~~~~~~~~~~~~~
		// cache specific weapons (Sword, Hammer, etc) via parent class 'Weapon'
		// use to call currently equipped weapon animations
		leftWeapon       = weaponBelt[left].GetComponent<Weapon>();
		equippedWeapon   = weaponBelt[equipped].GetComponent<Weapon>();
		rightWeapon      = weaponBelt[right].GetComponent<Weapon>();

		// set weapons to player's WeaponCollider layer
		weaponBelt[left].layer = 9;
		weaponBelt[equipped].layer = 9;
		weaponBelt[right].layer = 9;

		// disable animations for weapons that are not equipped
		leftWeapon.EnableAnimation(false);
		equippedWeapon.EnableAnimation(true);
		rightWeapon.EnableAnimation(false);

		// disable colliders for all weapons - they are only enabled during attacks
		leftWeapon.GetComponent<BoxCollider2D>().enabled = false;
		equippedWeapon.GetComponent<BoxCollider2D>().enabled = false;
		rightWeapon.GetComponent<BoxCollider2D>().enabled = false;

		// fade in newly equipped weapon
		float fadeAfter = 0f;
		float fadeTime  = .2f;

		SpriteRenderer upperSprite  = equippedWeapon.transform.Find("Upper").GetComponent<SpriteRenderer>();
		SpriteRenderer centerSprite = equippedWeapon.transform.Find("Center").GetComponent<SpriteRenderer>();
		SpriteRenderer lowerSprite  = equippedWeapon.transform.Find("Lower").GetComponent<SpriteRenderer>();

		upperSprite.DOKill();
		centerSprite.DOKill();
		lowerSprite.DOKill();

		MFX.Fade(upperSprite, 1f, fadeAfter, fadeTime);
		MFX.Fade(centerSprite, 1f, fadeAfter, fadeTime);
		MFX.Fade(lowerSprite, 1f, fadeAfter, fadeTime);

		// fade out newly stashed weapons
		FadeOutStashedWeapons(leftWeapon);
		FadeOutStashedWeapons(rightWeapon);
	}

	void FadeOutStashedWeapons(Weapon stashedWeapon)
	{
		float fadeAfter = 0f;
		float fadeTime  = .2f;

		SpriteRenderer upperSprite  = stashedWeapon.transform.Find("Upper").GetComponent<SpriteRenderer>();
		SpriteRenderer centerSprite = stashedWeapon.transform.Find("Center").GetComponent<SpriteRenderer>();
		SpriteRenderer lowerSprite  = stashedWeapon.transform.Find("Lower").GetComponent<SpriteRenderer>();

		upperSprite.DOKill();
		centerSprite.DOKill();
		lowerSprite.DOKill();

		MFX.Fade(upperSprite, 0f, fadeAfter, fadeTime);
		MFX.Fade(centerSprite, 0f, fadeAfter, fadeTime);
		MFX.Fade(lowerSprite, 0f, fadeAfter, fadeTime);
	}

	void PassInitialWeaponsToHUD()
	{
		Messenger.Broadcast<GameObject, int>("init stashed weapon", weaponBelt[left], LEFT);
		Messenger.Broadcast<GameObject>("init equipped weapon", weaponBelt[equipped]);
		Messenger.Broadcast<GameObject, int>("init stashed weapon", weaponBelt[right], RIGHT);
	}

	void PassNewWeaponsToHUD()
	{
		Messenger.Broadcast<GameObject, int>("change stashed weapon", weaponBelt[left], LEFT);
		Messenger.Broadcast<GameObject>("change equipped weapon", weaponBelt[equipped]);
		Messenger.Broadcast<GameObject, int>("change stashed weapon", weaponBelt[right], RIGHT);
	}

	void PassEquippedWeaponToHUD()
	{
		Messenger.Broadcast<GameObject>("init new equipped weapon", weaponBelt[equipped]);
	}

	void PassEquippedWeaponToWeaponManager()
	{
		Messenger.Broadcast<Weapon>("new equipped weapon", equippedWeapon);
	}

	void OnLevelLoading(bool status)
	{
		// pause weapon changes while level loading
		levelLoading = true;

		StartCoroutine(Timer.Start(WEAPON_PAUSE_ON_LEVEL_LOAD, false, () => {
			levelLoading = false;
		}));
	}

	void OnEnable()
	{
		Messenger.AddListener<GameObject, GameObject, GameObject>("init weapons", OnInitWeapons);
		Messenger.AddListener<GameObject>("equip new weapon", OnEquipNewWeapon);
		Messenger.AddListener<int>("switch weapon", OnSwitchWeapon);
		Messenger.AddListener<bool>("level loading", OnLevelLoading);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<GameObject, GameObject, GameObject>("init weapons", OnInitWeapons);
		Messenger.RemoveListener<GameObject>("equip new weapon", OnEquipNewWeapon);
		Messenger.RemoveListener<int>("switch weapon", OnSwitchWeapon);
		Messenger.RemoveListener<bool>("level loading", OnLevelLoading);
	}
}
