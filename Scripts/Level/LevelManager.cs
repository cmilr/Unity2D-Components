using Matcha.Dreadful;
using System.Collections;
using UnityEngine;

public class LevelManager : CacheBehaviour
{
	public float groundLine             = -50.00f;     // where above-ground ends and below-ground begins

	private float timeToFade            = 2f;
	private float fadeInAfter           = 2f;
	private float fadeOutAfter          = 0f;
	private float timeBeforeLevelReload = 3.2f;
	private float playerPositionY;
	private bool playerAboveGround;

	void Start()
	{
		spriteRenderer.enabled = true;

		FadeInNewLevel();
		GetPlayerPosition();
	}

	void FadeInNewLevel()
	{
		MFX.FadeInLevel(spriteRenderer, fadeOutAfter, timeToFade);
		EventKit.Broadcast<bool>("level loading", true);
	}

	void FadeOutCurrentLevel()
	{
		MFX.FadeOutLevel(spriteRenderer, fadeInAfter, timeToFade);
	}

	void GetPlayerPosition()
	{
		InvokeRepeating("CheckIfAboveGround", 0f, 0.5F);
	}

	void CheckIfAboveGround()
	{
		if (playerPositionY > groundLine)
		{
			// if player is not ALREADY above ground, broadcast message "player above ground"
			if (!playerAboveGround) {
				playerAboveGround = true;
				EventKit.Broadcast<bool>("player above ground", true);
			}
		}
		else
		{
			// if player is not ALREADY below ground, broadcast message !"player above ground"
			if (playerAboveGround) {
				playerAboveGround = false;
				EventKit.Broadcast<bool>("player above ground", false);
			}
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("load level", OnLoadLevel);
		EventKit.Subscribe<float, float>("player position", OnPlayerPosition);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("load level", OnLoadLevel);
		EventKit.Unsubscribe<float, float>("player position", OnPlayerPosition);
	}

	void OnLoadLevel(int newLevel)
	{
		FadeOutCurrentLevel();

		// load next level and trigger garbage collection
		StartCoroutine(Timer.Start(timeBeforeLevelReload, false, () =>
		{
			Application.LoadLevel("Level" + newLevel);
			System.GC.Collect();
		}));
	}

	void OnPlayerPosition(float x, float y)
	{
		playerPositionY = y;
	}
}
