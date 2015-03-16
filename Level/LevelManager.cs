using UnityEngine;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class LevelManager : CacheBehaviour {

	// private LevelData level;
	private float timeToFade            = 2f;
	private float fadeInAfter           = 0f;
	private float fadeOutAfter          = 2f;
	private float timeBeforeLevelReload = 3f;

	void Start()
	{
		// level = GameObject.Find("_LevelData").GetComponent<LevelData>();
		spriteRenderer.DOKill();
		spriteRenderer.enabled = true;

		// fade in new level
		MTween.FadeIn(spriteRenderer, fadeInAfter, timeToFade);
	}

	void OnLoadLevel(int newLevel)
	{
		// fade out current level
		MTween.FadeOut(spriteRenderer, fadeOutAfter, timeToFade);

		// start next level and trigger garbage collection
		StartCoroutine(Timer.Start(timeBeforeLevelReload, true, () =>
		{
			Application.LoadLevel("Level" + newLevel);
			System.GC.Collect();
		}));
	}

	void OnEnable()
	{
		Messenger.AddListener<int>( "load level", OnLoadLevel);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>( "load level", OnLoadLevel);
	}
}