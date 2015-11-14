using UnityEngine;
using System.Collections;


public class PixelArtCamera : BaseBehaviour
{
    private float baseOrthographicSize;

    void Start ()
    {
    	SetOrthographicSize();
    }

    void SetOrthographicSize()
    {
        // Experiment with: 32, 48, 64, 96.
        baseOrthographicSize = Screen.height / 64.0f / 2.0f;
        Camera.main.orthographicSize = baseOrthographicSize;
    }

    void OnScreenSizeChanged(float vExtent, float hExtent)
    {
        SetOrthographicSize();
    }

	void OnEnable()
	{
		Messenger.AddListener<float, float>( "screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<float, float>( "screen size changed", OnScreenSizeChanged);
	}
}
