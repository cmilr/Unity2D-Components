using Matcha.Dreadful;
using System.Collections;
using UnityEngine;

public class LightManager : CacheBehaviour
{
	private Light playerLight;
	private Light creatureLight;
	private Light pickupLight;
	private Light tileLight;
	// private Light planeLight;
	// private Light illuminatedPickupLight;
	private float fadeAfter                    = 0f;
	private float timeToFade                   = 1f;

	// above ground light intensity
	private float playerAboveGround            = 1.95f;
	private float creatureAboveGround          = 1.95f;
	private float tileAboveGround              = 1.44f;
	private float pickupAboveGround            = 1.95f;
	// private float planeAboveGround             = .13f;
	// private float illuminatedPickupAboveGround = 1.95f;

	// below ground light intensity
	private float playerBelowGround            = 1.56f;
	private float creatureBelowGround          = 2f;
	private float tileBelowGround              = 1.4f;
	private float pickupBelowGround            = 1.55f;
	// private float planeBelowGround             = 0f;
	// private float illuminatedPickupBelowGround = 1.55f;

	void Start()
	{
		playerLight            = GameObject.Find("PlayerLight").GetComponent<Light>();
		creatureLight          = GameObject.Find("CreatureLight").GetComponent<Light>();
		tileLight              = GameObject.Find("TileLight").GetComponent<Light>();
		pickupLight            = GameObject.Find("PickupLight").GetComponent<Light>();
		// planeLight             = GameObject.Find("PlaneLight").GetComponent<Light>();
		// illuminatedPickupLight = GameObject.Find("IlluminatedPickupLight").GetComponent<Light>();
	}

	void OnPlayerAboveGround(bool aboveGround)
	{
		if (aboveGround)
		{
			MFX.FadeIntensity(playerLight, playerAboveGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(creatureLight, creatureAboveGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(tileLight, tileAboveGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(pickupLight, pickupAboveGround, fadeAfter, timeToFade);
			// MFX.FadeIntensity(planeLight, planeAboveGround, fadeAfter, timeToFade);
			// MFX.FadeIntensity(illuminatedPickupLight, illuminatedPickupAboveGround, fadeAfter, timeToFade);
		}
		else
		{
			MFX.FadeIntensity(playerLight, playerBelowGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(creatureLight, creatureBelowGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(tileLight, tileBelowGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(pickupLight, pickupBelowGround, fadeAfter, timeToFade);
			// MFX.FadeIntensity(planeLight, planeBelowGround, fadeAfter, timeToFade);
			// MFX.FadeIntensity(illuminatedPickupLight, illuminatedPickupBelowGround, fadeAfter, timeToFade);
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
