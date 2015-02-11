using UnityEngine;
using System.Collections;


public class GameData : MonoBehaviour
{
	// player stats.
	private static int _currentScore;
	private static int _lastSavedScore;
	private static int _lives = 3;

	// level stats.
	private static int _currentLevel;

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<int>( "prize collected", OnPrizeCollected );
	}

	void OnDisable()
	{
		Messenger.RemoveListener<int>( "prize collected", OnPrizeCollected );
	}

	// EVENT RESPONDERS
	void OnPrizeCollected(int worth)
	{
		_currentScore += worth;
		Messenger.Broadcast<int>("change score", _currentScore);
	}

	public int CurrentScore
	{
		get
		{
			return _currentScore;
		}
	}

	public int Lives
	{
		get
		{
			return _lives;
		}
	}
}
