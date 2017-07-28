using UnityEngine;
using UnityEngine.Assertions;

public class PixelArtCamera : BaseBehaviour
{
	public float OrthoSize			{ get { return orthoSize; } }
	public float FinalUnitSize		{ get { return finalUnitSize; } }
	public float CanvasScale		{ get { return canvasScale; } }
	public int   PixelsPerUnit		{ get { return pixelsPerUnit; } }
	public int   VertUnitsOnScreen	{ get { return verticalUnitsOnScreen; } }

	[SerializeField]
	private int pixelsPerUnit;
	[SerializeField]
	private int verticalUnitsOnScreen;
	private float orthoSize;
	private float finalUnitSize;
	private float canvasScale;
	private new Camera camera;

	void Awake()
	{
		camera = gameObject.GetComponent<Camera>();
		Assert.IsNotNull(camera);

		SetOrthographicSize(verticalUnitsOnScreen);
	}

	void SetOrthographicSize(int unitsOnScreen)
	{
		// get device's screen height and divide by the number of units 
		// that we want to fit on the screen vertically. this gets us
		// the basic size of a unit on the the current device's screen.
		var tempUnitSize = Screen.height / unitsOnScreen;

		// with a basic rough unit size in-hand, we now round it to the
		// nearest power of pixelsPerUnit (ex; 16px.) this will guarantee
		// our sprites are pixel perfect, as they can now be evenly divided
		// into our final device's screen height.
		finalUnitSize = tempUnitSize.GetNearestMultiple(PixelsPerUnit);

		// ultimately, we are using the standard pixel art formula for 
		// orthographic cameras, but approaching it from the view of:
		// how many standard Unity units do we want to fit on the screen?
		// formula: cameraSize = ScreenHeight / (DesiredSizeOfUnit * 2)
		camera.orthographicSize = Screen.height / (finalUnitSize * 2.0f);

		// set OrthoSize property for any components that may need it.
		orthoSize = camera.orthographicSize;

		// set canvasScale for text-based hud elements.
		canvasScale = finalUnitSize * .0078125f;
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("set screen size", SetOrthographicSize);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("set screen size", SetOrthographicSize);
	}
}