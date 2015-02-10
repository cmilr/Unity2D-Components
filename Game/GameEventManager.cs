using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameData))]


public class GameEventManager : CacheBehaviour
{
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
        GameData.IncreaseScore(worth);
        Messenger.Broadcast<int>("change score", GameData.GetScore());
    }
}
