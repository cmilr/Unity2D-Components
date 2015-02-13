using UnityEngine;
using System.Collections;

public class WeaponCollider : MonoBehaviour {

	private CollisionManager collisionManager;

	void Start()
	{
		collisionManager = transform.parent.GetComponent<CollisionManager>();
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		collisionManager.OnWeaponTriggerEnter(coll.gameObject);
	}

	public void OnTriggerStay2D(Collider2D coll)
	{
		collisionManager.OnWeaponTriggerStay(coll.gameObject);
	}

	public void OnTriggerExit2D(Collider2D coll)
	{
		collisionManager.OnWeaponTriggerExit(coll.gameObject);
	}
}