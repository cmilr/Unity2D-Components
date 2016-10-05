using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class FlickeringFireLerp : BaseBehaviour
{
	public float minIntensity = 1f;
	public float maxIntensity = 12f;
	private float lerpTime       = 1f;
	private float startIntensity = 1f;
	private float endIntensity   = 5f;
	private float currentLerpTime;
	private float distance;
	private bool disabled;
	private Transform player;
	private new Transform transform;
	private new Light light;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
		
		light = GetComponent<Light>();
		Assert.IsNotNull(light);
	}
	
	void Start()
	{
		player = GameObject.Find(PLAYER).GetComponent<Transform>();
		Assert.IsNotNull(player);
		
		InvokeRepeating("CullingCheck", 0f, 1f);
	}

	void Update()
	{
		if (!disabled)
		{
			// increment timer once per frame.
			currentLerpTime += Time.deltaTime;

			if (currentLerpTime > lerpTime)
			{
				lerpTime = Rand.Range(.05f, .3f);
				currentLerpTime = 0f;
				startIntensity = endIntensity;
				endIntensity = Rand.Range(minIntensity, maxIntensity);
			}

			// lerp!
			float perc = currentLerpTime / lerpTime;
			light.intensity = Mathf.Lerp(startIntensity, endIntensity, perc);
		}
	}

	// disable if far from player.
	void CullingCheck()
	{
		distance = Vector3.Distance(player.position, transform.position);
		disabled = (distance > CULL_DISTANCE);
	}
}
