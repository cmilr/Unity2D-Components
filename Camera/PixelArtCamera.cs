using UnityEngine;
using System.Collections;


public class PixelArtCamera : MonoBehaviour
{
    private float s_baseOrthographicSize;

    void Start ()
    {
        // Set the camera to the correct orthographic size, so
        // scene pixels are 1:1. Experiment with: 32, 48, 64, 96.
        s_baseOrthographicSize = Screen.height / 64.0f / 2.0f;
        Camera.main.orthographicSize = s_baseOrthographicSize;
    }
}
