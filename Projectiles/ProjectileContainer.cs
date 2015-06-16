using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;

public class ProjectileContainer : Weapon {

    private Vector3 origin;
    private Weapon weapon;
    private bool linear;

    void Init(Weapon weapon)
    {
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.weapon = weapon;
        spriteRenderer.sprite = weapon.sprite;
        GetWeaponMass();
        InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
    }

    public void Fire(Weapon weapon, float direction)
    {
        Init(weapon);
        rigidbody2D.velocity = transform.right * weapon.speed * direction;
    }

    public void Fire(Weapon weapon, Transform target)
    {
        Init(weapon);
        MTween.Fade(spriteRenderer, 0f, 0f, 0f);
        MTween.Fade(spriteRenderer, 1f, 0f, .3f);

        if (linear)
            rigidbody2D.velocity = (target.position - transform.position).normalized * weapon.speed;
        else // lobbed
            rigidbody2D.velocity = (CompensatedTarget(target) - transform.position).normalized * weapon.speed;
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

    void GetWeaponMass()
    {
        // if weapon has appreciable mass it will be lobbed, otherwise, turn off gravity
        rigidbody2D.mass = weapon.mass;

        if (rigidbody2D.mass <= .001f)
        {
            rigidbody2D.gravityScale = 0;
            linear = true;
        }
    }

    Vector3 CompensatedTarget(Transform target)
    {
        float xDist;
        float yDist;
        float yComp;
        float xComp;

        //the farther away the target is horizontally, the higher the enemy aims to make up for gravity
        xDist = Mathf.Abs(transform.position.x - target.position.x);


        Debug.Log(xDist);

        // at zero
        if (xDist >= 0 && xDist <= 2)
            yComp = xDist * 0f;
        else if (xDist > 2 && xDist <= 4)
            yComp = xDist * .15f;
        else if (xDist > 4 && xDist <= 6)
            yComp = xDist * .17f;
        else if (xDist > 6 && xDist <= 8)
            yComp = xDist * .20f;
        else if (xDist > 8 && xDist <= 10)
            yComp = xDist * .25f;
        else if (xDist > 10 && xDist <= 12)
            yComp = xDist * .3f;
        else if (xDist > 12 && xDist <= 14)
            yComp = xDist * .37f;
        else if (xDist > 14 && xDist <= 16)
            yComp = xDist * .44f;
        else if (xDist > 16 && xDist <= 18)
            yComp = xDist * .53f;
        else if (xDist > 18 && xDist <= 20)
            yComp = xDist * .65f;
        else
            yComp = xDist * .99f;

        // get distance vertically
        yDist = Mathf.Abs(transform.position.y - target.position.y);

        // if target is below the enemy
        if (target.position.y < transform.position.y)
        {
            // yComp += yDist * -.1f;
            xComp = yDist * (xDist/10f) * -.9f;
        }

        // if target is above the enemy
        else
        {
            // yComp += yDist * .3f;
            xComp = 0;
        }

        return new Vector3(target.position.x + xComp, target.position.y + yComp, target.position.z);
    }

    override public void PlayIdleAnimation(float xOffset, float yOffset) {}
    override public void PlayRunAnimation(float xOffset, float yOffset) {}
    override public void PlayJumpAnimation(float xOffset, float yOffset) {}
    override public void PlaySwingAnimation(float xOffset, float yOffset) {}
    override public void EnableAnimation(bool status) {}
}


// 4 below
// if (xDist >= 0 && xDist <= 2)
//     yComp = xDist * 0f;
// else if (xDist > 2 && xDist <= 4)
//     yComp = xDist * .1f;
// else if (xDist > 4 && xDist <= 6)
//     yComp = xDist * .13f;
// else if (xDist > 6 && xDist <= 8)
//     yComp = xDist * .18f;
// else if (xDist > 8 && xDist <= 10)
//     yComp = xDist * .22f;
// else if (xDist > 10 && xDist <= 12)
//     yComp = xDist * .26f;
// else if (xDist > 12 && xDist <= 14)
//     yComp = xDist * .31f;
// else if (xDist > 14 && xDist <= 16)
//     yComp = xDist * .36f;
// else if (xDist > 16 && xDist <= 18)
//     yComp = xDist * .42f;
// else if (xDist > 18 && xDist <= 20)
//     yComp = xDist * .50f;
// else if (xDist > 20 && xDist <= 22)
//     yComp = xDist * .59f;
// else
//     yComp = xDist * .73f;