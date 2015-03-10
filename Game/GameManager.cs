using UnityEngine;
using System.Collections;


public class GameManager : BaseBehaviour
{
	private GameData gData;

	void Start()
	{
		gData = GameObject.Find("_GameData").GetComponent<GameData>();

		Messenger.Broadcast<int>("init score", gData.CurrentScore);
	}

	void OnPrizeCollected(int worth)
	{
		gData.CurrentScore += worth;
		Messenger.Broadcast<int>("change score", gData.CurrentScore);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		gData.CurrentScore = gData.LastSavedScore;
		gData.Lives -= 1;
		Messenger.Broadcast<bool>("fade hud", true);
		Messenger.Broadcast<int>("load level", gData.CurrentLevel);
	}

	void OnLevelCompleted(bool status)
	{
		gData.LastSavedScore = gData.CurrentScore;
		gData.CurrentLevel = gData.CurrentLevel;
		Messenger.Broadcast<bool>("fade hud", true);
		Messenger.Broadcast<int>("load level", gData.CurrentLevel);
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













	// public int gData.CurrentScore
	// {
	// 	get
	// 	{
	// 		return gData.CurrentScore;
	// 	}
	// }