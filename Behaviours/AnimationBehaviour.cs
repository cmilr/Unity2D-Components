using UnityEngine;
using System.Collections;

public class AnimationBehaviour : CacheBehaviour {

    protected void OffsetAnimation(float xOffset, float yOffset)
    {
        transform.localPosition = new Vector3(xOffset, yOffset, 0);
    }
}
