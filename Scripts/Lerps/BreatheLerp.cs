// this script is based on the work of Robert Utter, as found on his blog at
// https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/

using UnityEngine;

public class BreatheLerp : CacheBehaviour
{
	public float amplitude = 1f;
	public float period = 1f;

	Vector3 startPos;
	Transform player;
	bool disabled;

	protected void Start()
	{
		startPos = transform.position;
		player = GameObject.Find(PLAYER).transform;
		InvokeRepeating("CullingCheck", 0f, .2f);
	}

	protected void Update()
	{
		if (!disabled)
		{
			float theta        = Time.timeSinceLevelLoad / period;
			float distance     = amplitude * Mathf.Sin(theta);
			transform.position = startPos + Vector3.up * distance;
		}
	}

	// disable if far from player
	void CullingCheck()
	{
		float distance = Vector3.Distance(player.position, transform.position);
		disabled = (distance > CULL_DISTANCE ? true : false);
	}
}
