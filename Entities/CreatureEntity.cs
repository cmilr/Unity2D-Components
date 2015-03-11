using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]


public class CreatureEntity : Entity
{
	public enum EntityType { enemy };
	public EntityType entityType;
	public int hp;
	public int ac;
	public int damage;

	private BoxCollider2D thisCollider;

	void Start()
	{
		thisCollider = GetComponent<BoxCollider2D>();

		if (entityType == EntityType.enemy) { AutoAlign(); }
	}

	override public void OnBodyCollisionEnter()
	{
		collidedWithBody = true;

		if (!sceneLoading && !playerDead)
		{
			switch (entityType)
			{
				case EntityType.enemy:
					Messenger.Broadcast<string, Collider2D>("player dead", "struckdown", thisCollider);
				break;
			}
		}
	}

	override public void OnBodyCollisionStay() {}

	override public void OnBodyCollisionExit() {}

	override public void OnWeaponCollisionEnter()
	{
		collidedWithWeapon = true;

		if (!sceneLoading && !playerDead)
		{
			switch (entityType)
			{
				case EntityType.enemy:
				break;
			}
		}
	}

	override public void OnWeaponCollisionStay() {}

	override public void OnWeaponCollisionExit() {}
}
