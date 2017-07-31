using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class FlickeringFireLerp : BaseBehaviour
{
	public float minIntensity 		= 1f;
	public float maxIntensity 		= 1f;
	private float lerpTime       	= 1f;
	private float startIntensity;
	private float endIntensity;
	private float currentLerpTime;
	private float distance;
	private bool disabled;
	private Transform player;
	private new Transform transform;
	private Light foregroundLight;
	private Light backgroundLight;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}
	
	void Start()
	{
		player = GameObject.Find(PLAYER).GetComponent<Transform>();
		Assert.IsNotNull(player);

		foregroundLight = transform.Find("Foreground_Light").GetComponent<Light>();
		Assert.IsNotNull(foregroundLight);

		backgroundLight = transform.Find("Background_Light").GetComponent<Light>();
		Assert.IsNotNull(backgroundLight);
		
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
				lerpTime = Rand.Range(.1f, .3f);
				currentLerpTime = 0f;
				startIntensity = endIntensity;
				endIntensity = Rand.Range(minIntensity, maxIntensity);
			}

			// lerp!
			var perc = currentLerpTime / lerpTime;
			var intensity = Mathf.Lerp(startIntensity, endIntensity, perc);

			foregroundLight.intensity = intensity + .5f;
			backgroundLight.intensity = intensity;
		}
	}

	// disable if far from player.
	void CullingCheck()
	{
		distance = Vector3.Distance(player.position, transform.position);
		disabled = (distance > CULL_DISTANCE);
	}
}
