using UnityEngine;

public class CheckScreenSize : CacheBehaviour
{
	private float vertExtent;
	private float horizExtent;
	private int currentScreenWidth;
	private int currentScreenHeight;

	void Start()
	{
		vertExtent           = Camera.main.GetComponent<Camera>().orthographicSize;
		horizExtent          = vertExtent * Screen.width / Screen.height;
		currentScreenWidth   = Screen.width;
		currentScreenHeight  = Screen.height;

		InvokeRepeating("CheckForSizeChange", 0f, 0.3F);
	}

	void CheckForSizeChange()
	{
		if (Screen.width != currentScreenWidth || Screen.height != currentScreenHeight)
		{
			vertExtent  = Camera.main.GetComponent<Camera>().orthographicSize;
			horizExtent = vertExtent * Screen.width / Screen.height;

			EventKit.Broadcast<float, float>("screen size changed", vertExtent, horizExtent);
		}
	}
}
