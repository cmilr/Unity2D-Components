using Matcha.Unity;
using UnityEngine;

public class FlickeringFireLerp : CacheBehaviour
{
	public float minIntensity = 1f;
	public float maxIntensity = 12f;

	float lerpTime       = 1f;
	float startIntensity = 1f;
	float endIntensity   = 5f;
	float currentLerpTime;
	bool disabled;
	Transform player;

	void Start()
	{
		player = GameObject.Find(PLAYER).GetComponent<Transform>();
		InvokeRepeating("CullingCheck", 0f, .2f);
	}

	void Update()
	{
		if (!disabled)
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

	// disable if far from player
	void CullingCheck()
	{
		float distance = Vector3.Distance(player.position, transform.position);
		disabled = (distance > CULL_DISTANCE ? true : false);
	}
}
