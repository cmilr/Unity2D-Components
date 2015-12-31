using DG.Tweening;
using Matcha.Dreadful;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DisplayScoreFX : BaseBehaviour
{
	private Text HUDScore;
	private static int previousScore;

	void Start()
	{
		HUDScore = gameObject.GetComponent<Text>();
		HUDScore.DOKill();
	}

	void OnInitScore(int initScore)
	{
		HUDScore.text = initScore.ToString();
		FadeInScore();
	}

	void FadeInScore()
	{
		// fade to zero instantly, then fade up slowly
		MFX.Fade(HUDScore, 0, 0, 0);
		MFX.Fade(HUDScore, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnChangeScore(int newScore)
	{
		HUDScore.text = newScore.ToString();

		int scoreChange = (newScore - previousScore);

		if (scoreChange > 5)
		{
			MFX.DisplayScoreFX(gameObject, HUDScore);
		}

		previousScore = newScore;
	}

	void OnFadeHud(bool status)
	{
		MFX.Fade(HUDScore, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnEnable()
	{
		Evnt.Subscribe<int>("init score", OnInitScore);
		Evnt.Subscribe<int>("change score", OnChangeScore);
		Evnt.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<int>("init score", OnInitScore);
		Evnt.Unsubscribe<int>("change score", OnChangeScore);
		Evnt.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
