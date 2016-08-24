using UnityEngine;

public class GameManager : BaseBehaviour
{
	public bool disableAttack;
	private GameData gameData;

	private _GameData gameData;

	void Awake()
	{
		MDebug.attackDisabled = disableAttack;
		EventKit.Broadcast("wake singletons");
	}

	void Start()
	{
<<<<<<< HEAD
		gameData = GameObject.Find(_GAME_DATA).GetComponent<_GameData>();
		EventKit.Broadcast<int>("init score", gameData.CurrentScore);
=======
		gameData = GameObject.Find(_DATA).GetComponent<GameData>();
		EventKit.Broadcast<int>("init score", gameData.CurrentScore);
		EventKit.Broadcast<int>("set difficulty", NORMAL);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
	}

	void OnPrizeCollected(int worth)
	{
		gameData.CurrentScore += worth;
		EventKit.Broadcast<int>("change score", gameData.CurrentScore);
	}

	void OnPlayerDead(int hitFrom, Weapon.WeaponType weaponType)
	{
		gameData.CurrentScore = gameData.LastSavedScore;
		gameData.Lives -= 1;
		EventKit.Broadcast<bool>("fade hud", true);
		EventKit.Broadcast<int>("load level", gameData.CurrentLevel);
	}

	void OnLevelCompleted(bool status)
	{
		gameData.LastSavedScore = gameData.CurrentScore;
		gameData.CurrentLevel = gameData.CurrentLevel;
		EventKit.Broadcast<bool>("fade hud", true);
		EventKit.Broadcast<int>("load level", gameData.CurrentLevel);
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("prize collected", OnPrizeCollected);
<<<<<<< HEAD
		EventKit.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
=======
		EventKit.Subscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
		EventKit.Subscribe<bool>("level completed", OnLevelCompleted);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("prize collected", OnPrizeCollected);
<<<<<<< HEAD
		EventKit.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
=======
		EventKit.Unsubscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
		EventKit.Unsubscribe<bool>("level completed", OnLevelCompleted);
	}
}
