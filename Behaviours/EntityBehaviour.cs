using UnityEngine;
using System;
using System.Collections;

public class EntityBehaviour : CacheBehaviour {

	public bool AlreadyCollided { get; set; }

	protected void SelfDestruct(int inSeconds)
	{
		Destroy(gameObject, inSeconds);
	}

	protected void OnBecameInvisible()
	{
		if(rigidbody2D)
			rigidbody2D.Sleep();
	}

	protected void OnBecameVisible()
	{
		if(rigidbody2D)
			rigidbody2D.WakeUp();
	}

	protected void AutoAlign()
	{
		float targetY = (float)(Math.Round(transform.position.y) - .124);
		transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
	}
}
