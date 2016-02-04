using Matcha.Dreadful;
using UnityEngine;

public class LightManager : CacheBehaviour
{
	private Light playerLight;
	private Light creatureLight;
	private Light pickupLight;
	private Light tileLight;
	private float fadeAfter                    = 0f;
	private float timeToFade                   = 1f;

	// above ground light intensity
	private float playerAboveGround            = 1.95f;
	private float creatureAboveGround          = 1.95f;
	private float tileAboveGround              = 1.44f;
	private float pickupAboveGround            = 1.95f;

	// below ground light intensity
	private float playerBelowGround            = 1.56f;
	private float creatureBelowGround          = 2f;
	private float tileBelowGround              = 1.4f;
	private float pickupBelowGround            = 1.55f;

	void Start()
	{
		playerLight            = GameObject.Find("PlayerLight").GetComponent<Light>();
		creatureLight          = GameObject.Find("CreatureLight").GetComponent<Light>();
		tileLight              = GameObject.Find("TileLight").GetComponent<Light>();
		pickupLight            = GameObject.Find("PickupLight").GetComponent<Light>();
	}

	void OnPlayerAboveGround(bool aboveGround)
	{
		if (aboveGround)
		{
			MFX.FadeIntensity(playerLight, playerAboveGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(creatureLight, creatureAboveGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(tileLight, tileAboveGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(pickupLight, pickupAboveGround, fadeAfter, timeToFade);
		}
		else
		{
			MFX.FadeIntensity(playerLight, playerBelowGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(creatureLight, creatureBelowGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(tileLight, tileBelowGround, fadeAfter, timeToFade);
			MFX.FadeIntensity(pickupLight, pickupBelowGround, fadeAfter, timeToFade);
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("player above ground", OnPlayerAboveGround);
	}

	void OnDisable()
	{
		EventKit.Unsubscribe<bool>("player above ground", OnPlayerAboveGround);
	}
}
