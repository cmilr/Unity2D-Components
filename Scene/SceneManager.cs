using UnityEngine;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class SceneManager : CacheBehaviour {

	// private SceneData sData;
	private float timeToFade = 2f;
	private float fadeInAfter = 0f;
	private float fadeOutAfter = 2f;
	private float timeBeforeLevelReload = 3f;

	void Start() 
	{
		// sData = GameObject.Find("_SceneData").GetComponent<SceneData>();
		spriteRenderer.DOKill();
		
		MTween.FadeInSprite(spriteRenderer, fadeInAfter, timeToFade);
	}

	void OnLoadLevel(int newLevel)
	{
		MTween.FadeOutSprite(spriteRenderer, fadeOutAfter, timeToFade);

		StartCoroutine(Timer.Start(timeBeforeLevelReload, true, () =>
		{
			Application.LoadLevel("Scene" + newLevel);
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