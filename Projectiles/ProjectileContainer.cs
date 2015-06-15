using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;

public class ProjectileContainer : Weapon {

    private Vector3 origin;
    private Weapon weapon;

    // Init() is called by the various Fire() functions
    void Init()
    {
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spriteRenderer.sprite = weapon.sprite;
        InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
    }

    public void Fire(Weapon incomingWeapon, float direction)
    {
        weapon = incomingWeapon;
        Init();
        rigidbody2D.velocity = transform.right * weapon.speed * direction;
    }

    public void Fire(Weapon incomingWeapon, Transform target)
    {
        weapon = incomingWeapon;
        Init();
        MTween.Fade(spriteRenderer, 0f, 0f, 0f);
        MTween.Fade(spriteRenderer, 1f, 0f, .3f);
        rigidbody2D.velocity = (target.position - transform.position).normalized * weapon.speed;
    }

    void CheckDistanceTraveled()
    {
        float distance = Vector3.Distance(origin, transform.position);
        if (distance > weapon.maxDistance)
        {
            MTween.FadeOutProjectile(spriteRenderer, 0f, 0f, .6f);
            Invoke("Deactivate", 1.5f);
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    override public void PlayIdleAnimation(float xOffset, float yOffset) {}
    override public void PlayRunAnimation(float xOffset, float yOffset) {}
    override public void PlayJumpAnimation(float xOffset, float yOffset) {}
    override public void PlaySwingAnimation(float xOffset, float yOffset) {}
    override public void EnableAnimation(bool status) {}
}