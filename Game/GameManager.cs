using UnityEngine;
using System.Collections;


public class GameManager : BaseBehaviour
{
	private _GameData gameData;

	void Start()
	{
		gameData = GameObject.Find(_GAME_DATA).GetComponent<_GameData>();
		Messenger.Broadcast<int>("init score", gameData.CurrentScore);
	}

	void OnPrizeCollected(int worth)
	{
		gameData.CurrentScore += worth;
		Messenger.Broadcast<int>("change score", gameData.CurrentScore);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		gameData.CurrentScore = gameData.LastSavedScore;
		gameData.Lives -= 1;
		Messenger.Broadcast<bool>("fade hud", true);
		Messenger.Broadcast<int>("load level", gameData.CurrentLevel);
	}

	void OnLevelCompleted(bool status)
	{
		gameData.LastSavedScore = gameData.CurrentScore;
		gameData.CurrentLevel = gameData.CurrentLevel;
		Messenger.Broadcast<bool>("fade hud", true);
		Messenger.Broadcast<int>("load level", gameData.CurrentLevel);
	}

	void OnEnable()
	{
		Messenger.AddListener<int>( "prize collected", OnPrizeCollected);
		Messenger.AddListener<string, Collider2D>( "player dead", OnPlayerDead);
		Messenger.AddListener<bool>( "level completed", OnLevelCompleted);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>( "prize collected", OnPrizeCollected );
		Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
		Messenger.RemoveListener<bool>( "level completed", OnLevelCompleted);
	}
}