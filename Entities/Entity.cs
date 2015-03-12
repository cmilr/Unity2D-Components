using UnityEngine;
using System;
using System.Collections;


public abstract class Entity : CacheBehaviour {

	public int worth;

	protected bool collidedWithBody;
	protected bool collidedWithWeapon;
	protected IGameStateReadOnly game;
	protected IPlayerStateReadOnly player;

	public abstract void OnBodyCollisionEnter();
	public abstract void OnBodyCollisionStay();
	public abstract void OnBodyCollisionExit();
	public abstract void OnWeaponCollisionEnter();
	public abstract void OnWeaponCollisionStay();
	public abstract void OnWeaponCollisionExit();


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

	void OnEnable()
	{
		game = GameObject.Find("GameState").GetComponent<IGameStateReadOnly>();
		player = GameObject.Find("Player").GetComponent<IPlayerStateReadOnly>();
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
