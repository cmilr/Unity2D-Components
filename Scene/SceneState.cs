using UnityEngine;
using System.Collections;


// a clearinghouse for temporary scene state
public class SceneState : BaseBehaviour {

	// player state
	public bool Loading { get; set; }

	void OnLoadLevel(int levelToLoad)
	{
		Loading = true;
	}

	void OnEnable()
	{
		Messenger.AddListener<int>( "load level", OnLoadLevel);	
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>( "load level", OnLoadLevel);	
	}
}
