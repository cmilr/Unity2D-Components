using UnityEngine;
using System.Collections;

public class AnimationBehaviour : CacheBehaviour {

    protected float previousX = 0f;
    protected float previousY = 0f;

    protected void OffsetAnimation(float xOffset, float yOffset)
    {
        if (xOffset != previousX || yOffset != previousY)
            transform.localPosition = new Vector3(xOffset, yOffset, 0);

        previousX = xOffset;
        previousY = yOffset;
    }
}
