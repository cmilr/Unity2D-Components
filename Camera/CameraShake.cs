using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraShake : BaseBehaviour
{
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}

	void OnShakeCamera(float duration, float strength, int vibrato, float randomness)
	{
		transform.DOShakePosition(duration, strength, vibrato, randomness, false);
	}

	void OnEnable()
	{
		EventKit.Subscribe<float, float, int, float>("shake camera", OnShakeCamera);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<float, float, int, float>("shake camera", OnShakeCamera);
	}
}
