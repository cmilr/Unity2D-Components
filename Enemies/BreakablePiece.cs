using UnityEngine;
using System.Collections;

public class BreakablePiece : CacheBehaviour {

    private float originX;
    private float originY;
    private float newX;
    private float newY;

    void Start()
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.mass = UnityEngine.Random.Range(.5f, 20f);
        rigidbody.drag = UnityEngine.Random.Range(0f, .5f);
    }

    public void Init(int index, Sprite breakableSprite)
    {
        name = "Piece_" + index;
        spriteRenderer.sprite = breakableSprite;

        originX = transform.localPosition.x - .9375f;
        originY = transform.localPosition.y + .0625f;

        newX = originX + (spriteRenderer.sprite.rect.x * .0625f);
        newY = originY + (spriteRenderer.sprite.rect.y * .0625f);

        // set position, and randomize z to reduce z-fighting
        transform.localPosition = new Vector3(newX, newY, UnityEngine.Random.Range(-1f, 0f));
    }
}
