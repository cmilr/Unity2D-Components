using UnityEngine;
using System.Collections;

public class BreakablePiece : CacheBehaviour {

    private float xRectPosition;
    private float yRectPosition;

    public void Init(Sprite breakableSprite)
    {
        // name = "Piece_" + index;
        spriteRenderer.sprite = breakableSprite;
        // xRectPosition = spriteRenderer.sprite.rect.x;
    }
}
