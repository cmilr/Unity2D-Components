using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;


public class GameManager : BaseBehaviour
{
	// game stats
	// public static int DifficultyMultiplier = 1;

	// player stats
	public int CurrentScore 	{ get; private set; }
	public int LastSavedScore 	{ get; private set; }
	public int Lives 			{ get; private set; }
	public int CurrentLevel 	{ get; private set; }

	void Start()
	{
		CurrentScore = 0;
		LastSavedScore = 0;
		Lives = 3;
		CurrentLevel = 1;
	}

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<int>( "prize collected", OnPrizeCollected );
		Messenger.AddListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>( "prize collected", OnPrizeCollected );
		Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	// EVENT RESPONDERS
	void OnPrizeCollected(int worth)
	{
		CurrentScore += worth;
		Messenger.Broadcast<int>("change score", CurrentScore);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		CurrentScore = _lastSavedScore;
		_lives -= 1;
	}
}













	// public int CurrentScore
	// {
	// 	get
	// 	{
	// 		return CurrentScore;
	// 	}
	// }