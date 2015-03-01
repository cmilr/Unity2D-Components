using UnityEngine;
using System.Collections;


public class PlayerManager : CacheBehaviour
{
	// private PlayerData pd;

	// player state
	public bool FacingRight 		{ get; private set; }
	public bool RidingFastPlatform 	{ get; private set; }                   
	public bool TouchingWall 		{ get; private set; }
	public bool Dead 				{ get; private set; }

	void Start()
	{
		// pd = GameObject.Find("_PlayerData").GetComponent<PlayerData>();
	}
}
