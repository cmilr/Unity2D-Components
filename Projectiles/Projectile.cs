using UnityEngine;
using System.Collections;

public class Projectile : CacheBehaviour {

    private float direction;
    private int hp;
    private int ac;
    private int damage;
    private float speed;

    private float xOrigin;
    private float yOrigin;
    private float distance = 100f;

    public void InitAndFire(Sprite initSprite, float direction, float speed)
    {
        this.direction = direction;

        // get initial coordinates
        xOrigin = transform.position.x;
        yOrigin = transform.position.y;

        // set sprite to that of equipped weapon
        spriteRenderer.sprite = initSprite;

        // fire projectile
        rigidbody2D.velocity = transform.right * speed * direction;

        // check for distance traveled, then disable when passes threshold
        InvokeRepeating("CheckDistanceTraveled", 1, 0.3F);
    }

    void CheckDistanceTraveled()
    {
        if (Mathf.Abs(transform.position.x - xOrigin) > distance ||
            Mathf.Abs(transform.position.y - yOrigin) > distance)
            gameObject.SetActive(false);
    }
}
