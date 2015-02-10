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

    public static int GetScore()
	{
		return _currentScore;
	}

	public static void IncreaseScore(int addScore)
	{
		_currentScore += addScore;
	}

	public static int GetLives()
	{
		return _lives;
	}

	public static void IncreaseLives(int addLives)
	{
		_lives += addLives;
	}
}
