using Matcha.Unity;
using System.Collections;
using UnityEngine;

public class FlickeringFireLerp : CacheBehaviour
{
	public float minIntensity = .2f;
	public float maxIntensity = 5f;

	float lerpTime = 1f;
	float currentLerpTime;
	float startIntensity = 1f;
	float endIntensity = 5f;

	void Update()
	{
		//increment timer once per frame
		currentLerpTime += Time.deltaTime;

		if (currentLerpTime > lerpTime)
		{
			lerpTime = Rand.Range(.05f, .3f);
			currentLerpTime = 0f;
			startIntensity = endIntensity;
			endIntensity = Rand.Range(minIntensity, maxIntensity);
		}

		//lerp!
		float perc = currentLerpTime / lerpTime;
		light.intensity = Mathf.Lerp(startIntensity, endIntensity, perc);
	}
}
