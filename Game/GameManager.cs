using UnityEngine;
using System.Collections;


public class GameManager : BaseBehaviour
{
	private GameData data;

	void Start()
	{
		data = GameObject.Find("_GameData").GetComponent<GameData>();

		Messenger.Broadcast<int>("init score", data.CurrentScore);
	}

	// EVENT LISTENERS
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

	// EVENT RESPONDERS
	void OnPrizeCollected(int worth)
	{
		data.CurrentScore += worth;
		Messenger.Broadcast<int>("change score", data.CurrentScore);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		data.CurrentScore = data.LastSavedScore;
		data.Lives -= 1;
		Messenger.Broadcast<bool>("fade hud", true);
		Messenger.Broadcast<int>("load level", data.CurrentLevel);
	}

	void OnLevelCompleted(bool status)
	{
		data.LastSavedScore = data.CurrentScore;
		data.CurrentLevel = data.CurrentLevel;
		Messenger.Broadcast<bool>("fade hud", true);
		Messenger.Broadcast<int>("load level", data.CurrentLevel);
		Debug.Log("LEVEL COMPLETED!!");
	}
}













	// public int data.CurrentScore
	// {
	// 	get
	// 	{
	// 		return data.CurrentScore;
	// 	}
	// }