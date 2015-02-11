using UnityEngine;
using System.Collections;


public class PixelArtCamera : MonoBehaviour
{
    private float s_baseOrthographicSize;

    void Start ()
    {
        // Experiment with: 32, 48, 64, 96.
        s_baseOrthographicSize = Screen.height / 64.0f / 2.0f;
        Camera.main.orthographicSize = s_baseOrthographicSize;
    }
}
