using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : BaseBehaviour
{
	private GameData gameData;

	void Awake()
	{
		EventKit.Broadcast("wake singletons");
	}

	void Start()
	{
		gameData = GameObject.Find(_DATA).GetComponent<GameData>();
		Assert.IsNotNull(gameData);
		
		EventKit.Broadcast("init score", gameData.CurrentScore);
		EventKit.Broadcast("set difficulty", NORMAL);
	}

	void OnPrizeCollected(int worth)
	{
		gameData.CurrentScore += worth;
		EventKit.Broadcast("change score", gameData.CurrentScore);
	}

	void OnPlayerDead(Hit incomingHit)
	{
		gameData.CurrentScore = gameData.LastSavedScore;
		gameData.Lives -= 1;
		EventKit.Broadcast("fade hud", true);
		EventKit.Broadcast("load level", gameData.CurrentLevel);
	}

	void OnLevelCompleted(bool status)
	{
		gameData.LastSavedScore = gameData.CurrentScore;
		gameData.CurrentLevel = gameData.CurrentLevel;
		EventKit.Broadcast("fade hud", true);
		EventKit.Broadcast("load level", gameData.CurrentLevel);
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("prize collected", OnPrizeCollected);
		EventKit.Subscribe<Hit>("player dead", OnPlayerDead);
		EventKit.Subscribe<bool>("level completed", OnLevelCompleted);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("prize collected", OnPrizeCollected);
		EventKit.Unsubscribe<Hit>("player dead", OnPlayerDead);
		EventKit.Unsubscribe<bool>("level completed", OnLevelCompleted);
	}
}
