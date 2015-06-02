using UnityEngine;
using System.Collections;

public class Mover : CacheBehaviour {

    public float speed;

    void Start ()
    {
        rigidbody2D.velocity = transform.right * -speed;
    }
}
