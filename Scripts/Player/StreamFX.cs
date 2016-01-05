using Matcha.Unity;
using System.Collections;
using UnityEngine;

public class StreamFX : CacheBehaviour
{
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
