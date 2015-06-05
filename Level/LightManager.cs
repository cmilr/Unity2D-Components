using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;


public class LightManager : CacheBehaviour {

	private Light playerLight;
	private Light creatureLight;
	private Light pickupLight;
	private Light illuminatedPickupLight;
	private Light tileLight;
	private Light planeLight;
	private float fadeAfter                    = 0f;
	private float timeToFade                   = 1f;

	// above ground light intensity
	private float playerAboveGround            = 1.95f;
	private float creatureAboveGround          = 1.95f;
	private float tileAboveGround              = 1.44f;
	private float planeAboveGround             = .13f;
	private float pickupAboveGround            = 1.95f;
	private float illuminatedPickupAboveGround = 1.95f;
	// below ground light intensity
	private float playerBelowGround            = 1.2f;
	private float creatureBelowGround          = 1.5f;
	private float tileBelowGround              = .64f;
	private float planeBelowGround             = .13f;
	private float pickupBelowGround            = 1.1f;
	private float illuminatedPickupBelowGround = 1.55f;

	void Start ()
	{
		playerLight = GameObject.Find("PlayerLight").GetComponent<Light>();
		creatureLight = GameObject.Find("CreatureLight").GetComponent<Light>();
		tileLight = GameObject.Find("TileLight").GetComponent<Light>();
		planeLight = GameObject.Find("PlaneLight").GetComponent<Light>();
		pickupLight = GameObject.Find("PickupLight").GetComponent<Light>();
		illuminatedPickupLight = GameObject.Find("IlluminatedPickupLight").GetComponent<Light>();
	}

	void OnPlayerAboveGround(bool aboveGround)
	{
		if (aboveGround)
		{
			MTween.FadeIntensity(playerLight, playerAboveGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(creatureLight, creatureAboveGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(tileLight, tileAboveGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(planeLight, planeAboveGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(pickupLight, pickupAboveGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(illuminatedPickupLight, illuminatedPickupAboveGround, fadeAfter, timeToFade);
		}
		else
		{
			MTween.FadeIntensity(playerLight, playerBelowGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(creatureLight, creatureBelowGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(tileLight, tileBelowGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(planeLight, planeBelowGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(pickupLight, pickupBelowGround, fadeAfter, timeToFade);
			MTween.FadeIntensity(illuminatedPickupLight, illuminatedPickupBelowGround, fadeAfter, timeToFade);
		}
	}

	void OnEnable()
	{
		Messenger.AddListener<bool>("player above ground", OnPlayerAboveGround);
	}

	void OnDisable()
	{
		Messenger.RemoveListener<bool>("player above ground", OnPlayerAboveGround);
	}
}
