using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DisplayWeaponTitle : BaseBehaviour
{
	private Text textComponent;
	private CanvasScaler canvasScaler;
	private RectTransform rectTrans;
	private Sequence fadeIn;
	private Sequence fadeOutInstant;
	private Sequence fadeOutHUD;
	private float canvasScale;

	void Awake()
	{
		rectTrans = GetComponent<RectTransform>();
		Assert.IsNotNull(rectTrans);

		textComponent = GetComponent<Text>();
		Assert.IsNotNull(textComponent);
	}

	void Start()
	{
		canvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
		Assert.IsNotNull(canvasScaler);

		canvasScale = Camera.main.GetComponent<PixelArtCamera>().CanvasScale;
		Assert.AreNotApproximatelyEqual(0.0f, canvasScale);

		PositionHUDElements();

		// cache & pause tween sequences.
		(fadeOutInstant = MFX.Fade(textComponent, 0, 0, 0)).Pause();
		(fadeIn         = MFX.Fade(textComponent, 1, 0, ITEM_CHANGE_FADE)).Pause();
		(fadeOutHUD     = MFX.Fade(textComponent, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
	}

	void OnInitEquippedWeapon(GameObject weapon)
	{
		textComponent.text = weapon.GetComponent<Weapon>().title;
		FadeInInitialTitle();
	}

	void PositionHUDElements()
	{
		canvasScaler.scaleFactor = canvasScale;
		M.PositionInHUD(rectTrans, textComponent, ITEM_TITLE_ALIGNMENT, ITEM_TITLE_X_POS, ITEM_TITLE_Y_POS);
	}

	void OnInitNewEquippedWeapon(GameObject weapon)
	{
		textComponent.text = weapon.GetComponent<Weapon>().title;
	}

	void FadeInInitialTitle()
	{
		fadeOutInstant.Restart();
		fadeIn.Restart();
	}

	void OnChangeEquippedWeapon(GameObject newWeapon)
	{
		textComponent.text = newWeapon.GetComponent<Weapon>().title;
		FadeInNewTitle();
	}

	void FadeInNewTitle()
	{
		fadeOutInstant.Restart();
		fadeIn.Restart();
	}

	void OnFadeHud(bool status)
	{
		fadeOutHUD.Restart();
	}

	void OnEnable()
	{
		EventKit.Subscribe<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		EventKit.Subscribe<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		EventKit.Subscribe<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
		EventKit.Subscribe("reposition hud elements", PositionHUDElements);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		EventKit.Unsubscribe<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		EventKit.Unsubscribe<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
		EventKit.Unsubscribe("reposition hud elements", PositionHUDElements);
	}
}
