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

	void Start()
	{
		if (entityType == EntityType.enemy) { AutoAlign(); }
	}

	override public void OnBodyCollisionEnter()
	{
		if (!sceneLoading && !playerDead)
		{
			switch (entityType)
			{
				case EntityType.enemy:
					Messenger.Broadcast<string, Collider2D>("player dead", "struckdown", GetComponent<BoxCollider2D>());
				break;
			}
		}
	}

	override public void OnBodyCollisionStay() {}

	override public void OnBodyCollisionExit() {}

	override public void OnWeaponCollisionEnter()
	{
		if (!sceneLoading && !playerDead)
		{
			switch (entityType)
			{
				case EntityType.enemy:
					Debug.Log("Weapon collides with " + gameObject);
				break;
			}
		}
	}

	override public void OnWeaponCollisionStay() {}

	override public void OnWeaponCollisionExit() {}
}
