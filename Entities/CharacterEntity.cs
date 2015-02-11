using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;


public class CharacterEntity : CacheBehaviour
{
	public enum EntityType { none, player, enemy };
	public EntityType entityType;
	public bool disableIfOffScreen;
	public int hp;
	public int ac;
	public int damage;

	public bool AlreadyCollided { get; set; }

	void Start()
	{
		base.CacheComponents();

		if (entityType == EntityType.enemy)
			AutoAlign();
	}

	public void React()
	{
		AlreadyCollided = true;

		switch (entityType)
		{
		case EntityType.none:
			break;

		case EntityType.player:
			MTween.PickupPrize(gameObject);
			break;

		case EntityType.enemy:
			MTween.PickupWeapon(gameObject);
			break;
		}
	}

	void OnBecameInvisible()
	{
		if (disableIfOffScreen)
			gameObject.SetActive(false);
	}

	void OnBecameVisible()
	{
		if (disableIfOffScreen)
			gameObject.SetActive(true);
	}

	void AutoAlign()
	{
		float targetY = (float)(Math.Round(transform.position.y) - .124);
		transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
	}

	public void SelfDestruct(int inSeconds)
	{
		Destroy(gameObject, inSeconds);
	}
}
