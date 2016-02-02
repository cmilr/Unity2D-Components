using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;

public class LevelData : BaseBehaviour
{
	public int HP     { get; set; }
	public int AC     { get; set; }
	public int Damage { get; set; }

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/LevelData.dat");
		LevelDataContainer container = new LevelDataContainer();

		container.hp     = HP;
		container.ac     = AC;
		container.damage = Damage;

		bf.Serialize(file, container);
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/LevelData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/LevelData.dat",FileMode.Open);
			LevelDataContainer container = (LevelDataContainer)bf.Deserialize(file);
			file.Close();

			HP     = container.hp;
			AC     = container.ac;
			Damage = container.damage;
		}
	}

	void OnSaveLevelData(bool status)
	{
		Save();
	}

	void OnLoadLevelData(bool status)
	{
		Load();
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("save level data", OnSaveLevelData);
		EventKit.Subscribe<bool>("load level data", OnLoadLevelData);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<bool>("save level data", OnSaveLevelData);
		EventKit.Unsubscribe<bool>("load level data", OnLoadLevelData);
	}
}

[Serializable]
class LevelDataContainer
{
	public int hp;
	public int ac;
	public int damage;
}
