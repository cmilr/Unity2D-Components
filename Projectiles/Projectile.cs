using UnityEngine;
using System.Collections;
using Matcha.Game.Tweens;

public class Projectile : CacheBehaviour {

    private float xOrigin;
    private float yOrigin;
    private float distance = 100f;

    public void Init(Weapon weapon)
    {
        xOrigin = transform.position.x;
        yOrigin = transform.position.y;
        spriteRenderer.sprite = weapon.sprite;
        InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
    }

    public void Fire(Weapon weapon, float direction)
    {
        // fire projectile
        rigidbody2D.velocity = transform.right * weapon.projectileSpeed * direction;
    }

    public void Fire(Weapon weapon, GameObject target)
    {
        MTween.Fade(spriteRenderer, 0f, 0f, 0f);
        MTween.Fade(spriteRenderer, 1f, 0f, .3f);

        // fire projectile
        rigidbody2D.velocity = (target.transform.position - transform.position).normalized * weapon.projectileSpeed;
    }

    void CheckDistanceTraveled()
    {
        // check for distance traveled, then disable when passes threshold
        if (Mathf.Abs(transform.position.x - xOrigin) > distance ||
            Mathf.Abs(transform.position.y - yOrigin) > distance)
            gameObject.SetActive(false);
    }
}