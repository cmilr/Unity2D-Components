using UnityEngine;
using System.Collections;

public class BreakablePiece : CacheBehaviour {

    private float xRectPosition;
    private float yRectPosition;

    void Start()
    {
        Invoke("SetPosition", 1f);
    }

    public void Init(Sprite breakableSprite)
    {
        // name = "Piece_" + index;
        spriteRenderer.sprite = breakableSprite;
        // xRectPosition = spriteRenderer.sprite.rect.x;
    }

    void SetPosition()
    {
        transform.position = new Vector3(0f, 0f, 0f);
    }
}
