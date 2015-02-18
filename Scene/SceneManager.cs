using UnityEngine;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class SceneManager : CacheBehaviour {

	private float timeToFade = 2f;
	private float timeBeforeLevelReload = 3f;

	void Start() 
	{
		base.CacheComponents();
		spriteRenderer.DOKill();
		
		MTween.FadeIn(spriteRenderer, timeToFade);
	}

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<string, bool>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, bool>( "player dead", OnPlayerDead);
	}

	// EVENT RESPONDERS
	void OnPlayerDead(string methodOfDeath, bool value)
	{
		MTween.FadeOut(spriteRenderer, timeToFade);

		StartCoroutine(Timer.Start(timeBeforeLevelReload, true, () =>
		{
			Application.LoadLevel(Application.loadedLevel);
		}));
	}
}
