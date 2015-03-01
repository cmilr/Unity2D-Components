using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;

public class SunlightManager : MonoBehaviour {

	private float aboveGround = .71f;
	private float belowGround = .45f;

	void Start () {
		light.intensity = aboveGround;
	}
	
	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<bool>( "player above ground", OnPlayerAboveGround);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>( "player above ground", OnPlayerAboveGround);
	}

	// EVENT RESPONDERS
	void OnPlayerAboveGround(bool status)
	{
		float targetIntensity = status ? aboveGround : belowGround;
		float fadeAfter = 0f;
		float timeToFade = 1f;

		MTween.FadeIntensity(light, targetIntensity, fadeAfter, timeToFade);
	}
}
