using UnityEngine;
using System.Collections;


public class PixelArtCamera : BaseBehaviour
{
    private float s_baseOrthographicSize;

    void Start ()
    {
    	SetOrthographicSize();
    }

    void SetOrthographicSize()
    {
        // Experiment with: 32, 48, 64, 96.
        s_baseOrthographicSize = Screen.height / 64.0f / 2.0f;
        Camera.main.orthographicSize = s_baseOrthographicSize;
    }

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<float, float>( "screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<float, float>( "screen size changed", OnScreenSizeChanged);	
	}

	// EVENT RESPONDERS
	void OnScreenSizeChanged(float vExtent, float hExtent)
	{
		SetOrthographicSize();
	}
}
