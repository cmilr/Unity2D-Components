using DG.Tweening;

public class CameraShake : CacheBehaviour
{
	void OnShakeCamera(float duration, float strength, int vibrato, float randomness)
	{
		transform.DOShakePosition(duration, strength, vibrato, randomness, false);
	}

	void OnEnable()
	{
		Messenger.AddListener<float, float, int, float>("shake camera", OnShakeCamera);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<float, float, int, float>("shake camera", OnShakeCamera);
	}

}
