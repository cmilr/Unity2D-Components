using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;


public class CharacterEntity : CacheBehaviour
{
	public enum EntityType { none, player, enemy };
	public EntityType entityType;
	public bool rbodySleepOffScreen = true;
	public int hp;
	public int ac;
	public int damage;

	void Start()
	{
		base.CacheComponents();

		if (entityType == EntityType.enemy)
			AutoAlign();
	}

	public void React()
	{

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
		if (rbodySleepOffScreen)
			if (rigidbody2D)
				rigidbody2D.Sleep();
	}

	void OnBecameVisible()
	{
		if (rbodySleepOffScreen)
			if (rigidbody2D)
				rigidbody2D.WakeUp();
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
