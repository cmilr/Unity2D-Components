using UnityEngine;
using System.Collections;


public class GameManager : BaseBehaviour
{
	// game stats
	// private static int _difficultyMultiplier = 1;

	// player stats
	private static int _currentScore = 0;
	private static int _lastSavedScore = 0;
	private static int _lives = 3;

	// level stats
	private static int _currentLevel;

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
		_currentScore += worth;
		Messenger.Broadcast<int>("change score", _currentScore);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		_currentScore = _lastSavedScore;
		_lives -= 1;
	}
}













	// public int CurrentScore
	// {
	// 	get
	// 	{
	// 		return _currentScore;
	// 	}
	// }