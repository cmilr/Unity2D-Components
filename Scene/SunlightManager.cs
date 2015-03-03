using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;

public class SunlightManager : CacheBehaviour {

	private GameObject player;
	private PlayerState playerState;
	private int groundline;
	private float aboveGround = .71f;
	private float belowGround = .45f;

	void Start () 
	{
		player = GameObject.Find("Player");
		playerState = player.GetComponent<PlayerState>();

		light.intensity = aboveGround;

		InvokeRepeating("CheckIfAboveGround", 0f, 0.3F);
	}

	void CheckIfAboveGround()
	{
		if (playerState.GetY() > groundline)
		{
			Messenger.Broadcast<bool>("player above ground", true);
		}
		else 
		{
			Messenger.Broadcast<bool>("player above ground", false);
		}
	}
	

	void OnEnable()
	{
		Messenger.AddListener<int>("set groundline", OnSetGroundline);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>("set groundline", OnSetGroundline);
	}

	void OnSetGroundline(int coordinates)
	{
		groundline = coordinates;
	}

	void OnPlayerAboveGround(bool status)
	{
		float targetIntensity = status ? aboveGround : belowGround;
		float fadeAfter = 0f;
		float timeToFade = 1f;

		MTween.FadeIntensity(light, targetIntensity, fadeAfter, timeToFade);
	}
}
