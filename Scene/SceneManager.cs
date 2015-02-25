using UnityEngine;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class SceneManager : CacheBehaviour {

	private float timeToFade = 2f;
	private float fadeInAfter = 0f;
	private float fadeOutAfter = 2f;
	private float timeBeforeLevelReload = 3f;

	void Start() 
	{
		base.CacheComponents();
		spriteRenderer.DOKill();
		
		MTween.FadeInSprite(spriteRenderer, fadeInAfter, timeToFade);
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

	// EVENT RESPONDERS
	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		MTween.FadeOutSprite(spriteRenderer, fadeOutAfter, timeToFade);

		StartCoroutine(Timer.Start(timeBeforeLevelReload, true, () =>
		{
			Application.LoadLevel(Application.loadedLevel);
			System.GC.Collect();
		}));
	}
}
