using UnityEngine;
using System.Collections;


// a clearinghouse for temporary scene state
public class LevelState : BaseBehaviour {

	// player state
	public bool Loading { get; set; }

	void OnLoadLevel(int sceneToLoad)
	{
		Loading = true;
	}

	void OnEnable()
	{
		Messenger.AddListener<int>( "load scene", OnLoadLevel);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>( "load scene", OnLoadLevel);
	}
}
