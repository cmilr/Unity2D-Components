using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]


public class StickToMovingPlatforms : CacheBehaviour
{
    private bool isPlayer;

    void Start()
    {
        base.CacheComponents();
        isPlayer = (gameObject.name == "Player");
    }

    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "MovingPlatform")
        {
            transform.parent = coll.gameObject.transform;
            
            if (isPlayer && coll.gameObject.GetComponent<MovingPlatform>().fastPlatform)
                Messenger.Broadcast<bool>("riding fast platform", true);
        }
    }

    void OnTriggerStay2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "MovingPlatform")
            transform.parent = coll.gameObject.transform;
    }

    void OnTriggerExit2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;

            if (isPlayer)
                Messenger.Broadcast<bool>("riding fast platform", false);
        }
    }
}
