using UnityEngine;
using System;
using System.Collections;


public abstract class Entity : CacheBehaviour {

	public int worth;
	public bool alreadyCollided;

	protected int layer;
	protected bool collidedWithBody;
	protected bool collidedWithWeapon;
	protected bool onScreen;
	protected IGameStateReadOnly game;
	protected IPlayerStateReadOnly player;

	public abstract void OnBodyCollisionEnter();
	public abstract void OnBodyCollisionStay();
	public abstract void OnBodyCollisionExit();
	public abstract void OnWeaponCollisionEnter();
	public abstract void OnWeaponCollisionStay();
	public abstract void OnWeaponCollisionExit();


	void OnEnable()
	{
		game = GameObject.Find(GAME_STATE).GetComponent<IGameStateReadOnly>();
		player = GameObject.Find(PLAYER).GetComponent<IPlayerStateReadOnly>();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		// check for layer instead of name — it's much quicker
		layer = coll.gameObject.layer;

		if (layer == BODY_COLLIDER && !collidedWithBody)
			OnBodyCollisionEnter();

		if (layer == WEAPON_COLLIDER && !collidedWithWeapon)
			OnWeaponCollisionEnter();
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == BODY_COLLIDER)
			OnBodyCollisionStay();

		if (layer == WEAPON_COLLIDER)
			OnWeaponCollisionStay();
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == BODY_COLLIDER)
			OnBodyCollisionExit();

		if (layer == WEAPON_COLLIDER)
			OnWeaponCollisionExit();
	}

	protected void AutoAlign()
	{
		float targetY = (float)(Math.Round(transform.position.y) - ALIGN_ENTITY_TO);
		transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
	}

	protected void LifecycleOver()
	{
		gameObject.SetActive(false);
	}

	public void OnTweenCompleted()
	{
		LifecycleOver();
	}

	void OnBecameVisible()
	{
	    enabled = true;
	    onScreen = true;
	}

	void OnBecameInvisible()
	{
	    enabled = false;
	    onScreen = false;
	}
}
