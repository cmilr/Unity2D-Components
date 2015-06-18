using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;

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
        float distance;
        float yDifference;
        float angleToPoint;
        float distanceFactor;
        float distanceCompensation;
        float speedCompensation;
        float angleCorrection;
        float speed;

        Init(weapon, target);
        MTween.Fade(spriteRenderer, 0f, 0f, 0f);
        MTween.Fade(spriteRenderer, 1f, 0f, .3f);

        // if weapon has no mass, fire projectile linearally
        if (rigidbody2D.mass <= .001f)
        {
            rigidbody2D.gravityScale = 0;
            rigidbody2D.velocity = (target.position - transform.position).normalized * weapon.speed;
        }
        else
        {   // otherwise, lob projectile like a cannon ball
            // the below formula is accurate given the following settings: gravity of .5,
            // angular drag of .05, mass of 1, and projectile speeds between 8 and 20

            distance = target.position.x - transform.position.x;
            yDifference = target.position.y - origin.y;
            angleToPoint = (float)Math.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x);
            speed = weapon.speed;

            // compensate for various projectile speeds
            // in terms of accuracy, the data set below supports speeds between 8 and 20
            if (speed < 8)
                speedCompensation = 3.2f;
            else if (speed >= 8 && speed < 9)
                speedCompensation = 2.2f;
            else if (speed >= 9 && speed < 10)
                speedCompensation = 1.85f;
            else if (speed >= 10 && speed < 11)
                speedCompensation = 1.50f;
            else if (speed >= 11 && speed < 12)
                speedCompensation = 1.25f;
            else if (speed >= 12 && speed < 13)
                speedCompensation = 1f;
            else if (speed >= 13 && speed < 14)
                speedCompensation = .85f;
            else if (speed >= 14 && speed < 15)
                speedCompensation = .75f;
            else if (speed >= 15 && speed < 16)
                speedCompensation = .65f;
            else if (speed >= 16 && speed < 17)
                speedCompensation = .55f;
            else if (speed >= 17 && speed < 18)
                speedCompensation = .5f;
            else if (speed >= 18 && speed < 19)
                speedCompensation = .45f;
            else if (speed >= 19 && speed < 20)
                speedCompensation = .4f;
            else
                speedCompensation = .35f;

            // compensate for both positive and negative distances between origin and target
            distanceFactor = .034f * speedCompensation;

            if (yDifference >= -2f)
                distanceCompensation = .001f * speedCompensation;
            else
                distanceCompensation = .00065f * speedCompensation;

            distanceFactor += yDifference * distanceCompensation;
            angleCorrection = (float)(3.14*0.18) * (distance * distanceFactor);
            rigidbody2D.velocity = new Vector2((float)Math.Cos(angleToPoint+angleCorrection) * speed,
                                               (float)Math.Sin(angleToPoint+angleCorrection) * speed);
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


    //     // at zero
    //     if (xDist >= 0 && xDist <= 2)
    //         yComp = xDist * 0f;
    //     else if (xDist > 2 && xDist <= 4)
    //         yComp = xDist * .15f;
    //     else if (xDist > 4 && xDist <= 6)
    //         yComp = xDist * .17f;
    //     else if (xDist > 6 && xDist <= 8)
    //         yComp = xDist * .20f;
    //     else if (xDist > 8 && xDist <= 10)
    //         yComp = xDist * .25f;
    //     else if (xDist > 10 && xDist <= 12)
    //         yComp = xDist * .3f;
    //     else if (xDist > 12 && xDist <= 14)
    //         yComp = xDist * .37f;
    //     else if (xDist > 14 && xDist <= 16)
    //         yComp = xDist * .44f;
    //     else if (xDist > 16 && xDist <= 18)
    //         yComp = xDist * .53f;
    //     else if (xDist > 18 && xDist <= 20)
    //         yComp = xDist * .65f;
    //     else
    //         yComp = xDist * .99f;
        // }

    override public void PlayIdleAnimation(float xOffset, float yOffset) {}
    override public void PlayRunAnimation(float xOffset, float yOffset) {}
    override public void PlayJumpAnimation(float xOffset, float yOffset) {}
    override public void PlaySwingAnimation(float xOffset, float yOffset) {}
    override public void EnableAnimation(bool status) {}
}

