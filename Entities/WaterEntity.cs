using UnityEngine;
using System.Collections;

public class WaterEntity : EntityBehaviour {

	private bool hasCollided;

	void Start()
	{
		base.CacheComponents();
	}

	public void SetCollidedWithBody(bool status)
	{
		hasCollidedWithBody = status;
	}

	public bool HasCollidedWithBody()
	{
		return hasCollidedWithBody;
	}

	public void SetCollidedWithWeapon(bool status)
	{
		hasCollidedWithWeapon = status;
	}

	public bool HasCollidedWithWeapon()
	{
		return hasCollidedWithWeapon;
	}
}
