using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class DisplayHearts : BaseBehaviour
{
	public bool pulse;
	private CanvasScaler canvasScaler;
	private RectTransform rectTrans;
	private SpriteRenderer spriteRenderer;
	private Sequence fadeIn;
	private Sequence fadeOutInstant;
	private Sequence fadeOutHUD;
	private Sequence pulsingHeart;

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
		
		switch (name)
		{
			case "Heart_1":
				M.PositionInHUD(rectTrans, spriteRenderer, HEART_ALIGNMENT, HEART_X_POS, HEART_Y_POS);
				break;
			case "Heart_2":
				M.PositionInHUD(rectTrans, spriteRenderer, HEART_ALIGNMENT, HEART_X_POS + HEART_OFFSET, HEART_Y_POS);
				break;
			case "Heart_3":
				M.PositionInHUD(rectTrans, spriteRenderer, HEART_ALIGNMENT, HEART_X_POS + (HEART_OFFSET * 2), HEART_Y_POS);
				break;
			default:
				Assert.IsTrue(false, "Default case reached @ " + gameObject);
				break;
		}
		
		// cache & pause tween sequences.
		(fadeOutInstant = MFX.Fade(spriteRenderer, 0, 0, 0)).Pause();
		(fadeIn         = MFX.Fade(spriteRenderer, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(fadeOutHUD     = MFX.Fade(spriteRenderer, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(pulsingHeart   = MFX.Pulse(gameObject)).Pause();

		if (pulse)
		{
			pulsingHeart.Restart();
			pulsingHeart.timeScale = 2f;
		}
		
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
