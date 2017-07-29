using UnityEngine;
using UnityEngine.Assertions;

public class CreatureEntity : Entity
{
	public enum Type { Invalid, Enemy };
	public Type type;
	public int hp;
	public int ac;
	public int touchDamage;

	private Hit cachedHit;
	private bool dead;
	private bool blockedRight;
	private bool blockedLeft;
	private BoxCollider2D thisCollider;
	private Weapon playerWeapon;
	private BreakableManager breakableManager;
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

		cachedHit = new Hit();
	}

	void Start()
	{
		breakableManager = GetComponentInChildren<BreakableManager>();
		Assert.IsNotNull(breakableManager);

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

	public void TakesMeleeHit(Hit hit)
	{
		if (!dead)
		{
			hp -= (hit.weapon.damage);

			if (hit.horizontalSide == Side.Right && !blockedLeft)
			{

			}
			else if (hit.horizontalSide == Side.Left && !blockedRight)
			{

			}
			else
			{
				rigidbody2D.velocity = Vector2.zero;
			}

			if (hp <= 0)
			{
				EventKit.Broadcast("prize collected", worth);
				KillSelf(hit);
			}
		}
	}

	public void TakesProjectileHit(Hit hit)
	{
		if (!dead)
		{
			hp -= (hit.weapon.damage);

			if (hit.horizontalSide == Side.Right && !blockedLeft)
			{

			}
			else if (hit.horizontalSide == Side.Left && !blockedRight)
			{

			}
			else
			{
				rigidbody2D.velocity = Vector2.zero;
			}

			if (hp <= 0)
			{
				EventKit.Broadcast("prize collected", worth);
				KillSelf(hit);
			}
		}
	}

	override public void OnWeaponCollisionEnter(Collider2D coll)
	{
		playerWeapon = coll.GetComponentInParent<Weapon>() ?? coll.GetComponentInParent<ProjectileContainer>().weapon;

		if (playerWeapon.type == Weapon.Type.Hammer ||
			playerWeapon.type == Weapon.Type.Dagger ||
			playerWeapon.type == Weapon.Type.MagicProjectile)
		{
			cachedHit.Create(gameObject, coll);
			TakesProjectileHit(cachedHit);
		}
	}


	void KillSelf(Hit hit)
	{
		if (!dead)
		{
			// activate and kill breakable sprite.
			breakableManager.MakeActive();
			gameObject.SendEventDown("ExplodeCreature", hit);

			// fade solid sprite & deactivate gameObject.
			dead = true;
			rigidbody2D.isKinematic = true;
			collider2D.enabled = false;
			spriteRenderer.enabled = false;
			gameObject.SendEvent("CreatureDead");
			Invoke("DeactivateObject", 5f);
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
