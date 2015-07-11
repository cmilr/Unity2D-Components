using UnityEngine;
using System;
using System.Collections;
using Matcha.Lib;
using Matcha.Game.Tweens;

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
	private AttackAI attackAI;
	private MovementAI movementAI;

	void Start()
	{
		attackAI = gameObject.GetComponent<AttackAI>();
		movementAI = gameObject.GetComponent<MovementAI>();

		if (entityType == EntityType.Enemy) { AutoAlign(); }
	}

	override public void OnWeaponCollisionEnter(Collider2D coll)
	{
		playerWeapon = coll.GetComponent<Weapon>();

		hitFrom = MLib.HorizSideThatWasHit(gameObject, coll);

		if (playerWeapon.weaponType == Weapon.WeaponType.Projectile)
		{
			TakesProjectileHit(playerWeapon, coll, hitFrom);
		}
	}

	void TakesProjectileHit(Weapon playerWeapon, Collider2D coll, int hitFrom)
	{
		hp -= (int)(playerWeapon.damage * DIFFICULTY_DAMAGE_MODIFIER);

		if (hp <= 0)
		{
			collider2D.enabled = false;
			attackAI.enabled   = false;
			movementAI.enabled = false;
			MTween.Fade(spriteRenderer, 0f, 0f, 2f);
			Invoke("DeactivateObject", 5f);
		}
	}

	void DeactivateObject()
	{
		gameObject.SetActive(false);
	}

	void OnDisable()
	{
	    CancelInvoke();
	}

	override public void OnBodyCollisionEnter(Collider2D coll) {}
	override public void OnBodyCollisionStay() {}
	override public void OnBodyCollisionExit() {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
