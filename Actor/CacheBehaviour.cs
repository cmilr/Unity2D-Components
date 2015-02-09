using UnityEngine;


public class CacheBehaviour : MonoBehaviour
{
    [HideInInspector]
    public new AudioSource audio;
    [HideInInspector]
    public new Camera camera;
    [HideInInspector]
    public new Collider collider;
    [HideInInspector]
    public new Collider2D collider2D;
    [HideInInspector]
    public new Renderer renderer;
    [HideInInspector]
    public new Rigidbody rigidbody;
    [HideInInspector]
    public new Rigidbody2D rigidbody2D;
    [HideInInspector]
    public new Transform transform;

    public void CacheComponents()
    {
        transform = gameObject.transform;
        if (gameObject.audio) audio = gameObject.audio;
        if (gameObject.camera) camera = gameObject.camera;
        if (gameObject.collider) collider = gameObject.collider;
        if (gameObject.collider2D) collider2D = gameObject.collider2D;
        if (gameObject.renderer) renderer = gameObject.renderer;
        if (gameObject.rigidbody) rigidbody = gameObject.rigidbody;
        if (gameObject.rigidbody2D) rigidbody2D = gameObject.rigidbody2D;
    }
}
