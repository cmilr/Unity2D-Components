public class PhysicsCollider : CacheBehaviour
{
	public void EnablePhysicsCollider()
	{
			collider2D.enabled = true;
	}

	public void DisablePhysicsCollider()
	{
			collider2D.enabled = false;
	}
}
