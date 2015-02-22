using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class UpdateHUDLives : BaseBehaviour
{
	public Sprite threeLives;
	public Sprite twoLives;
	public Sprite oneLife;
	private Image lives;
	private float fadeInAfter = .5f;
	private float fadeOutAfter = .5f;
	private float timeToFade = 2f;

	void Start()
	{
		lives = gameObject.GetComponent<Image>();
		lives.sprite = threeLives;
		lives.DOKill();
		
		MTween.FadeOutImage(lives, 0, 0);
		MTween.FadeInImage(lives, fadeInAfter, timeToFade);
	}

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		MTween.FadeOutImage(lives, fadeOutAfter, timeToFade);
	}
}
