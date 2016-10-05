using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class DisplayStashedRight : BaseBehaviour
{
	private Vector3 slideTweenOrigin;
	private new Transform transform;
	private Camera mainCamera;
	private SpriteRenderer spriteRenderer;
	private Sequence fadeInHUD;
	private Sequence fadeOutHUD;
	private Sequence fadeToTransparency;
	private Sequence fadeOutInstant;
	private Sequence fadeInInstant;
	private Tween slideItem;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);

		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
	}
	
	void Start()
	{
		mainCamera = Camera.main.GetComponent<Camera>();
		Assert.IsNotNull(mainCamera);

		// cache & pause tween sequences.
		(fadeInHUD          = MFX.Fade(spriteRenderer, STASHED_ITEM_TRANSPARENCY, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(fadeOutHUD         = MFX.Fade(spriteRenderer, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(fadeToTransparency = MFX.Fade(spriteRenderer, STASHED_ITEM_TRANSPARENCY, 0f, .2f)).Pause();
		(fadeOutInstant     = MFX.Fade(spriteRenderer, 0, 0, 0)).Pause();
		(fadeInInstant      = MFX.Fade(spriteRenderer, 1, 0, 0)).Pause();
		
		Invoke("PositionHUDElements", .001f);
	}

	void PositionHUDElements()
	{
		// shift item to the right
		transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
			Screen.width / 2 + STASHED_ITEM_OFFSET,
			Screen.height - INVENTORY_Y_POS,
			HUD_Z)
		);

		// set origin for item's slide tween
		slideTweenOrigin = new Vector3(
			transform.localPosition.x - DISTANCE_TO_SLIDE_ITEMS,
			transform.localPosition.y,
			transform.localPosition.z
		);

		// cache & pause slide tween.
		(slideItem = MFX.SlideStashedItem(transform, slideTweenOrigin, DISTANCE_TO_SLIDE_ITEMS, INVENTORY_SHIFT_SPEED, false)).Pause();
	}

	void InitStashedWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
		fadeOutInstant.Restart();
		fadeInHUD.Restart();
	}

	void ChangeStashedWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
		Assert.IsNotNull(spriteRenderer.sprite);

		// upon receiving new weapon, instantly relocate it back to its previous location.
		transform.localPosition = new Vector3(
			slideTweenOrigin.x,
			slideTweenOrigin.y,
			slideTweenOrigin.z
		);

		// set opacity to 100%. 
		fadeInInstant.Restart();
		
		// then tween the transparency down to that of stashed weapons.
		fadeToTransparency.Restart();

		// finally, tween the image to the right, into its final slot position.
		slideItem.Restart();
	}

	void OnFadeHud(bool status)
	{
		fadeOutHUD.Restart();
	}

	void OnScreenSizeChanged(float vExtent, float hExtent)
	{
		PositionHUDElements();
	}

	void OnInitStashedWeapon(GameObject weapon, int weaponSide)
	{
		if (weaponSide == RIGHT)
		{
			InitStashedWeapon(weapon);
		}
	}

	void OnChangeStashedWeapon(GameObject weapon, int weaponSide)
	{
		if (weaponSide == RIGHT)
		{
			ChangeStashedWeapon(weapon);
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<GameObject, int>("init stashed weapon", OnInitStashedWeapon);
		EventKit.Subscribe<GameObject, int>("change stashed weapon", OnChangeStashedWeapon);
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
		EventKit.Subscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<GameObject, int>("init stashed weapon", OnInitStashedWeapon);
		EventKit.Unsubscribe<GameObject, int>("change stashed weapon", OnChangeStashedWeapon);
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
		EventKit.Unsubscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}
}
