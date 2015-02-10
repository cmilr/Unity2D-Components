using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;


public class InteractiveEntity : CacheBehaviour
{
    public enum EntityType { none, prize, weapon };
    public EntityType entityType;

    [HideInInspector]
    public bool alreadyCollided = false;
    public bool disableIfOffScreen = true;
    public int worth;

    void Start()
    {
        float autoY;
        
        base.CacheComponents();

        switch (entityType)
        {
        case EntityType.none:
            break;

        case EntityType.prize:
                autoY = (float)(Math.Ceiling(transform.position.y) - .623f);
                transform.position = new Vector3(transform.position.x, autoY, transform.position.z);
            break;

        case EntityType.weapon:

            break;
        }
    }

    void OnBecameInvisible() 
    {
        if(disableIfOffScreen)
            gameObject.SetActive(false);
    }

    void OnBecameVisible() 
    {
        if(disableIfOffScreen)
            gameObject.SetActive(true);
    }

    public void React()
    {
        alreadyCollided = true;

        switch (entityType)
        {
        case EntityType.none:
            break;

        case EntityType.prize:
                MTween.PickupPrize(gameObject);
            break;

        case EntityType.weapon:
                MTween.PickupWeapon(gameObject);
            break;
        }
    }

    public void SelfDestruct(int inSeconds)
    {
        Destroy(gameObject, inSeconds);
    }
}
