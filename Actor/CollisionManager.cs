using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]


public class CollisionManager : CacheBehaviour
{
    private GameObject coll;
    private InteractiveEntity interEntity;

    void Start()
    {
        base.CacheComponents();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        coll = col.gameObject;

        interEntity = coll.GetComponent<InteractiveEntity>() as InteractiveEntity;

        if (coll.tag == "Prize" && !interEntity.alreadyCollided)
        {
            Messenger.Broadcast<GameObject, int>("prize collected", coll, interEntity.worth);
            interEntity.React();
        }
    }
}