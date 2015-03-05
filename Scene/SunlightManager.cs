using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;


public class SunlightManager : CacheBehaviour {

	private float fadeAfter = 0f;
	private float timeToFade = 1f;
	private PlayerState playerState;
	private Light playerLight;
	private Light creatureLight;
	private Light pickupLight;
	private Light tileLight;
	private Light planeLight;
	private float groundLine;						// coordinates on TileMap where above ground and below ground meet
	private float aboveGround = 1.44f;				// light intensity when player is above ground
	private float playerBelowGround = .82f;			// light intensity when player is below ground
	private float creatureBelowGround = .82f;		// light intensity when player is below ground
	private float pickupBelowGround = .82f;			// light intensity when player is below ground
	private float tileBelowGround = .64f;			// light intensity when player is below ground
	private float planeAboveBelowGround = .13f;		// light intensity when player is below ground

	void Start () 
	{
		playerState = GameObject.Find("Player").GetComponent<PlayerState>();
		playerLight = GameObject.Find("PlayerLight").GetComponent<Light>();
		creatureLight = GameObject.Find("CreatureLight").GetComponent<Light>();
		pickupLight = GameObject.Find("PickupLight").GetComponent<Light>();
		tileLight = GameObject.Find("TileLight").GetComponent<Light>();
		planeLight = GameObject.Find("PlaneLight").GetComponent<Light>();

		OnAboveGround();

		InvokeRepeating("CheckIfAboveGround", 0f, 0.3F);
	}

	void CheckIfAboveGround()
	{
		if (playerState.GetY() > groundLine)
		{
			OnAboveGround();
			Messenger.Broadcast<bool>("player above ground", true);
		}
		else 
		{
			OnBelowGround();
			Messenger.Broadcast<bool>("player above ground", false);
		}
	}

	void OnAboveGround()
	{
		playerLight.intensity = aboveGround;
		creatureLight.intensity = aboveGround;
		pickupLight.intensity = aboveGround;
		tileLight.intensity = aboveGround;
		planeLight.intensity = planeAboveBelowGround;
	}

	void OnBelowGround()
	{
		MTween.FadeIntensity(playerLight, playerBelowGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(creatureLight, creatureBelowGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(pickupLight, pickupBelowGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(tileLight, tileBelowGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(planeLight, planeAboveBelowGround, fadeAfter, timeToFade);
	}
	
	void OnSetGroundLine(float coordinates)
	{
		groundLine = coordinates;
	}

	void OnEnable()
	{
		Messenger.AddListener<float>("set ground line", OnSetGroundLine);
	}

	void OnDisable()
	{
		Messenger.RemoveListener<float>("set ground line", OnSetGroundLine);
	}
}
