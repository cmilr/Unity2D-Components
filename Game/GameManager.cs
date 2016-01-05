using UnityEngine;

public class GameManager : BaseBehaviour
{
	private _GameData gameData;
	public bool disableAttack;

	void Awake()
	{
		_attackDisabled = disableAttack;
	}

	void Start()
	{
		gameData = GameObject.Find(_GAME_DATA).GetComponent<_GameData>();
		Evnt.Broadcast<int>("init score", gameData.CurrentScore);
	}

	void OnPrizeCollected(int worth)
	{
		gameData.CurrentScore += worth;
		Evnt.Broadcast<int>("change score", gameData.CurrentScore);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
	{
		gameData.CurrentScore = gameData.LastSavedScore;
		gameData.Lives -= 1;
		Evnt.Broadcast<bool>("fade hud", true);
		Evnt.Broadcast<int>("load level", gameData.CurrentLevel);
	}

	void OnLevelCompleted(bool status)
	{
		gameData.LastSavedScore = gameData.CurrentScore;
		gameData.CurrentLevel = gameData.CurrentLevel;
		Evnt.Broadcast<bool>("fade hud", true);
		Evnt.Broadcast<int>("load level", gameData.CurrentLevel);
	}

	void OnEnable()
	{
		Evnt.Subscribe<int>("prize collected", OnPrizeCollected);
		Evnt.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
		Evnt.Subscribe<bool>("level completed", OnLevelCompleted);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<int>("prize collected", OnPrizeCollected);
		Evnt.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
		Evnt.Unsubscribe<bool>("level completed", OnLevelCompleted);
	}
}
