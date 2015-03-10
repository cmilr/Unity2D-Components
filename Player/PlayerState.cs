using UnityEngine;
using System.Collections;


// a clearinghouse for temporary player state
public class PlayerState : BaseBehaviour {

	// player state
	public bool FacingRight 		{ get; set; }
	public bool RidingFastPlatform 	{ get; set; }                   
	public bool TouchingWall 		{ get; set; }
	public bool Dead 				{ get; set; }
	public bool LevelCompleted 		{ get; set; }
	public bool AboveGround 		{ get; set; }

	public float GetY()
	{
		return transform.position.y;
	}

	void OnEnable()
	{
		Messenger.AddListener<bool>("touching wall", OnTouchingWall);
		Messenger.AddListener<bool>("riding fast platform", OnRidingFastPlatform);
		Messenger.AddListener<bool>("player above ground", OnPlayerAboveGround);
		Messenger.AddListener<string, Collider2D>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>("touching wall", OnTouchingWall);		
		Messenger.RemoveListener<bool>("riding fast platform", OnRidingFastPlatform);
		Messenger.RemoveListener<bool>("player above ground", OnPlayerAboveGround);
		Messenger.RemoveListener<string, Collider2D>("player dead", OnPlayerDead);
	}

	void OnTouchingWall(bool status)
	{
		TouchingWall = status;
	}

	void OnRidingFastPlatform(bool status)
	{
		RidingFastPlatform = status;
	}

	void OnPlayerAboveGround(bool status)
	{
		AboveGround = status;
	}

	void OnPlayerDead (string methodOfDeath, Collider2D coll)
	{
		Dead = true;
	}
}
