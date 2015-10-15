using UnityEngine;
using System;
using System.Collections;
using Matcha.Extensions;


public abstract class Entity : CacheBehaviour {

	public int worth;
	public bool autoAlign = true;
	[HideInInspector]
	public bool alreadyCollided;

	protected int layer;
	protected int hitFrom;
	protected bool collidedWithBody;
	protected bool collidedWithWeapon;
	protected bool onScreen;
	protected IGameStateReadOnly game;
	protected IPlayerStateReadOnly player;

	public abstract void OnBodyCollisionEnter(Collider2D coll);
	public abstract void OnBodyCollisionStay();
	public abstract void OnBodyCollisionExit();
	public abstract void OnWeaponCollisionEnter(Collider2D coll);
	public abstract void OnWeaponCollisionStay();
	public abstract void OnWeaponCollisionExit();


	void OnEnable()
	{
		game = GameObject.Find(GAME_STATE).GetComponent<IGameStateReadOnly>();
		player = GameObject.Find(PLAYER).GetComponent<IPlayerStateReadOnly>();
	}

	protected void AutoAlign()
	{
		float targetY = (float)(Math.Round(transform.position.y) - ALIGN_ENTITY_TO);
		transform.SetY(targetY);
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

	void OnTriggerEnter2D(Collider2D coll)
	{
		// check for layer instead of name — it's much quicker
		layer = coll.gameObject.layer;

		if (layer == BODY_COLLIDER && !collidedWithBody)
		{
			OnBodyCollisionEnter(coll);
			collidedWithBody = true;
		}

		if (layer == WEAPON_COLLIDER)
		{
			// doesn't check for previous collision because player weapon colliders
			// are automatically turned offer after each successful hit
			OnWeaponCollisionEnter(coll);
		}
	}

	// void OnTriggerStay2D(Collider2D coll)
	// {
	// 	layer = coll.gameObject.layer;

	// 	if (layer == BODY_COLLIDER)
	// 		OnBodyCollisionStay();

	// 	if (layer == WEAPON_COLLIDER)
	// 		OnWeaponCollisionStay();
	// }

	void OnTriggerExit2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == BODY_COLLIDER)
		{
			OnBodyCollisionExit();
			collidedWithBody = false;
		}

		if (layer == WEAPON_COLLIDER)
		{
			OnWeaponCollisionExit();
			collidedWithWeapon = false;
		}
	}
}
