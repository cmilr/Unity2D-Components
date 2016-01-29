using UnityEngine;

public class Bootstrap : BaseBehaviour
{
	PlayerData playerData;

	void Awake()
	{
		if (FindObjectOfType(typeof(PlayerData)) == null)
		{
			GameObject newPD = new GameObject();
			newPD.name = "_PlayerData";
			newPD.AddComponent<PlayerData>();
			playerData = (PlayerData)newPD.GetComponent("PlayerData");
		}
		else
		{
			playerData = (PlayerData)FindObjectOfType(typeof(PlayerData));
		}
	}
}
