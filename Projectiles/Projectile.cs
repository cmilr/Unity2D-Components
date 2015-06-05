using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;

public class Projectile : CacheBehaviour {

    private float maxDistance;
    private Vector3 origin;

    public void Init(Weapon weapon)
    {
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spriteRenderer.sprite = weapon.sprite;
        maxDistance = weapon.maxDistance;
        InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
    }

    public void Fire(Weapon weapon, float direction)
    {
        rigidbody2D.velocity = transform.right * weapon.speed * direction;
    }

    public void Fire(Weapon weapon, GameObject target)
    {
        MTween.Fade(spriteRenderer, 0f, 0f, 0f);
        MTween.Fade(spriteRenderer, 1f, 0f, .3f);
        rigidbody2D.velocity = (target.transform.position - transform.position).normalized * weapon.speed;
    }

    void CheckDistanceTraveled()
    {
        float distance = Vector3.Distance(origin, transform.position);
        if (distance > maxDistance)
        {
            MTween.FadeOutProjectile(spriteRenderer, 0f, 0f, .6f);
            Invoke("Deactivate", 1.5f);
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}