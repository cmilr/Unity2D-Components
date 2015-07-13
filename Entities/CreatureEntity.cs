using UnityEngine;
using System;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class CreatureEntity : Entity
{
	public enum EntityType { Enemy };
	public EntityType entityType;
	public int hp;
	public int ac;
	public int touchDamage;

	private BoxCollider2D thisCollider;
	private Weapon playerWeapon;

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
		      playerWeapon = coll.GetComponent<Weapon>();

		      if (!playerWeapon.alreadyCollided)
		      {
		          hitFrom = MLib.HorizSideThatWasHit(gameObject, coll);

		          if (playerWeapon.weaponType == Weapon.WeaponType.Projectile)
		          {
		              playerWeapon.alreadyCollided = true;

		              TakesHit(playerWeapon, coll, hitFrom);
		          }
		      }
		  }
		  else if (layer == ENEMY_COLLIDER)
		  {
		      // enemy = coll.GetComponent<CreatureEntity>();

		      // if (!enemy.alreadyCollided && !game.LevelLoading && !state.Dead)
		      // {
		      //     hitFrom = MLib.HorizSideThatWasHit(gameObject, coll);

		      //     if (enemy.entityType == CreatureEntity.EntityType.Enemy)
		      //     {
		      //         enemy.alreadyCollided = true;

		      //         player.TouchesEnemy("touch", enemy, coll, hitFrom);
		      //     }
		      // }
		  }
	}

	void TakesHit(Weapon playerWeapon, Collider2D coll, int hitFrom)
	{
		hp -= (int)(playerWeapon.damage * DIFFICULTY_DAMAGE_MODIFIER);

		if (hp <= 0)
			Dbug();
	}

	override public void OnBodyCollisionStay() {}

	override public void OnBodyCollisionExit()
	{
		collidedWithWeapon = false;
		playerWeapon.alreadyCollided = false;
	}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
