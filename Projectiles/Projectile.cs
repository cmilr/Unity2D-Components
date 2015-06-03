using UnityEngine;
using System.Collections;

public class Projectile : CacheBehaviour {

    public float speed;

    public void Shoot (float direction)
    {
        rigidbody2D.velocity = transform.right * speed * direction;
    }

    public void Init(Sprite initSprite)
    {
        spriteRenderer.sprite = initSprite;
    }
}
