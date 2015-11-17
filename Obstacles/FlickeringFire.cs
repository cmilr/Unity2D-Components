using Matcha.Dreadful;
using Matcha.Unity;
using System.Collections;
using UnityEngine;

public class FlickeringFire : CacheBehaviour
{
	public float minIntensity;
	public float maxIntensity;

	void Start()
	{
		StartCoroutine(FlickerFXLoop());
	}

	IEnumerator FlickerFXLoop()
	{
		while (true)
		{
			MFX.Flicker(light, minIntensity, maxIntensity);
			yield return new WaitForSeconds(Rand.Range(.01f, .3f));
		}
	}
}
