using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;


public class LightManager : CacheBehaviour {

	private float fadeAfter = 0f;
	private float timeToFade = 1f;
	private IPlayerStateReadOnly player;
	private Light playerLight;
	private Light creatureLight;
	private Light pickupLight;
	private Light tileLight;
	private Light planeLight;
	// coordinates on TileMap where ground begins
	private float groundLine;
	// above ground light intensity
	private float playerAboveGround = 1.95f;
	private float creatureAboveGround = 1.95f;
	private float pickupAboveGround = 1.95f;
	private float tileAboveGround = 1.44f;
	private float planeAboveGround = .13f;
	// below ground light intensity
	private float playerBelowGround = 1.17f;
	private float creatureBelowGround = 1.17f;
	private float pickupBelowGround = 1.1f;
	private float tileBelowGround = .64f;
	private float planeBelowGround = .13f;

	void Start ()
	{
		player = GameObject.Find("Player").GetComponent<IPlayerStateReadOnly>();
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
		if (player.GetY() > groundLine)
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
		MTween.FadeIntensity(playerLight, playerAboveGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(creatureLight, creatureAboveGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(pickupLight, pickupAboveGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(tileLight, tileAboveGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(planeLight, planeAboveGround, fadeAfter, timeToFade);
	}

	void OnBelowGround()
	{
		MTween.FadeIntensity(playerLight, playerBelowGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(creatureLight, creatureBelowGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(pickupLight, pickupBelowGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(tileLight, tileBelowGround, fadeAfter, timeToFade);
		MTween.FadeIntensity(planeLight, planeBelowGround, fadeAfter, timeToFade);
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
