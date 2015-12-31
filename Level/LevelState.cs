using System.Collections;
using UnityEngine;

// a clearinghouse for temporary scene state
public class LevelState : BaseBehaviour
{
	// player state
	public bool Loading { get; set; }

	void OnLoadLevel(int LevelToLoad)
	{
		Loading = true;
	}

	void OnEnable()
	{
		Evnt.Subscribe<int>("load level", OnLoadLevel);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<int>("load level", OnLoadLevel);
	}
}
