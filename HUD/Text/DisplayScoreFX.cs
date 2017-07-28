using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DisplayScoreFX : HudTextBehaviour
{
	private Text textComponent;
	private Sequence fadeIn;
	private Sequence fadeOutInstant;
	private Sequence fadeOutHUD;
	private Sequence displayScoreFX;

	void Awake()
	{
		textComponent = GetComponent<Text>();
		Assert.IsNotNull(textComponent);

		legend = "SC: ";
		anchorPosition = SCORE_ALIGNMENT;
		targetXPos = SCORE_X_POS;
		targetYPos = SCORE_Y_POS;

		BaseAwake();
	}

	void Start()
	{
		BaseStart();

		// cache & pause tween sequences.
		(fadeOutInstant = MFX.Fade(textComponent, 0, 0, 0)).Pause();
		(fadeIn         = MFX.Fade(textComponent, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
		(fadeOutHUD     = MFX.Fade(textComponent, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
		(displayScoreFX = MFX.DisplayScoreFX(gameObject)).Pause();
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
		displayScoreFX.Restart();
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
