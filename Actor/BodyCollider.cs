using UnityEngine;
using System.Collections;

public class BodyCollider : MonoBehaviour {

	private CollisionManager collisionManager;

	void Start()
	{
		collisionManager = transform.parent.GetComponent<CollisionManager>();
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		collisionManager.OnBodyTriggerEnter(coll.gameObject);
	}

	public void OnTriggerStay2D(Collider2D coll)
	{
		collisionManager.OnBodyTriggerStay(coll.gameObject);
	}

	public void OnTriggerExit2D(Collider2D coll)
	{
		collisionManager.OnBodyTriggerExit(coll.gameObject);
	}
}