using UnityEngine;

public class WeaponCollider : CacheBehaviour
{
	void Start()
	{
		// only enable WeaponCollider during attacks
		collider2D.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == ENEMY_COLLIDER)
		{
			collider2D.enabled = false;
		}
	}

	void OnTriggerStay2D(Collider2D coll) {}

	void OnTriggerExit2D(Collider2D coll) {}
}
