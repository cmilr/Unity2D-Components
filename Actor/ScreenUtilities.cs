using UnityEngine;
using System.Collections;


public class ScreenUtilities : CacheBehaviour
{
    public GameObject objectToTrack;            // object whose position we are tracking.
    private Transform trackedObject;            // reference to the tracked object's transform.
    private float vertExtent;                   // half the height of the game screen.
    private float horizExtent;                  // half the width of the game screen.

    void Start()
    {
        base.CacheComponents();
        trackedObject = objectToTrack.transform;

        vertExtent = Camera.main.camera.orthographicSize;
        horizExtent = vertExtent * Screen.width / Screen.height;
    }

    // returns the distance from a gameObject to the edge of the screen on 2D orthographic cameras.
    public float DistanceFromEdge(string screenEdge)
    {
        switch (screenEdge.ToLower())
        {
        case "top":
            return Mathf.Abs(transform.position.y + vertExtent - trackedObject.position.y);

        case "bottom":
            return Mathf.Abs(transform.position.y - vertExtent - trackedObject.position.y);

        case "left":
            return Mathf.Abs(transform.position.x - horizExtent - trackedObject.position.x);

        case "right":
            return Mathf.Abs(transform.position.x + horizExtent - trackedObject.position.x);

        default:
            Debug.Log("ERROR: Which edge would you like to check? top, bottom, left, or right.");
            return 0f;
        }
    }
}
