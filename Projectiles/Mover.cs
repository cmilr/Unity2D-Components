using UnityEngine;
using System.Collections;

public class Mover : CacheBehaviour {

    public float speed;

    public void StartProjectile (bool facingRight)
    {
        if (facingRight)
            rigidbody2D.velocity = transform.right * speed;
        else
            rigidbody2D.velocity = transform.right * -speed;
    }
}
