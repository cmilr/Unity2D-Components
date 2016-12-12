using UnityEngine;
using UnityEngine.Assertions;

public class PixelArtCamera : BaseBehaviour
{
    public int pixelsPerUnit;
    public int screenZoom;
	private new Camera camera;

	void Awake()
	{
		camera = gameObject.GetComponent<Camera>();
		Assert.IsNotNull(camera);
		
		SetOrthographicSize();
	}

	void SetOrthographicSize()
	{
        // screen width and other settings the game was designed to             // REFERENCE NUMS:
        var baseScreenWidth = 1024f;                                            // 1024        
        var baseOrthographicFactor = pixelsPerUnit * screenZoom;                // 112
        var baseCanvasScale = .0078125 * baseOrthographicFactor;                // .875
        var baseResolutionOffset = .00446428571429 * baseOrthographicFactor;    // .5

        // ratio of the original screen width to the actual screen width being displayed
        var screenWidthRatio = Screen.width / baseScreenWidth;

        // calculate global constants.
        BASE_PIXELS_PER_UNIT = pixelsPerUnit;
        ORTHOGRAPHIC_FACTOR = baseOrthographicFactor * screenWidthRatio;
        CANVAS_SCALE = (float)(baseCanvasScale * screenWidthRatio);
        RESOLUTION_OFFSET = (float)(baseResolutionOffset * screenWidthRatio);

        // set camera size.
		camera.orthographicSize = FINAL_ORTHOGRAPHIC_SIZE = Screen.height / ORTHOGRAPHIC_FACTOR / 2.0f;
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
