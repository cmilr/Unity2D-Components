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

	private int groundline;

	void Start()
	{
		InvokeRepeating("CheckIfAboveGround", 0f, 0.2F);
	}

	void OnEnable()
	{
		Messenger.AddListener<int>("set groundline", OnSetGroundline);
		Messenger.AddListener<bool>( "touching wall", OnTouchingWall);
		Messenger.AddListener<bool>( "riding fast platform", OnRidingFastPlatform);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>("set groundline", OnSetGroundline);
		Messenger.RemoveListener<bool>( "touching wall", OnTouchingWall);		
		Messenger.RemoveListener<bool>( "riding fast platform", OnRidingFastPlatform);
	}

	void OnTouchingWall(bool status)
	{
		TouchingWall = status;
	}

	void OnRidingFastPlatform(bool status)
	{
		RidingFastPlatform = status;
	}

	void OnSetGroundline(int ground)
	{
		groundline = ground;
	}

	void CheckIfAboveGround()
	{
		if (transform.position.y > groundline)
		{
			Messenger.Broadcast<bool>("player above ground", true);
			AboveGround = true;
		}
		else 
		{
			Messenger.Broadcast<bool>("player above ground", false);
			AboveGround = false;
		}
	}
}
