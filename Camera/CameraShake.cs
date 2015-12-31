using DG.Tweening;

public class CameraShake : CacheBehaviour
{
	void OnShakeCamera(float duration, float strength, int vibrato, float randomness)
	{
		transform.DOShakePosition(duration, strength, vibrato, randomness, false);
	}

	void OnEnable()
	{
		Evnt.Subscribe<float, float, int, float>("shake camera", OnShakeCamera);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<float, float, int, float>("shake camera", OnShakeCamera);
	}
}
