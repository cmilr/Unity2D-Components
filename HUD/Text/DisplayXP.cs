using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class DisplayXP : BaseBehaviour
{
	private int intToDisplay;
	private string legend = "XP: ";
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
		(fadeIn         = MFX.Fade(textComponent, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
		(fadeOutHUD     = MFX.Fade(textComponent, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
	}

	void OnInitInteger(int initInt)
	{
		textComponent.text = legend + initInt;
		FadeInText();
	}

	void PositionHUDElements()
	{
		canvasScaler.scaleFactor = canvasScale;
		M.PositionInHUD(rectTrans, textComponent, XP_ALIGNMENT, XP_X_POS, XP_Y_POS);
	}

	void FadeInText()
	{
		fadeOutInstant.Restart();
		fadeIn.Restart();
	}

	void OnChangeInteger(int newInt)
	{
		textComponent.text = legend + newInt;
	}

	void OnFadeHud(bool status)
	{
		fadeOutHUD.Restart();
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("init xp", OnInitInteger);
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("init xp", OnInitInteger);
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
