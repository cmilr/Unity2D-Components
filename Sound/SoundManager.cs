using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]

//uses mp3 for music, and ogg for sound effects
public class SoundManager : BaseBehaviour
{
	public AudioClip collectPrize;
	private new AudioSource audio;

	void Awake()
	{
		audio = GetComponent<AudioSource>();
		Assert.IsNotNull(audio);
		
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
		EventKit.Subscribe<int>("prize collected", OnPrizeCollected);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("prize collected", OnPrizeCollected);
	}
}
