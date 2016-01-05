using UnityEngine;

public class GameManager : BaseBehaviour
{
	public bool disableAttack;

	private _GameData gameData;

	void Awake()
	{
		_attackDisabled = disableAttack;
	}

	void Start()
	{
		gameData = GameObject.Find(_GAME_DATA).GetComponent<_GameData>();
		EventKit.Broadcast<int>("init score", gameData.CurrentScore);
	}

	void OnPrizeCollected(int worth)
	{
		gameData.CurrentScore += worth;
		EventKit.Broadcast<int>("change score", gameData.CurrentScore);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
	{
		gameData.CurrentScore = gameData.LastSavedScore;
		gameData.Lives -= 1;
		EventKit.Broadcast<bool>("fade hud", true);
		EventKit.Broadcast<int>("load level", gameData.CurrentLevel);
	}

	void OnLevelCompleted(bool status)
	{
		gameData.LastSavedScore = gameData.CurrentScore;
		gameData.CurrentLevel = gameData.CurrentLevel;
		EventKit.Broadcast<bool>("fade hud", true);
		EventKit.Broadcast<int>("load level", gameData.CurrentLevel);
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("prize collected", OnPrizeCollected);
		EventKit.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
		EventKit.Subscribe<bool>("level completed", OnLevelCompleted);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("prize collected", OnPrizeCollected);
		EventKit.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
		EventKit.Unsubscribe<bool>("level completed", OnLevelCompleted);
	}
}
