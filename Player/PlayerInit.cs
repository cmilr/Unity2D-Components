using UnityEngine;
using System.Collections;

public class PlayerInit : BaseBehaviour {

	private _PlayerData player;

	void Start () 
	{
		player = GameObject.Find("_PlayerData").GetComponent<_PlayerData>();
	}
}
