using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


// this is a pseudo-singleton — it enforces a single instance, but doesn't expose
// a static variable, so you can't access it without a GetComponent() call
public class _PlayerData : BaseBehaviour {

	public _PlayerData data;

	public string Character 	{ get; set; }
	public int HP 				{ get; set; }
	public int AC 				{ get; set; }
	public int XP				{ get; set; }
	public int LVL				{ get; set; }
	public GameObject sword;

	void Awake()
	{
		MakePseudoSingleton();

		// initialize settings
		Character = "LAURA";
		HP        = 25;
		AC        = 4;
		XP        = 0;
		LVL       = 1;
		sword = GameObject.Find("Player/Sword");


	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.dat");

		PlayerDataContainer container = new PlayerDataContainer();

		container.character = Character;
		container.hp        = HP;
		container.ac        = AC;
		container.xp        = XP;
		container.lvl       = LVL;

		bf.Serialize(file, container);
		file.Close();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat",FileMode.Open);
			PlayerDataContainer container = (PlayerDataContainer)bf.Deserialize(file);
			file.Close();

			Character = container.character;
			HP        = container.hp;
			AC        = container.ac;
			XP    	  = container.xp;
			LVL    	  = container.lvl;
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

	void OnSavePlayerData(bool status)
	{
		Save();
	}

	void OnLoadPlayerData(bool status)
	{
		Load();
	}

	void OnEnable()
	{
		Messenger.AddListener<bool>("save player data", OnSavePlayerData);
		Messenger.AddListener<bool>("load player data", OnLoadPlayerData);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>("save player data", OnSavePlayerData);
		Messenger.RemoveListener<bool>("load player data", OnLoadPlayerData);
	}
}

[Serializable]
class PlayerDataContainer
{
	public string character;
	public int hp;
	public int ac;
	public int xp;
	public int lvl;
}