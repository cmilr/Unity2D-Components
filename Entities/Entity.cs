using Matcha.Unity;
using System;
using UnityEngine;

public abstract class Entity : CacheBehaviour
{
	[HideInInspector]
	public bool alreadyCollided;
	public int worth;
	public bool autoAlign = true;

	protected int layer;
	protected int hitFrom;
	protected bool collidedWithBody;
	protected bool collidedWithWeapon;
	protected bool onScreen;
	protected bool playerDead;
	protected IGameStateReadOnly game;

	public abstract void OnBodyCollisionEnter(Collider2D coll);
	public abstract void OnBodyCollisionStay();
	public abstract void OnBodyCollisionExit();
	public abstract void OnWeaponCollisionEnter(Collider2D coll);
	public abstract void OnWeaponCollisionStay();
	public abstract void OnWeaponCollisionExit();


	void OnEnable()
	{
		game   = GameObject.Find(GAME_STATE).GetComponent<IGameStateReadOnly>();
		Evnt.Subscribe<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnDisable()
	{
		Evnt.Unsubscribe<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	protected void AutoAlign()
	{
		float targetY = (float)(Math.Round(transform.position.y) - ALIGN_ENTITY_TO);
		transform.SetPositionY(targetY);
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

	void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
	{
		playerDead = true;
	}
}
