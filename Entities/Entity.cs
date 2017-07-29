using System;
using UnityEngine;

public abstract class Entity : BaseBehaviour
{
	public abstract void OnBodyCollisionEnter(Collider2D coll);
	public abstract void OnBodyCollisionStay();
	public abstract void OnBodyCollisionExit();
	public abstract void OnWeaponCollisionEnter(Collider2D coll);
	public abstract void OnWeaponCollisionStay();
	public abstract void OnWeaponCollisionExit();

	[HideInInspector]
	public bool alreadyCollided;
	public int worth;
	public bool autoAlign = true;

	protected const float ALIGN_ENTITY_TO = .124f;

	protected int layer;
	protected int hitFrom;
	protected bool collidedWithBody;
	protected bool collidedWithWeapon;
	protected bool onScreen;
	protected bool playerDead;
	protected bool levelCompleted;

	protected void AutoAlign()
	{
		var targetY = (float)(Math.Round(transform.position.y) - ALIGN_ENTITY_TO);
		transform.SetPositionY(targetY);
	}

	public void OnTweenCompleted()
	{
		gameObject.SetActive(false);
	}

	void OnBecameVisible()
	{
		//enabled = true;
		onScreen = true;
	}

	void OnBecameInvisible()
	{
		//enabled = false;
		onScreen = false;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == PLAYER_BODY_COLLIDER && !collidedWithBody)
		{
			OnBodyCollisionEnter(coll);
			collidedWithBody = true;
		}

		if (layer == PLAYER_WEAPON_COLLIDER)
		{
			OnWeaponCollisionEnter(coll);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == PLAYER_BODY_COLLIDER)
		{
			OnBodyCollisionExit();
			collidedWithBody = false;
		}

		if (layer == PLAYER_WEAPON_COLLIDER)
		{
			OnWeaponCollisionExit();
			collidedWithWeapon = false;
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("level completed", OnLevelCompleted);
		EventKit.Subscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnDisable()
	{
		EventKit.Unsubscribe<bool>("level completed", OnLevelCompleted);
		EventKit.Unsubscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnPlayerDead(Hit incomginHit)
	{
		playerDead = true;
	}

	void OnLevelCompleted(bool status)
	{
		levelCompleted = status;
	}
}
