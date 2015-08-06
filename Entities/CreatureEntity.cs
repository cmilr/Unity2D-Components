using UnityEngine;
using System;
using System.Collections;
using Matcha.Lib;
using Matcha.Dreadful.FX;
using Matcha.Dreadful.Colors;
using DG.Tweening;

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
	private bool blockedRight;
	private bool blockedLeft;

	void Start()
	{
		attackAI = gameObject.GetComponent<AttackAI>();
		movementAI = gameObject.GetComponent<MovementAI>();

		if (entityType == EntityType.Enemy) { AutoAlign(); }
	}

	private void SetBlockedRightState(bool status)
	{
	    blockedRight = status;
	}

	private void SetBlockedLeftState(bool status)
	{
	    blockedLeft = status;
	}

	override public void OnWeaponCollisionEnter(Collider2D coll)
	{
		playerWeapon = coll.GetComponent<Weapon>();

		hitFrom = MLib.HorizSideThatWasHit(gameObject, coll);

		if (playerWeapon.weaponType == Weapon.WeaponType.Hammer ||
			playerWeapon.weaponType == Weapon.WeaponType.Dagger ||
			playerWeapon.weaponType == Weapon.WeaponType.MagicProjectile)
		{
			TakesProjectileHit(playerWeapon, coll, hitFrom);
		}
	}

	void TakesProjectileHit(Weapon playerWeapon, Collider2D coll, int hitFrom)
	{
		hp -= (int)(playerWeapon.damage * DIFFICULTY_DAMAGE_MODIFIER);

		// bounceback from projectile
		if (hitFrom == RIGHT && !blockedLeft)
		{
			MFX.RepulseToLeftRandomly(transform, .3f, .8f, .2f);
		}
		else if (hitFrom == LEFT && !blockedRight)
		{
			MFX.RepulseToRightRandomly(transform, .3f, .8f, .2f);
		}
		else
		{
			rigidbody2D.velocity = Vector2.zero;
		}

		if (hp <= 0)
		{
			Messenger.Broadcast<int>("prize collected", worth);
			KillSelf();
		}
	}

	void KillSelf()
	{
		rigidbody2D.velocity = Vector2.zero;
		collider2D.enabled   = false;
		attackAI.enabled     = false;
		movementAI.enabled   = false;
		MFX.FadeToColor(spriteRenderer, MLib.HexToColor("5c0c01"), 0f, .75f);
		MFX.Fade(spriteRenderer, 0f, 0f, 1.5f);
		Invoke("DeactivateObject", 2f);
	}

	void DeactivateObject()
	{
		gameObject.SetActive(false);
	}

	override public void OnBodyCollisionEnter(Collider2D coll) {}
	override public void OnBodyCollisionStay() {}
	override public void OnBodyCollisionExit() {}
	override public void OnWeaponCollisionStay() {}
	override public void OnWeaponCollisionExit() {}
}
