using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class CreatureEntity : Entity
{
	//TODO: convert collisions to Hits.
	public enum Type { Invalid, Enemy };
	public Type type;
	public int hp;
	public int ac;
	public int touchDamage;

	private bool dead;
	private bool blockedRight;
	private bool blockedLeft;
	private BoxCollider2D thisCollider;
	private Weapon playerWeapon;
	private BreakableManager breakable;
	private new Transform transform;
	private new Collider2D collider2D;
	private new Rigidbody2D rigidbody2D;
	private SpriteRenderer spriteRenderer;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);

		collider2D = GetComponent<Collider2D>();
		Assert.IsNotNull(collider2D);

		rigidbody2D = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(rigidbody2D);

		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);

		Assert.IsFalse(type == Type.Invalid,
   			("Invalid creature type @ " + gameObject));
	}

	void Start()
	{
		breakable = gameObject.GetComponentInChildren<BreakableManager>();
		Assert.IsNotNull(breakable);

		if (type == Type.Enemy)
			AutoAlign();
	}

	private void SetBlockedRightState(bool status)
	{
		blockedRight = status;
	}

	private void SetBlockedLeftState(bool status)
	{
		blockedLeft = status;
	}

	public void TakesMeleeHit(Weapon incomingWeapon, Collider2D coll)
	{
		if (!dead)
		{
			var hitSide = M.HorizontalSideHit(gameObject, coll);

			hp -= (incomingWeapon.damage);

			if (hitSide == RIGHT && !blockedLeft)
			{

			}
			else if (hitSide == LEFT && !blockedRight)
			{

			}
			else 
			{
				rigidbody2D.velocity = Vector2.zero;
			}

			if (hp <= 0)
			{
				EventKit.Broadcast("prize collected", worth);
				KillSelf(hitSide, MELEE);
			}
		}
	}

	public void TakesProjectileHit(Weapon incomingWeapon, Collider2D coll)
	{
		if (!dead)
		{
			var hitSide = M.HorizontalSideHit(gameObject, coll);

			hp -= (incomingWeapon.damage);

			if (hitSide == RIGHT && !blockedLeft)
			{

			}
			else if (hitSide == LEFT && !blockedRight)
			{

			}
			else
			{
				rigidbody2D.velocity = Vector2.zero;
			}

			if (hp <= 0)
			{
				EventKit.Broadcast("prize collected", worth);
				KillSelf(hitSide, PROJECTILE);
			}
		}
	}

	override public void OnWeaponCollisionEnter(Collider2D coll)
	{
		playerWeapon = coll.GetComponentInParent<Weapon>();

		if (playerWeapon.type == Weapon.Type.Hammer ||
			playerWeapon.type == Weapon.Type.Dagger ||
			playerWeapon.type == Weapon.Type.MagicProjectile) 
		{
			TakesProjectileHit(playerWeapon, coll);
		}
	}


	void KillSelf(int hitSide, int weaponType)
	{
		if (!dead)
		{
			// activate and kill breakable sprite
			if (weaponType == MELEE)
			{
				breakable.DirectionalSlump(hitSide);
			}
			else if (weaponType == PROJECTILE)
			{
				breakable.Explode(hitSide);
			}

			// deactivate and fade solid sprite
			rigidbody2D.isKinematic = true;
			collider2D.enabled = false;

			spriteRenderer.enabled = false;

			dead = true;

			gameObject.SendMessage("CreatureDead");

			Invoke("DeactivateObject", MAX_BEFORE_FADE + 5f);
		}
	}

	void DeactivateObject()
	{
		gameObject.SetActive(false);
	}

	override public void OnBodyCollisionEnter(Collider2D coll) { }
	override public void OnBodyCollisionStay() { }
	override public void OnBodyCollisionExit() { }
	override public void OnWeaponCollisionStay() { }
	override public void OnWeaponCollisionExit() { }
}
