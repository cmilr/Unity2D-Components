using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;


public class SunlightManager : CacheBehaviour {

	private GameObject player;
	private PlayerState playerState;
	private float groundLine;				// coordinates on TileMap where above ground and below ground meet
	private float aboveGround = 1.45f;		// light intensity when player is above ground
	private float belowGround = .9f;		// light intensity when player is below ground

	void Start () 
	{
		player = GameObject.Find("Player");
		playerState = player.GetComponent<PlayerState>();

		light.intensity = aboveGround;

		InvokeRepeating("CheckIfAboveGround", 0f, 0.3F);
	}

	void CheckIfAboveGround()
	{
		float targetIntensity;
		float fadeAfter = 0f;
		float timeToFade = 1f;

		if (playerState.GetY() > groundLine)
		{
			targetIntensity = aboveGround;
			Messenger.Broadcast<bool>("player above ground", true);
		}
		else 
		{
			targetIntensity = belowGround;
			Messenger.Broadcast<bool>("player above ground", false);
		}

		MTween.FadeIntensity(light, targetIntensity, fadeAfter, timeToFade);
	}
	
	void OnSetGroundLine(float coordinates)
	{
		groundLine = coordinates;
	}

	void OnEnable()
	{
		Messenger.AddListener<float>("set groundLine", OnSetGroundLine);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<float>("set groundLine", OnSetGroundLine);
	}
}
