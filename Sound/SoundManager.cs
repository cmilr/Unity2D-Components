using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SoundManager : CacheBehaviour
{
	public AudioClip collectPrize;

	void Start()
	{
		AudioListener.volume = 1F;
	}

	void OnPrizeCollected(int worth)
	{
		if (worth > 5)
		{
			audio.PlayOneShot(collectPrize, 0F);
		}
	}

	void OnEnable()
	{
		Evnt.Subscribe<int>("prize collected", OnPrizeCollected);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<int>("prize collected", OnPrizeCollected);
	}
}
