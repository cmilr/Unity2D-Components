using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]


public class SoundManager : CacheBehaviour
{
    public AudioClip collectPrize;

    void Start()
    {
        base.CacheComponents();
    }

    // EVENT LISTENERS
    void OnEnable()
    {
        Messenger.AddListener<GameObject, int>( "prize collected", OnPrizeCollected );
    }

    void OnDisable()
    {
        Messenger.RemoveListener<GameObject, int>( "prize collected", OnPrizeCollected );
    }


    // EVENT RESPONDERS
    void OnPrizeCollected(GameObject prize, int worth)
    {
        audio.PlayOneShot(collectPrize, 0.1F);
    }

}
