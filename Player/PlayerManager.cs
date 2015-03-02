using UnityEngine;
using System.Collections;


public class PlayerManager : CacheBehaviour
{
	private _PlayerData data;
	private PlayerState state;

	void Start()
	{
		state = GetComponent<PlayerState>();
		data = GameObject.Find("_PlayerData").GetComponent<_PlayerData>();
	}

	void OnHasDied(string methodOfDeath, Collider2D coll)
	{
		Messenger.Broadcast<bool>("disable movement", true);
		Messenger.Broadcast<bool>("enable death handler", true);
		Messenger.Broadcast<string, Collider2D>("handle death", methodOfDeath, coll);
		
		if (!state.Dead)
			Messenger.Broadcast<bool>("player dead", true);

		state.Dead = true;
	}

	void OnEnable()
	{
		Messenger.AddListener<string, Collider2D>( "has died", OnHasDied);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, Collider2D>( "has died", OnHasDied);
	}
}
