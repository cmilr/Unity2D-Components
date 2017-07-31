using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class LevelManager : BaseBehaviour
{
	public float groundLine = 0f;

	private float fadeInAfter = 0f;
	private float timeToFadeIn = 2f;
	private float fadeOutAfter = 1f;
	private float timeToFadeOut = 2f;
	private float playerPositionY;
	private bool playerAboveGround;
	private Transform player;
	private SpriteRenderer spriteRenderer;
	private Sequence fadeInViewport;
	private Sequence fadeOutViewport;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
	}

	void Start()
	{
		player = GameObject.Find(PLAYER).GetComponent<Transform>();
		Assert.IsNotNull(player);

		(fadeInViewport = MFX.FadeInViewport(spriteRenderer, fadeInAfter, timeToFadeIn)).Pause();
		(fadeOutViewport = MFX.FadeOutViewport(spriteRenderer, fadeOutAfter, timeToFadeOut)).Pause();

		spriteRenderer.enabled = true;
		FadeInNewLevel();
		GetPlayerPosition();
	}

	void OnLoadLevel(int newLevel)
	{
		// load level async in the background, but don't activate.
		var backgroundLoadedScene = SceneManager.LoadSceneAsync("Level" + newLevel);
		backgroundLoadedScene.allowSceneActivation = false;

		// fade camera to black.
		fadeOutViewport.Restart();

		// wait a few seconds, then kill all tweens and load new level.
		StartCoroutine(Timer.Start((fadeOutAfter + timeToFadeOut), false, () =>
		{
			DOTween.KillAll();
			backgroundLoadedScene.allowSceneActivation = true;
		}));
	}

	void FadeInNewLevel()
	{
		fadeInViewport.Restart();
		Invoke("ExplicitGarbageCollection", 1f);
		EventKit.Broadcast("level loading", true);
	}

	void ExplicitGarbageCollection()
	{
		System.GC.Collect();
	}

	void GetPlayerPosition()
	{
		InvokeRepeating("CheckIfAboveGround", 0f, 1F);
	}

	void CheckIfAboveGround()
	{
		if (player.position.y > groundLine)
		{
			if (!playerAboveGround)
			{
				playerAboveGround = true;
				EventKit.Broadcast("player above ground", true);
			}
		}
		else
		{
			if (playerAboveGround)
			{
				playerAboveGround = false;
				EventKit.Broadcast("player above ground", false);
			}
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("load level", OnLoadLevel);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("load level", OnLoadLevel);
	}
}
