using UnityEngine;
using System.Collections;


public class PlayerManager : CacheBehaviour
{
	// private PlayerData pd;
	private PlayerState state;
	private int groundLine = -50;						// y coordinate where aboveground ends

	void Start()
	{
		base.CacheComponents();
		state = GetComponent<PlayerState>();
		// pd = GameObject.Find("_PlayerData").GetComponent<PlayerData>();
	}

	void LateUpdate()
	{
		CheckIfAboveGround();
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		Messenger.Broadcast<bool>("disable movement", true);
	}

	void OnTouchingWall(bool status)
	{
		state.TouchingWall = status;
	}

	void CheckIfAboveGround()
	{
		if (transform.position.y > groundLine)
			Messenger.Broadcast<bool>("player above ground", true);
		else 
			Messenger.Broadcast<bool>("player above ground", false);
	}

	void OnEnable()
	{
		Messenger.AddListener<bool>( "touching wall", OnTouchingWall);
		// Messenger.AddListener<bool>( "riding fast platform", OnRidingFastPlatform);
		Messenger.AddListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>( "touching wall", OnTouchingWall);		
		// Messenger.RemoveListener<bool>( "riding fast platform", OnRidingFastPlatform);
		Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
	}
}
