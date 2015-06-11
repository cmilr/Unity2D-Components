using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]


public class CreatureEntity : Entity
{
	public enum EntityType { Enemy };
	public EntityType entityType;
	public int hp;
	public int ac;
	public int damage;

	private BoxCollider2D thisCollider;

	void Start()
	{
		if (entityType == EntityType.Enemy) { AutoAlign(); }
	}

	override public void OnBodyCollisionEnter()
	{
		collidedWithBody = true;

		if (!game.LevelLoading && !player.Dead)
		{
			switch (entityType)
			{
				case EntityType.Enemy:
				break;
			}
		}
	}

	override public void OnBodyCollisionStay() {}

	override public void OnBodyCollisionExit() {}

	override public void OnWeaponCollisionEnter()
	{
		collidedWithWeapon = true;

		if (!game.LevelLoading && !player.Dead)
		{
			switch (entityType)
			{
				case EntityType.Enemy:
				break;
			}
		}
	}

	override public void OnWeaponCollisionStay() {}

	override public void OnWeaponCollisionExit() {}
}
