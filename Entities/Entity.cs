using UnityEngine;
using System;
using System.Collections;


public abstract class Entity : CacheBehaviour {

	public int worth;

	protected bool collidedWithBody;
	protected bool collidedWithWeapon;
	protected bool sceneLoading;
	protected bool playerDead;
	protected IPlayerStateReadOnly player;

	public abstract void OnBodyCollisionEnter();
	public abstract void OnBodyCollisionStay();
	public abstract void OnBodyCollisionExit();
	public abstract void OnWeaponCollisionEnter();
	public abstract void OnWeaponCollisionStay();
	public abstract void OnWeaponCollisionExit();

	public void Start()
	{
		player = GameObject.Find("Player").GetComponent<IPlayerStateReadOnly>();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.name == "BodyCollider" && !collidedWithBody)
			OnBodyCollisionEnter();

		if (coll.name == "WeaponCollider" && !collidedWithWeapon)
			OnWeaponCollisionEnter();
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.name == "BodyCollider")
			OnBodyCollisionStay();

		if (coll.name == "WeaponCollider")
			OnWeaponCollisionStay();
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.name == "BodyCollider")
			OnBodyCollisionExit();

		if (coll.name == "WeaponCollider")
			OnWeaponCollisionExit();
	}

	protected void SelfDestruct(int inSeconds)
	{
		Destroy(gameObject, inSeconds);
	}

	protected void AutoAlign()
	{
		float targetY = (float)(Math.Round(transform.position.y) - ALIGN_ENTITY_TO);
		transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
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
