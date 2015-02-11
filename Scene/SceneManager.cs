using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	// EVENT LISTENERS
	void OnEnable()
	{
		// Messenger.AddListener<bool>( "player touching top of screen", OnPlayerTouchingTopOfScreen);
		Messenger.AddListener<bool>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		// Messenger.AddListener<bool>( "player touching top of screen", OnPlayerTouchingTopOfScreen);
		Messenger.RemoveListener<bool>( "player dead", OnPlayerDead);
	}

	// EVENT RESPONDERS
	void OnPlayerDead(bool value)
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
