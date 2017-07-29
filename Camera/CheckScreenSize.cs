using UnityEngine;
using UnityEngine.Assertions;

public class CheckScreenSize : BaseBehaviour
{
	private float vertExtent;
	private float horizExtent;
	private int currentScreenWidth;
	private int currentScreenHeight;
	private new Camera camera;

	void Start()
	{
		camera = Camera.main.GetComponent<Camera>();
		Assert.IsNotNull(camera);

		vertExtent			= camera.orthographicSize;
		horizExtent			= vertExtent * Screen.width / Screen.height;
		currentScreenWidth	= Screen.width;
		currentScreenHeight	= Screen.height;

		InvokeRepeating("CheckForSizeChange", 0f, 0.3F);
	}

	void CheckForSizeChange()
	{
		if (Screen.width != currentScreenWidth || Screen.height != currentScreenHeight)
		{
			vertExtent = camera.orthographicSize;
			horizExtent = vertExtent * Screen.width / Screen.height;

			EventKit.Broadcast("screen size changed");
		}
	}
}
