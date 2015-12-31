using System.Collections;

// a clearinghouse for temporary game state
public class GameState : BaseBehaviour, IGameStateReadOnly, IGameStateFullAccess
{
	// game state
	public bool LevelLoading      { get; set; }

	void OnLoadLevel(int unused)
	{
		LevelLoading = true;
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
