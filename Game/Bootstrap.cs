using UnityEngine;

public class Bootstrap : BaseBehaviour
{
	void Awake()
	{
		//instantiate new _Data object
		if (FindObjectOfType(typeof(GameData)) == null)
		{
			GameObject instance = new GameObject();
			instance.name = "_Data";
			instance.AddComponent<GameData>();
			instance.AddComponent<LevelData>();
			instance.AddComponent<PlayerData>();
		}
	}
}


//instantiate new _PlayerData object
// if (FindObjectOfType(typeof(PlayerData)) == null)
// {
// 	GameObject instance = new GameObject();
// 	instance.name = "_PlayerData";
// 	instance.AddComponent<PlayerData>();
// 	playerData = (PlayerData)instance.GetComponent<PlayerData>();
// }
// else
// {
// 	playerData = (PlayerData)FindObjectOfType(typeof(PlayerData));
// }
