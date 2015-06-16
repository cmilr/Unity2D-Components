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

        //the farther away the target is horizontally, the higher the enemy aims to make up for gravity
        xDist = Mathf.Abs(transform.position.x - target.position.x);

        Debug.Log(xDist);

        if (xDist >= 0 && xDist <= 5)
            yComp = xDist * 0f;
        else if (xDist > 5 && xDist <= 10)
            yComp = xDist * .2f;
        else if (xDist > 10 && xDist <= 15)
            yComp = xDist * .3f;
        else
            yComp = xDist * .4f;

        // get distance vertically
        yDist = Mathf.Abs(transform.position.y - target.position.y);

        // if target is below the enemy
        if (target.position.y < transform.position.y)
            yComp += yDist * .1f;

        // if target is above the enemy
        else
            yComp += yDist * .3f;

        return new Vector3(target.position.x, target.position.y + yComp, target.position.z);
    }

    override public void PlayIdleAnimation(float xOffset, float yOffset) {}
    override public void PlayRunAnimation(float xOffset, float yOffset) {}
    override public void PlayJumpAnimation(float xOffset, float yOffset) {}
    override public void PlaySwingAnimation(float xOffset, float yOffset) {}
    override public void EnableAnimation(bool status) {}
}