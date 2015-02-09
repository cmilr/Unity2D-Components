using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;


public class InteractiveEntity : MonoBehaviour
{
    public enum EntityType { none, prize, weapon };
    public EntityType entityType;

    [HideInInspector]
    public bool alreadyCollided = false;
    public bool disableIfOffScreen = true;
    public int worth;

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
