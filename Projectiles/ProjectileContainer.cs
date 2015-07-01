using UnityEngine;
using System;
using System.Collections;
using Matcha.Game.Tweens;
using Matcha.Lib;


public class ProjectileContainer : Weapon {

    private Weapon weapon;
    private Vector3 origin;
    private RuntimeAnimatorController anim;

    void Init(Weapon weapon)
    {
        this.weapon = weapon;
        rigidbody2D.mass = weapon.mass;
        spriteRenderer.sprite = weapon.sprite;
        collider2D.enabled = true;
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // initialize animation controller
        if (weapon.GetComponent<Projectile>().animatedProjectile)
        {
            anim = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate
                (Resources.Load(("AnimControllers/Projectiles/" + weapon.name + "_0"), typeof(RuntimeAnimatorController )));
            animator.runtimeAnimatorController = anim;
            animator.speed = .5f;
            // animator.Play("Flaming Skull");
        }

        InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
    }

    // FIRE DIRECTIONALLY
    public void Fire(Weapon weapon, float direction)
    {
        Init(weapon);
        MTween.Fade(spriteRenderer, 1f, 0f, 0f);
        rigidbody2D.velocity = transform.right * weapon.speed * direction;
    }

    // FIRE AT TARGET
    public void Fire(Weapon weapon, Transform target)
    {
        Init(weapon);
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
            // MTween.FadeOutProjectile(spriteRenderer, 0f, 0f, .6f);
            Invoke("DeactivateCollider", .1f);
            Invoke("DeactivateEntireObject", 1.5f);
        }
    }

    void DeactivateCollider()
    {
        collider2D.enabled = false;
    }

    void DeactivateEntireObject()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        MTween.Fade(spriteRenderer, 1f, 0f, 0f);
    }

    // upon recycling, clear previous references and fade gameObject to zero
    void OnDisable()
    {
        anim = null;
        weapon = null;
        animator.runtimeAnimatorController = null;
        spriteRenderer.sprite = null;
        MTween.Fade(spriteRenderer, 0f, 0f, 0f);
        CancelInvoke();
    }

    public void AllocateMemory()
    {
        // at time of instantiation in the pool, allocate memory for GetComponent() calls
        rigidbody2D.mass = rigidbody2D.mass;
        spriteRenderer.sprite = spriteRenderer.sprite;
        transform.position = transform.position;
        collider2D.enabled = true;
    }

    override public void PlayIdleAnimation(float xOffset, float yOffset) {}
    override public void PlayRunAnimation(float xOffset, float yOffset) {}
    override public void PlayJumpAnimation(float xOffset, float yOffset) {}
    override public void PlaySwingAnimation(float xOffset, float yOffset) {}
    override public void EnableAnimation(bool status) {}
}

