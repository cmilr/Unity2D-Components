using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Dreadful;


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
		Messenger.AddListener<int>("init score", OnInitScore);
		Messenger.AddListener<int>("change score", OnChangeScore);
		Messenger.AddListener<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>("init score", OnInitScore);
		Messenger.RemoveListener<int>("change score", OnChangeScore);
		Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
	}
}