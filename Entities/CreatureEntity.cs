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
	public int touchDamage;

	private BoxCollider2D thisCollider;

	void Start()
	{
		if (entityType == EntityType.Enemy) { AutoAlign(); }
	}

	override public void OnBodyCollisionEnter(Collider2D coll)
	{
		// collidedWithBody = true;

		// if (!game.LevelLoading && !player.Dead)
		// {
		// 	switch (entityType)
		// 	{
		// 		case EntityType.Enemy:
		// 		break;
		// 	}
		// }
	}

	override public void OnWeaponCollisionEnter(Collider2D coll)
	{
		collidedWithWeapon = true;
		// check for layer instead of name — it's much quicker
		  layer = coll.gameObject.layer;

		  if (layer == WEAPON_COLLIDER)
		  {
		      enemyWeapon = coll.GetComponent<Weapon>();

		      if (!enemyWeapon.alreadyCollided && !game.LevelLoading && !state.Dead)
		      {
		          hitFrom = MLib.HorizSideThatWasHit(gameObject, coll);

		          if (enemyWeapon.weaponType == Weapon.WeaponType.Projectile)
		          {
		              enemyWeapon.alreadyCollided = true;

		              player.TakesHit("projectile", enemyWeapon, coll, hitFrom);
		          }
		      }
		  }
		  else if (layer == ENEMY_COLLIDER)
		  {
		      enemy = coll.GetComponent<CreatureEntity>();

		      if (!enemy.alreadyCollided && !game.LevelLoading && !state.Dead)
		      {
		          hitFrom = MLib.HorizSideThatWasHit(gameObject, coll);

		          if (enemy.entityType == CreatureEntity.EntityType.Enemy)
		          {
		              enemy.alreadyCollided = true;

		              player.TouchesEnemy("touch", enemy, coll, hitFrom);
		          }
		      }
		  }
	}

	override public void OnBodyCollisionStay() {}

	override public void OnBodyCollisionExit()
	{
		collidedWithWeapon = false;
	}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
