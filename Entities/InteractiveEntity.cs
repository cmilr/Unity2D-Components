using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;


public class InteractiveEntity : CacheBehaviour
{
	public enum EntityType { none, prize, weapon };
	public EntityType entityType;
	public bool disableIfOffScreen = true;
	public int worth;
	
	public bool AlreadyCollided { get; set; }

	void Start()
	{
		base.CacheComponents();

		if (entityType == EntityType.prize)
		{
			AutoAlign();
		}
	}

	public void React()
	{
		AlreadyCollided = true;

		switch (entityType)
		{
		case EntityType.none:
			break;

		case EntityType.prize:
			MTween.PickupPrize(gameObject);
			break;

		case EntityType.weapon:
			MTween.PickupWeapon(gameObject);
			break;
		}
	}

	void OnBecameInvisible()
	{
		if (disableIfOffScreen)
		{
			gameObject.SetActive(false);
		}
	}

	void OnBecameVisible()
	{
		if (disableIfOffScreen)
		{
			gameObject.SetActive(true);
		}
	}

	void AutoAlign()
	{
		float targetY = (float)(Math.Ceiling(transform.position.y) - .623f);
		transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
	}

	public void SelfDestruct(int inSeconds)
	{
		Destroy(gameObject, inSeconds);
	}
}
