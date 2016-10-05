using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DisplayScore : BaseBehaviour
{
	private Text textComponent;
	private CanvasScaler canvasScaler;
	private RectTransform rectTrans;
	private Sequence fadeIn;
	private Sequence fadeOutInstant;
	private Sequence fadeOutHUD;
	private Sequence displayScore;

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

		canvasScaler.scaleFactor = PLATFORM_SPECIFIC_CANVAS_SCALE;
		M.PositionInHUD(rectTrans, textComponent, SCORE_ALIGNMENT, SCORE_X_POS, SCORE_Y_POS);

		// cache & pause tween sequences.
		(fadeOutInstant = MFX.Fade(textComponent, 0, 0, 0)).Pause();
		(fadeIn         = MFX.Fade(textComponent, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(fadeOutHUD     = MFX.Fade(textComponent, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(displayScore   = MFX.DisplayScore(gameObject, textComponent)).Pause();
	}

	void OnInitScore(int initScore)
	{
		textComponent.text = initScore.ToString();
		FadeInScore();
	}

	void FadeInScore()
	{
		fadeOutInstant.Restart();
		fadeIn.Restart();
	}

	void OnChangeScore(int newScore)
	{
		textComponent.text = newScore.ToString();
		displayScore.Restart();
	}

	void OnFadeHud(bool status)
	{
		fadeOutHUD.Restart();
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("init score", OnInitScore);
		EventKit.Subscribe<int>("change score", OnChangeScore);
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("init score", OnInitScore);
		EventKit.Unsubscribe<int>("change score", OnChangeScore);
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
