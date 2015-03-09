using UnityEngine;
using System;
using System.Collections;


public abstract class Entity : CacheBehaviour {

	protected bool collidedWithBody = false;
	protected bool collidedWithWeapon = false;
	public int worth;

	public abstract void ReactToCollision();
	
	public int Worth()
	{
		return worth;
	}

	protected void SelfDestruct(int inSeconds)
	{
		Destroy(gameObject, inSeconds);
	}

	protected void AutoAlign()
	{
		float targetY = (float)(Math.Round(transform.position.y) - ALIGN_ENTITY_TO);
		transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
	}

	public void SetCollidedWithBody(bool status)
	{
		collidedWithBody = status;
	}

	public bool AlreadyCollidedWithBody()
	{
		return collidedWithBody;
	}

	public void SetCollidedWithWeapon(bool status)
	{
		collidedWithWeapon = status;
	}

	public bool AlreadyCollidedWithWeapon()
	{
		return collidedWithWeapon;
	}

	// protected void OnBecameInvisible()
	// {
	// 	if(rigidbody2D)
	// 		rigidbody2D.Sleep();
	// }

	// protected void OnBecameVisible()
	// {
	// 	if(rigidbody2D)
	// 		rigidbody2D.WakeUp();
	// }
}
