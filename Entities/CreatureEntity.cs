using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;

[RequireComponent(typeof(BoxCollider2D))]


public class CreatureEntity : EntityBehaviour
{
	public enum EntityType { none, player, enemy };
	public EntityType entityType;

	public int hp;
	public int ac;
	public int damage;
	public int worth;

	void Start()
	{
		base.CacheComponents();

		if (entityType == EntityType.enemy) { AutoAlign(); }
	}

	public void ReactToCollision()
	{
		switch (entityType)
		{
		case EntityType.none:
			break;

		case EntityType.player:
			break;

		case EntityType.enemy:
			break;
		}
	}

	public void SetCollidedWithBody(bool status)
	{
		hasCollidedWithBody = status;
	}

	public bool HasCollidedWithBody()
	{
		return hasCollidedWithBody;
	}

	public void SetCollidedWithWeapon(bool status)
	{
		hasCollidedWithWeapon = status;
	}

	public bool HasCollidedWithWeapon()
	{
		return hasCollidedWithWeapon;
	}
}
