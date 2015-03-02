using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(PlayerInit))]
[RequireComponent(typeof(PlayerDataSingleton))]


public class PlayerData : BaseBehaviour {

	// stats
	public int hp 		{ get; set; }
	public int ac 		{ get; set; }
	public int damage 	{ get; set; }

	void Awake()
	{
		hp = 100;
		ac = 10;
		damage = 5;
	}
}