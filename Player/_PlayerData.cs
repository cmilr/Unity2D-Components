using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class _PlayerData : MonoBehaviour {

	public static _PlayerData data;

	public int HP 		{ get; set; }
	public int AC 		{ get; set; }
	public int Damage	{ get; set; }

	void Awake()
	{
		MakeSingleton();
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.dat");

		DataContainer container = new DataContainer();
		container.hp = HP;
		container.ac = AC;
		container.damage = Damage;

		bf.Serialize(file, container);
		file.Close();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat",FileMode.Open);
			DataContainer container = (DataContainer)bf.Deserialize(file);
			file.Close();

			HP = container.hp;
			AC = container.ac;
			Damage = container.damage;
		}
	}

	void MakeSingleton()
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
} 

[Serializable]
class DataContainer
{
	public int hp;
	public int ac;
	public int damage;		
}