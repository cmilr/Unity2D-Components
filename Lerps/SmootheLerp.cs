using UnityEngine;
using UnityEngine.Assertions;

public class SmootheLerp : BaseBehaviour
{
	public float lerpTime = 5f;
	public float moveDistance = 5f;
	private float currentLerpTime;
	private Vector3 startPos;
	private Vector3 endPos;
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
		
		startPos = transform.position;
		endPos   = transform.position + transform.up * moveDistance;
	}

	void Update()
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
