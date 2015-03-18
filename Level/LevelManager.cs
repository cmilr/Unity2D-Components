﻿using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;


public class LevelManager : CacheBehaviour {

	private IPlayerStateReadOnly player;

	// screen-fader settings
	private float timeToFade            = 2f;
	private float fadeInAfter           = 0f;
	private float fadeOutAfter          = 2f;
	private float timeBeforeLevelReload = 3f;

	// tile map specs
	public float groundLine             = -50.00f;

	void Start()
	{
		player = GameObject.Find(PLAYER).GetComponent<IPlayerStateReadOnly>();
		spriteRenderer.enabled = true;

		FadeInNewLevel();
		GetPlayerPosition();
	}

	void FadeInNewLevel()
	{
		MTween.FadeIn(spriteRenderer, fadeInAfter, timeToFade);
	}

	void FadeOutCurrentLevel()
	{
		MTween.FadeOut(spriteRenderer, fadeOutAfter, timeToFade);
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
		Messenger.AddListener<int>( "load level", OnLoadLevel);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>( "load level", OnLoadLevel);
	}
}