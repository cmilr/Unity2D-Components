using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]


public class CreatureEntity : Entity
{
	public enum EntityType { none, player, enemy };
	public EntityType entityType;
	public int hp;
	public int ac;
	public int damage;

	void Start()
	{
		if (entityType == EntityType.enemy) { AutoAlign(); }
	}

	public override void ReactToCollision()
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
}
