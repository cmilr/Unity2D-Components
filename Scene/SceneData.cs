using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(SceneDataSingleton))]


public class SceneData : BaseBehaviour {

	// scene stats
	// public float DifficultyMultiplier 	{ get; set; }

	// player stats
	// public int CurrentScore 			{ get; set; }
	// public int LastSavedScore 			{ get; set; }
	// public int Lives 					{ get; set; }
	// public int CurrentLevel 			{ get; set; }

	void Awake()
	{
		// DifficultyMultiplier = 1.0f;
		// CurrentScore = 0;
		// LastSavedScore = 0;
		// Lives = 3;		
		// CurrentLevel = 1;
	}
}
