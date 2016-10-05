using UnityEngine;
using UnityEngine.Assertions;

public class PixelArtCamera : BaseBehaviour
{
	private float baseOrthographicSize;
	private new Camera camera;

	void Awake()
	{
		camera = gameObject.GetComponent<Camera>();
		Assert.IsNotNull(camera);
		
		SetOrthographicSize();
	}

	void SetOrthographicSize()
	{
		baseOrthographicSize = Screen.height / PLATFORM_SPECIFIC_ORTHOGRAPHIC_FACTOR / 2.0f;
		camera.orthographicSize = baseOrthographicSize;
	}

	void OnScreenSizeChanged(float vExtent, float hExtent)
	{
		SetOrthographicSize();
	}

	void OnEnable()
	{
		EventKit.Subscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}
}
