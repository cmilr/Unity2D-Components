using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;

public class PlayerData : BaseBehaviour
{
	public string Character    { get; set; }
	public int HP              { get; set; }
	public int AC              { get; set; }
	public int XP              { get; set; }
	public int LVL             { get; set; }
	[HideInInspector]
	public GameObject equippedWeapon;
	[HideInInspector]
	public GameObject leftWeapon;
	[HideInInspector]
	public GameObject rightWeapon;

	//called whenever awake() is called
	void SingletonAwake()
	{
		HP             = 50;
		AC             = 10;
		XP             = 0;
		LVL            = 1;
		equippedWeapon = GameObject.Find("Player/Inventory/Equipped");
		leftWeapon     = GameObject.Find("Player/Inventory/Left");
		rightWeapon    = GameObject.Find("Player/Inventory/Right");
	}

	public void Save()
	{
		var bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.dat");
		var container = new PlayerDataContainer();

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
		if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
		{
			var bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat",FileMode.Open);
			var container = (PlayerDataContainer)bf.Deserialize(file);
			file.Close();

			Character = container.character;
			HP        = container.hp;
			AC        = container.ac;
			XP         = container.xp;
			LVL        = container.lvl;
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
		EventKit.Subscribe("wake singletons", SingletonAwake);
		EventKit.Subscribe<bool>("save player data", OnSavePlayerData);
		EventKit.Subscribe<bool>("load player data", OnLoadPlayerData);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe("wake singletons", SingletonAwake);
		EventKit.Unsubscribe<bool>("save player data", OnSavePlayerData);
		EventKit.Unsubscribe<bool>("load player data", OnLoadPlayerData);
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
