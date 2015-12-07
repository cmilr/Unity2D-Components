using UnityEngine;

public class MeleeCollider : CacheBehaviour
{
	void Start()
	{
		// only enable MeleeCollider during attacks
		collider2D.enabled = false;
		gameObject.layer = WEAPON_COLLIDER;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == ENEMY_COLLIDER)
		{
			collider2D.enabled = false;
		}
	}

	public void EnableMeleeCollider()
	{
			collider2D.enabled = true;
	}

	public void DisableMeleeCollider()
	{
			collider2D.enabled = false;
	}
}
