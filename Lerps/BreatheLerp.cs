// this script is based on the work of Robert Utter, as found on his blog at
// https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/

using UnityEngine;
using UnityEngine.Assertions;

public class BreatheLerp : BaseBehaviour
{
	public float amplitude = 1f;
	public float period = 1f;
	private bool disabled;
	private Vector3 startPos;
	private Transform player;
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}
	
	void Start()
	{
		player = GameObject.Find(PLAYER).GetComponent<Transform>();
		Assert.IsNotNull(player);

		startPos = transform.position;
		InvokeRepeating("CullingCheck", 0f, .2f);
	}

	void Update()
	{
		if (!disabled)
		{
			float theta = Time.timeSinceLevelLoad / period;
			float distance = amplitude * Mathf.Sin(theta);
			transform.position = startPos + Vector3.up * distance;
		}
	}

	// disable if far from player.
	void CullingCheck()
	{
		float distance = Vector3.Distance(player.position, transform.position);
		disabled = (distance > CULL_DISTANCE);
	}
}
