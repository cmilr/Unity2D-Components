// this script is based on the work of Robert Utter, as found on his blog at
// https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/

using System.Collections;
using UnityEngine;

public class SmootheLerp : CacheBehaviour
{
	public float lerpTime = 5f;
	public float moveDistance = 5f;

	float currentLerpTime;
	Vector3 startPos;
	Vector3 endPos;

	protected void Start()
	{
		startPos = transform.position;
		endPos   = transform.position + transform.up * moveDistance;
	}

	protected void Update()
	{
		//reset when we press spacebar
		if (Input.GetKeyDown(KeyCode.Space))
		{
			currentLerpTime = 0f;
		}

		//increment timer once per frame
		currentLerpTime += Time.deltaTime;

		if (currentLerpTime > lerpTime)
		{
			currentLerpTime = lerpTime;
		}

		//lerp!
		float t = currentLerpTime / lerpTime;
		t = t * t * t * (t * (6f * t - 15f) + 10f);
		// float perc = currentLerpTime / lerpTime;
		transform.position = Vector3.Lerp(startPos, endPos, t);
	}
}
