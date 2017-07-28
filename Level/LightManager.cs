using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class LightManager : BaseBehaviour
{
	private Light playerLight;
	private Light creatureLight;
	private Light pickupLight;
	private Light tileLight;
	private Light decorationLight;
	private float fadeAfter							= 0f;
	private float timeToFade						= 1f;
	// above ground light intensity
	private float playerAboveGround					= 1.95f;
	private float creatureAboveGround				= 1.95f;
	private float tileAboveGround					= 1.44f;
	private float pickupAboveGround					= 1.95f;
	private float decorationAboveGround				= 1.95f;

	// below ground light intensity
	private float playerBelowGround					= 1.56f;
	private float creatureBelowGround				= 2f;
	private float tileBelowGround					= 1.4f;
	private float pickupBelowGround					= 1.55f;
	private float decorationBelowGround				= 1.65f;

	private Sequence playerAboveGroundTween;
	private Sequence creatureAboveGroundTween;
	private Sequence tileAboveGroundTween;
	private Sequence pickupAboveGroundTween;
	private Sequence decorationAboveGroundTween;

	private Sequence playerBelowGroundTween;
	private Sequence creatureBelowGroundTween;
	private Sequence tileBelowGroundTween;
	private Sequence pickupBelowGroundTween;
	private Sequence decorationBelowGroundTween;

	void Start()
	{
		playerLight = GameObject.Find("PlayerLight").GetComponent<Light>();
		Assert.IsNotNull(playerLight);

		creatureLight = GameObject.Find("CreatureLight").GetComponent<Light>();
		Assert.IsNotNull(creatureLight);

		tileLight = GameObject.Find("TileLight").GetComponent<Light>();
		Assert.IsNotNull(tileLight);

		pickupLight = GameObject.Find("PickupLight").GetComponent<Light>();
		Assert.IsNotNull(pickupLight);

		decorationLight = GameObject.Find("DecorationLight").GetComponent<Light>();
		Assert.IsNotNull(decorationLight);

		// cache & pause tweens.
		(playerAboveGroundTween = MFX.FadeIntensity(playerLight, playerAboveGround, fadeAfter, timeToFade)).Pause();
		(creatureAboveGroundTween = MFX.FadeIntensity(creatureLight, creatureAboveGround, fadeAfter, timeToFade)).Pause();
		(tileAboveGroundTween = MFX.FadeIntensity(tileLight, tileAboveGround, fadeAfter, timeToFade)).Pause();
		(pickupAboveGroundTween = MFX.FadeIntensity(pickupLight, pickupAboveGround, fadeAfter, timeToFade)).Pause();
		(decorationAboveGroundTween = MFX.FadeIntensity(decorationLight, pickupAboveGround, fadeAfter, timeToFade)).Pause();

		(playerBelowGroundTween = MFX.FadeIntensity(playerLight, playerBelowGround, fadeAfter, timeToFade)).Pause();
		(creatureBelowGroundTween = MFX.FadeIntensity(creatureLight, creatureBelowGround, fadeAfter, timeToFade)).Pause();
		(tileBelowGroundTween = MFX.FadeIntensity(tileLight, tileBelowGround, fadeAfter, timeToFade)).Pause();
		(pickupBelowGroundTween = MFX.FadeIntensity(pickupLight, pickupBelowGround, fadeAfter, timeToFade)).Pause();
		(decorationBelowGroundTween = MFX.FadeIntensity(decorationLight, pickupBelowGround, fadeAfter, timeToFade)).Pause();

	}

	void OnPlayerAboveGround(bool aboveGround)
	{
		if (aboveGround)
		{
			playerAboveGroundTween.Restart();
			creatureAboveGroundTween.Restart();
			tileAboveGroundTween.Restart();
			pickupAboveGroundTween.Restart();
			decorationAboveGroundTween.Restart();

		}
		else
		{
			playerBelowGroundTween.Restart();
			creatureBelowGroundTween.Restart();
			tileBelowGroundTween.Restart();
			pickupBelowGroundTween.Restart();
			decorationBelowGroundTween.Restart();
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
