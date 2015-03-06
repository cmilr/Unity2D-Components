using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;


public class LightManager : CacheBehaviour {

	private float fadeAfter = 0f;
	private float timeToFade = 1f;
	private PlayerState playerState;
	private Light playerLight;
	private Light creatureLight;
	private Light pickupLight;
	private Light tileLight;
	private Light planeLight;
<<<<<<< HEAD:Assets/Scripts/Scene/LightManager.cs
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
=======
	private float groundLine;						// coordinates on TileMap where above ground and below ground meet
	private float aboveGround = 1.95f;				// light intensity when player is above ground
	private float playerBelowGround = 1.17f;		// light intensity when player is below ground
	private float creatureBelowGround = 1.17f;		// light intensity when player is below ground
	private float pickupBelowGround = 1.1f;			// light intensity when player is below ground
	private float tileBelowGround = .64f;			// light intensity when player is below ground
	private float planeAboveBelowGround = .13f;		// light intensity when player is below ground
>>>>>>> c241ff51d911cd89ed8dc6a108d25085e4e7771a:Assets/Scripts/Scene/SunlightManager.cs

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
