using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayLives : BaseBehaviour
{
	public Sprite threeLives;
	public Sprite twoLives;
	public Sprite oneLife;
	private Image HUDLives;
	private float timeToFade = 2f;

	void Start()
	{
		HUDLives = gameObject.GetComponent<Image>();
		HUDLives.sprite = threeLives;
		HUDLives.DOKill();
		FadeInLives();
	}

	void FadeInLives()
	{
		// fade lives to zero instantly, then fade up slowly
		MTween.FadeOut(HUDLives, 0, 0);
		MTween.FadeIn(HUDLives, HUD_FADE_IN_AFTER, timeToFade);
	}

	void OnFadeHud(bool status)
	{
		MTween.FadeOut(HUDLives, HUD_FADE_OUT_AFTER, timeToFade);
	}

	void OnEnable()
	{
		Messenger.AddListener<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
	}
}
