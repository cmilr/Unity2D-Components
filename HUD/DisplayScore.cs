using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayScore : BaseBehaviour
{
	public bool topLayer;
	private Text HUDScore;
	private int currentScore;
	private float timeToFade = 2f;

	void Start ()
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
		// fade score to zero instantly, then fade up slowly
		MTween.FadeOut(HUDScore, 0, 0);
		MTween.FadeIn(HUDScore, HUD_FADE_IN_AFTER, timeToFade);
	}
	void OnChangeScore(int newScore)
	{
		HUDScore.text = newScore.ToString();

		if (topLayer)
		{
			MTween.DisplayScore(gameObject, HUDScore);
		}
		else
		{
			MTween.DisplayScoreFX(gameObject, HUDScore);
		}
	}

	void OnFadeHud(bool status)
	{
		MTween.FadeOut(HUDScore, HUD_FADE_OUT_AFTER, timeToFade);
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