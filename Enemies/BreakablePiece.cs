using UnityEngine;
using System.Collections;

public class BreakablePiece : CacheBehaviour {

    private float xRectPosition;
    private float yRectPosition;

	// Use this for initialization
	void Start () {

        xRectPosition = spriteRenderer.sprite.rect.x;
        Debug.Log(xRectPosition);
	}

	// Update is called once per frame
	void Update () {

	}
}
