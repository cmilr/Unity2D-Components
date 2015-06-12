// this script is based on the work of Robert Utter, as found on his blog at
// https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/

using UnityEngine;
using System.Collections;

public class Breathe : CacheBehaviour {

    Vector3 startPos;

    public float amplitude = 1f;
    public float period = 1f;

    protected void Start() {
        startPos = transform.position;
    }

    protected void Update() {
        float theta = Time.timeSinceLevelLoad / period;
        float distance = amplitude * Mathf.Sin(theta);
        transform.position = startPos + Vector3.up * distance;
    }
}