using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]


public class StickToMovingPlatforms : CacheBehaviour
{
    void Start()
    {
        base.CacheComponents();
    }

    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "MovingPlatform")
            transform.parent = coll.gameObject.transform;
    }

    void OnTriggerStay2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "MovingPlatform")
            transform.parent = coll.gameObject.transform;
    }

    void OnTriggerExit2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "MovingPlatform")
            transform.parent = null;
    }
}
