using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class DisplayEquipped : BaseBehaviour
{
	private Vector3 slideTweenOrigin;
	private new Transform transform;
	private Camera mainCamera;
	private SpriteRenderer spriteRenderer;
	private Sequence fadeInHUD;
	private Sequence fadeOutHUD;
	private Sequence fadeOutInstant;
	private Tween slideItem;
	private Tween fadeToNoTint;
	private Tween setToStashedTint;

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
		(fadeToNoTint       = MFX.FadeToNoTint(spriteRenderer, 0, .5f)).Pause();
		(setToStashedTint 	= MFX.SetToStashedTint(spriteRenderer)).Pause();
		(fadeInHUD          = MFX.Fade(spriteRenderer, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(fadeOutHUD         = MFX.Fade(spriteRenderer, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(fadeOutInstant     = MFX.Fade(spriteRenderer, 0, 0, 0)).Pause();

		Invoke("PositionHUDElements", .001f);
	}

	void PositionHUDElements()
	{	
		// position in top-center of HUD.
		transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
			Screen.width / 2,
			Screen.height - (INVENTORY_Y_POS * RESOLUTION_OFFSET),
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

	void OnInitWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
		fadeOutInstant.Restart();
		fadeInHUD.Restart();
	}
	
	void OnInitNewWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
	}

	void OnChangeWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;

		// upon receiving new weapon, instantly relocate it back to its previous location.
		transform.localPosition = new Vector3(
			slideTweenOrigin.x,
			slideTweenOrigin.y,
			slideTweenOrigin.z
		);

		// set tint to that of stashed weapons. 
		setToStashedTint.Restart();

		// then tween to NO tint.
		fadeToNoTint.Restart();

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

	void OnEnable()
	{
		EventKit.Subscribe<GameObject>("init equipped weapon", OnInitWeapon);
		EventKit.Subscribe<GameObject>("init new equipped weapon", OnInitNewWeapon);
		EventKit.Subscribe<GameObject>("change equipped weapon", OnChangeWeapon);
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
		EventKit.Subscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<GameObject>("init equipped weapon", OnInitWeapon);
		EventKit.Unsubscribe<GameObject>("init new equipped weapon", OnInitNewWeapon);
		EventKit.Unsubscribe<GameObject>("change equipped weapon", OnChangeWeapon);
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
		EventKit.Unsubscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}
}
