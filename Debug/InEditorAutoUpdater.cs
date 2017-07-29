using UnityEngine;
using UnityEngine.UI;

// keeps editor Game View in sync with camera & resolution settings.
// press play to refresh Game View.

[ExecuteInEditMode]
public class InEditorAutoUpdater : BaseBehaviour
{
#if UNITY_EDITOR

	private CanvasScaler[] canvasScaler;
	private float orthoSize;
	private float canvasScale;
	private int pixelsPerUnit;

	void Start()
	{
		canvasScaler = Camera.main.GetComponentsInChildren<CanvasScaler>();
		canvasScale = Camera.main.GetComponent<PixelArtCamera>().CanvasScale;
	}

	void Update()
	{
		SetCameraOrthographicSize();
	}

	void SetCameraOrthographicSize()
	{
		foreach (CanvasScaler c in canvasScaler)
		{
			c.scaleFactor = canvasScale;
		}
	}

	void RefreshGameView()
	{
		UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
	}

	void OnApplicationQuit()
	{
		SetCameraOrthographicSize();
	}

#endif
}
