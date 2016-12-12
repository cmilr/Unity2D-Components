using UnityEngine;
using UnityEngine.Assertions;

public class PhysicsCollider : BaseBehaviour
{
	private new Collider2D collider2D;

	void Awake()
	{
		collider2D = GetComponent<Collider2D>();
		Assert.IsNotNull(collider2D);
	}
	
	public void EnablePhysicsCollider()
	{
		collider2D.enabled = true;
	}

	public void DisablePhysicsCollider()
	{
		collider2D.enabled = false;
	}
}
