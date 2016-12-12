using UnityEngine;

public class SmartCollider : BaseBehaviour
{
	private int layer;
	private bool collidedWithBody;
	private bool collidedWithWeapon;
	private bool playerDead;
	private bool levelCompleted;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (!playerDead && !levelCompleted)
		{
			layer = coll.gameObject.layer;

			if (layer == PLAYER_BODY_COLLIDER && !collidedWithBody)
			{
				gameObject.SendMessage("OnPlayerBodyCollEnter", coll);
				collidedWithBody = true;
			}

			if (layer == PLAYER_WEAPON_COLLIDER && !collidedWithWeapon)
			{
				gameObject.SendMessage("OnPlayerWeaponCollEnter", coll);
				collidedWithWeapon = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (!playerDead && !levelCompleted)
		{
			layer = coll.gameObject.layer;

			if (layer == PLAYER_BODY_COLLIDER)
			{
				gameObject.SendMessage("OnPlayerBodyCollExit");
				collidedWithBody = false;
			}

			if (layer == PLAYER_WEAPON_COLLIDER)
			{
				gameObject.SendMessage("OnPlayerWeaponCollExit");
				collidedWithWeapon = false;
			}
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("level completed", OnLevelCompleted);
		EventKit.Subscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnDisable()
	{
		EventKit.Unsubscribe<bool>("level completed", OnLevelCompleted);
		EventKit.Unsubscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnPlayerDead(Hit incomingHit)
	{
		playerDead = true;
	}

	void OnLevelCompleted(bool status)
	{
		levelCompleted = status;
	}
}
