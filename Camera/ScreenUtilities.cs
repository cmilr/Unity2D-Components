using UnityEngine;
using System.Collections;


public class ScreenUtilities : CacheBehaviour
{
	public GameObject objectToTrack;            // object whose position we are tracking.
	private Transform trackedObject;            // reference to the tracked object's transform.
	private float vertExtent;                   // half the height of the game screen.
	private float horizExtent;                  // half the width of the game screen.
	private int currentScreenWidth;
    private int currentScreenHeight;

	void Start()
	{
		trackedObject = objectToTrack.transform;

		vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;
		horizExtent = vertExtent * Screen.width / Screen.height;

		currentScreenWidth = Screen.width;
    	currentScreenHeight = Screen.height;

    	InvokeRepeating("CheckScreenSize", 0f, 0.1F);
	}

	void CheckScreenSize()
	{
    	if (Screen.width != currentScreenWidth || Screen.height != currentScreenHeight)
    	{
	        vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;
			horizExtent = vertExtent * Screen.width / Screen.height;

	        Messenger.Broadcast<float, float>("screen size changed", vertExtent, horizExtent);
    	}
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
