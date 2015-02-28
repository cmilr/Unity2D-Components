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
	private Image lives;
	private float timeToFade = 2f;

	void Start()
	{
		lives = gameObject.GetComponent<Image>();
		lives.sprite = threeLives;
		lives.DOKill();
		
		MTween.FadeOutImage(lives, 0, 0);
		MTween.FadeInImage(lives, HUD_FADE_IN_AFTER, timeToFade);
	}

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
	}

	void OnFadeHud(bool status)
	{
		MTween.FadeOutImage(lives, HUD_FADE_OUT_AFTER, timeToFade);
	}
}
