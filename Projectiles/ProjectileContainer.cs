using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;
using Matcha.Lib;

public class ProjectileContainer : Weapon {

    private Transform target;
    private Vector3 origin;
    private bool linear;
    private Weapon weapon;

    void Init(Weapon weapon, Transform target)
    {
        this.weapon = weapon;
        this.target = target;
        rigidbody2D.mass = weapon.mass;
        spriteRenderer.sprite = weapon.sprite;
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
    }

    public void Fire(Weapon weapon, float direction)
    {
        Init(weapon, target);
        rigidbody2D.velocity = transform.right * weapon.speed * direction;
    }

    public void Fire(Weapon weapon, Transform target)
    {
        Init(weapon, target);
        MTween.Fade(spriteRenderer, 0f, 0f, 0f);
        MTween.Fade(spriteRenderer, 1f, 0f, .3f);

        if (rigidbody2D.mass <= .001f)
        {   // if weapon has no mass, fire projectile linearally
            rigidbody2D.gravityScale = 0;
            rigidbody2D.velocity = (target.position - transform.position).normalized * weapon.speed;
        }
        else
        {   // otherwise, lob projectile like a cannon ball
            rigidbody2D.velocity = MLib.LobProjectile(weapon, transform, target);
        }
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

