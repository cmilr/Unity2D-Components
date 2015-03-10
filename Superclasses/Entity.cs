using UnityEngine;
using System;
using System.Collections;


public abstract class Entity : CacheBehaviour {

	protected bool alreadyCollidedWithBody;
	protected bool alreadyCollidedWithWeapon;
	protected bool sceneLoading;
	protected bool playerDead;
	public int worth;

	public abstract void OnBodyCollisionEnter();

	public abstract void OnBodyCollisionStay();

	public abstract void OnBodyCollisionExit();

	protected void SelfDestruct(int inSeconds)
	{
		Destroy(gameObject, inSeconds);
	}

	protected void AutoAlign()
	{
		float targetY = (float)(Math.Round(transform.position.y) - ALIGN_ENTITY_TO);
		transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.name == "BodyCollider" && !alreadyCollidedWithBody)
		{
			alreadyCollidedWithBody = true;
			OnBodyCollisionEnter();
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.name == "BodyCollider")
			OnBodyCollisionStay();
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.name == "BodyCollider")
		{
			alreadyCollidedWithBody = false;
			OnBodyCollisionExit();
		}
	}

	void OnLoadLevel(int unused)
	{
		sceneLoading = true;
	}

	void OnPlayerDead(string unused, Collider2D alsoUnused)
	{
		playerDead = true;
	}

	void OnEnable()
	{
		Messenger.AddListener<int>( "load level", OnLoadLevel);	
		Messenger.AddListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>( "load level", OnLoadLevel);	
		Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
	}

	// protected void OnBecameInvisible()
	// {
	// 	if(rigidbody2D)
	// 		rigidbody2D.Sleep();
	// }

	// protected void OnBecameVisible()
	// {
	// 	if(rigidbody2D)
	// 		rigidbody2D.WakeUp();
	// }
}
