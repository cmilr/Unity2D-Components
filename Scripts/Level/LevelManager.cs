using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : CacheBehaviour
{
	public float groundLine             = -50.00f;
	private float timeToFade            = 2f;
	private float fadeInAfter           = 2f;
	private float fadeOutAfter          = 0f;
	private float timeBeforeLevelReload = 3.2f;
	private float playerPositionY;
	private bool playerAboveGround;
	private Transform player;

	void Start()
	{
		player = GameObject.Find(PLAYER).GetComponent<Transform>();
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
		if (player.position.y > groundLine)
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

	void OnLoadLevel(int newLevel)
	{
		FadeOutCurrentLevel();

		//load next level
		StartCoroutine(Timer.Start(timeBeforeLevelReload, false, () =>
		{
			SceneManager.LoadScene("Level" + newLevel);
		}));
	}

	void OnLevelWasLoaded()
	{
		//trigger garbage collection
		System.GC.Collect();
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
