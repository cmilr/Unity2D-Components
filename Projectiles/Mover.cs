using UnityEngine;
using System.Collections;

public class Mover : CacheBehaviour {

    public float speed;

    public void StartProjectile (float direction)
    {
        rigidbody2D.velocity = transform.right * speed * direction;
    }
}
