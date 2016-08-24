using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;

public class DisplayEquipped : CacheBehaviour
{
	private SpriteRenderer HUDWeapon;
	private Camera mainCamera;
	private Vector3 hudPosition;

	void Start()
	{
		mainCamera = Camera.main.GetComponent<Camera>();
		PositionHUDElements();
	}

	void PositionHUDElements()
	{
		transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
							Screen.width / 2,
							Screen.height - HUD_WEAPON_TOP_MARGIN,
							HUD_Z));

		hudPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
	}

	void OnInitEquippedWeapon(GameObject weapon)
	{
		HUDWeapon = spriteRenderer;
		HUDWeapon.sprite = weapon.GetComponent<Weapon>().iconSprite;
		HUDWeapon.DOKill();
		FadeInWeapon();
	}

	void OnInitNewEquippedWeapon(GameObject weapon)
	{
		HUDWeapon = spriteRenderer;
		HUDWeapon.sprite = weapon.GetComponent<Weapon>().iconSprite;
		HUDWeapon.DOKill();
	}

	void OnChangeEquippedWeapon(GameObject weapon)
	{
		HUDWeapon = spriteRenderer;
		HUDWeapon.sprite = weapon.GetComponent<Weapon>().iconSprite;
		transform.localPosition = hudPosition;

		// upon receiving new weapon for the equipped slot, instantly relocate it back to its previous location,
		// while setting opacity to that of a stashed weapon, then tween the image to the right, into the final
		// equipped slot position, all while tweening the opacity back to 100%
		MFX.Fade(HUDWeapon, HUD_STASHED_TRANSPARENCY, 0, 0);
		MFX.Fade(HUDWeapon, 1f, 0f, .2f);

		// shift weapon left, to roughly the position it was just in
		transform.localPosition = new Vector3(
			transform.localPosition.x - SPACE_BETWEEN_WEAPONS,
			transform.localPosition.y,
			transform.localPosition.z)
		;

		// tween weapon to new position
		transform.DOLocalMove(new Vector3(
					transform.localPosition.x + SPACE_BETWEEN_WEAPONS,
					transform.localPosition.y,
					transform.localPosition.z), INVENTORY_SHIFT_SPEED, false).OnComplete(() => SetFinalPosition()
				);
	}

	void SetFinalPosition()
	{
		// set weapon into final position; fixes graphical bugs if operation gets interrupted by a new click
		// transform.localPosition = hudPosition;
	}

	void FadeInWeapon()
	{
		// fade weapon to zero instantly, then fade up slowly
		MFX.Fade(HUDWeapon, 0, 0, 0);
		MFX.Fade(HUDWeapon, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnFadeHud(bool status)
	{
		MFX.Fade(HUDWeapon, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnScreenSizeChanged(float vExtent, float hExtent)
	{
		PositionHUDElements();
	}

	void OnEnable()
	{
		EventKit.Subscribe<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		EventKit.Subscribe<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		EventKit.Subscribe<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
		EventKit.Subscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		EventKit.Unsubscribe<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		EventKit.Unsubscribe<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
		EventKit.Unsubscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}
}
