using UnityEngine;
using UnityEngine.Assertions;

public class StreamFX : BaseBehaviour
{
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}
	
	void OnFacingRight(bool status)
	{
		if (status)
		{
			transform.rotation = Quaternion.Euler(0, 270, 180);
		}
		else
		{
			transform.rotation = Quaternion.Euler(180, 270, 180);
		}
	}
}
