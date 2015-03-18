using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


// this is a pseudo-singleton — it enforces a single instance, but doesn't expose
// a static variable, so you can't access it without a GetComponent() call
public class _LevelData : BaseBehaviour {

	public _LevelData data;

	public int HP 		{ get; set; }
	public int AC 		{ get; set; }
	public int Damage	{ get; set; }

	void Awake()
	{
		MakePseudoSingleton();
	}

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
		if(File.Exists(Application.persistentDataPath + "/LevelData.dat"))
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
		Messenger.AddListener<bool>("save level data", OnSaveLevelData);
		Messenger.AddListener<bool>("load level data", OnLoadLevelData);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>("save level data", OnSaveLevelData);
		Messenger.RemoveListener<bool>("load level data", OnLoadLevelData);
	}
}

[Serializable]
class LevelDataContainer
{
	public int hp;
	public int ac;
	public int damage;
}