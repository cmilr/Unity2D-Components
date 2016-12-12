using UnityEngine;

public class Bootstrap : BaseBehaviour
{
	void Awake()
	{
		// instantiate new _Data object.
		if (FindObjectOfType(typeof(GameData)) == null)
		{
			var instance = new GameObject();
			instance.name = "_Data";
			instance.AddComponent<GameData>();
			instance.AddComponent<LevelData>();
			instance.AddComponent<PlayerData>();
			DontDestroyOnLoad(instance);
		}

		// instantiate new _HUD.
		//if (FindObjectOfType(typeof(DisplayEquipped)) == null)
		//{
		//	var hud = (GameObject)Instantiate(Resources.Load("Prefabs/Misc/HUD"));
		//	hud.transform.parent = Camera.main.transform;
		//	Debug.Log("HUD Built");
		//}
	}
}
