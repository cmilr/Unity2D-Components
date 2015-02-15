using UnityEngine;
using System.Collections;


public class GameManager : BaseBehaviour
{
	// player stats.
	private static int _currentScore = 0;
	private static int _lastSavedScore = 0;
	private static int _lives = 3;

	// level stats.
	private static int _currentLevel;

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<int>( "prize collected", OnPrizeCollected );
		Messenger.AddListener<bool>( "player dead", OnPlayerDead );
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>( "prize collected", OnPrizeCollected );
		Messenger.RemoveListener<bool>( "player dead", OnPlayerDead );
	}

	// EVENT RESPONDERS
	void OnPrizeCollected(int worth)
	{
		_currentScore += worth;
		Messenger.Broadcast<int>("change score", _currentScore);
	}

	void OnPlayerDead(bool status)
	{
		_currentScore = _lastSavedScore;
	}
}













	// public int CurrentScore
	// {
	// 	get
	// 	{
	// 		return _currentScore;
	// 	}
	// }

	// public int Lives
	// {
	// 	get
	// 	{
	// 		return _lives;
	// 	}
	// }