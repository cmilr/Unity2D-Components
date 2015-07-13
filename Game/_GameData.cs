using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[RequireComponent(typeof(GameInit))]


public class _GameData : BaseBehaviour {

	public _GameData data;

	// game stats
	public float DifficultyMultiplier 	{ get; set; }

	// player stats
	public int CurrentScore 			{ get; set; }
	public int LastSavedScore 			{ get; set; }
	public int Lives 					{ get; set; }
	public int CurrentLevel 			{ get; set; }

	void Awake()
	{
		MakePseudoSingleton();

		DifficultyMultiplier = 1.0f;
		CurrentScore         = 0;
		LastSavedScore       = 0;
		Lives                = 3;
		CurrentLevel         = 1;
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/GameData.dat");

		GameDataContainer container = new GameDataContainer();

		container.difficultyMultiplier = DifficultyMultiplier;
		container.currentScore         = CurrentScore;
		container.lastSavedScore       = LastSavedScore;
		container.lives                = Lives;
		container.currentLevel         = CurrentLevel;

		bf.Serialize(file, container);
		file.Close();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/GameData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
			GameDataContainer container = (GameDataContainer)bf.Deserialize(file);
			file.Close();

			DifficultyMultiplier = container.difficultyMultiplier;
			CurrentScore         = container.currentScore;
			LastSavedScore       = container.lastSavedScore;
			Lives                = container.lives;
			CurrentLevel         = container.currentLevel;
		}
	}

	void MakePseudoSingleton()
	{
		if (data == null)
		{
			DontDestroyOnLoad(gameObject);
			data = this;
		}
		else if (data != this)
		{
			Destroy(gameObject);
		}
	}

	void OnSaveGameData(bool status)
	{
		Save();
	}

	void OnLoadGameData(bool status)
	{
		Load();
	}

	void OnEnable()
	{
		Messenger.AddListener<bool>("save game data", OnSaveGameData);
		Messenger.AddListener<bool>("load game data", OnLoadGameData);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>("save game data", OnSaveGameData);
		Messenger.RemoveListener<bool>("load game data", OnLoadGameData);
	}
}

[Serializable]
class GameDataContainer
{
	public float difficultyMultiplier;

	public int currentScore;
	public int lastSavedScore;
	public int lives;
	public int currentLevel;
}