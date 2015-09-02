using UnityEngine;
using System.Collections;
using Matcha.Dreadful.FX;


public class LevelManager : CacheBehaviour {

	private IPlayerStateReadOnly player;

	// screen-fader settings
	private float timeToFade            = 2f;
	private float fadeInAfter           = 2f;
	private float fadeOutAfter          = 0f;
	private float timeBeforeLevelReload = 3f;

	// tile map specs
	public float groundLine             = -50.00f;		// where above-ground ends and below-ground begins

	void Start()
	{
		player = GameObject.Find(PLAYER).GetComponent<IPlayerStateReadOnly>();
		spriteRenderer.enabled = true;

		FadeInNewLevel();
		GetPlayerPosition();
	}

	void FadeInNewLevel()
	{
		MFX.FadeInLevel(spriteRenderer, fadeOutAfter, timeToFade);
		Messenger.Broadcast<bool>("level loading", true);
	}

	void FadeOutCurrentLevel()
	{
		MFX.FadeOutLevel(spriteRenderer, fadeInAfter, timeToFade);
	}

	void OnLoadLevel(int newLevel)
	{
		FadeOutCurrentLevel();

		// load next level and trigger garbage collection
		StartCoroutine(Timer.Start(timeBeforeLevelReload, true, () =>
		{
			Application.LoadLevel("Level" + newLevel);
			System.GC.Collect();
		}));
	}

	void GetPlayerPosition()
	{
		InvokeRepeating("CheckIfAboveGround", 0f, 0.5F);
	}

	void CheckIfAboveGround()
	{
		if (player.Y > groundLine)
		{
			// if player is not ALREADY above ground, broadcast message "player above ground"
			if (!player.AboveGround)
				Messenger.Broadcast<bool>("player above ground", true);
		}
		else
		{
			// if player is not ALREADY below ground, broadcast message !"player above ground"
			if (player.AboveGround)
				Messenger.Broadcast<bool>("player above ground", false);
		}
	}

	void OnEnable()
	{
		Messenger.AddListener<int>("load level", OnLoadLevel);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>("load level", OnLoadLevel);
	}
}