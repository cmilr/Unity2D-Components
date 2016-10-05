using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DisplayWaterDrop : BaseBehaviour
{
	private CanvasScaler canvasScaler;
	private RectTransform rectTrans;
	private SpriteRenderer spriteRenderer;
	private Sequence fadeIn;
	private Sequence fadeOutInstant;
	private Sequence fadeOutHUD;

	void Awake()
	{
		rectTrans = GetComponent<RectTransform>();
		Assert.IsNotNull(rectTrans);

		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
	}
	
	void Start()
	{
		canvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
		Assert.IsNotNull(canvasScaler);

		canvasScaler.scaleFactor = PLATFORM_SPECIFIC_CANVAS_SCALE;
		M.PositionInHUD(rectTrans, spriteRenderer, WATER_DROP_ALIGNMENT, WATER_DROP_X_POS, WATER_DROP_Y_POS);
		
		// cache & pause tween sequences.
		(fadeOutInstant = MFX.Fade(spriteRenderer, 0, 0, 0)).Pause();
		(fadeIn         = MFX.Fade(spriteRenderer, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(fadeOutHUD     = MFX.Fade(spriteRenderer, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
	}

	void FadeInText()
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
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
