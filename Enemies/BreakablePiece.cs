using UnityEngine;
using System.Collections;

public class BreakablePiece : CacheBehaviour {

    private float xRectPosition;
    private float yRectPosition;
    private float originX;
    private float originY;
    private float newX;
    private float newY;

    public void Init(int index, Sprite breakableSprite)
    {
        name = "Piece_" + index;
        spriteRenderer.sprite = breakableSprite;

        originX = transform.localPosition.x - .9375f;
        originY = transform.localPosition.y + .0625f;

        newX = originX + (spriteRenderer.sprite.rect.x * .0625f);
        newY = originY + (spriteRenderer.sprite.rect.y * .0625f);

        transform.localPosition = new Vector3(newX, newY, transform.position.z);
    }
}
