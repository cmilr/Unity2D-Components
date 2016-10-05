using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class BreakablePiece : BaseBehaviour
{
	private float originX;
	private float originY;
	private float newX;
	private float newY;
	private new Transform transform;
	private new Rigidbody2D rigidbody2D;
	private SpriteRenderer spriteRenderer;
	private Sequence fadeInInstant;
	private Sequence fadeOut;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);

		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
	}

	void Start()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(rigidbody2D);

		rigidbody2D.mass = Rand.Range(.5f, 20f);
		rigidbody2D.drag = Rand.Range(0f, .5f);

		// cache & pause tween sequences.
		(fadeInInstant   = MFX.Fade(spriteRenderer, 1f, 0f, 0f)).Pause();
		(fadeOut         = MFX.Fade(spriteRenderer, 0f, 0f, 3f)).Pause();
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
		transform.localPosition = new Vector3(newX, newY, Rand.Range(-1f, 0f));
	}

	public void CountDown()
	{
		fadeInInstant.Restart();

		Invoke("FadeOut", Rand.Range(MIN_BEFORE_FADE, MAX_BEFORE_FADE));
	}

	void FadeOut()
	{
		fadeOut.Restart();
	}
}
