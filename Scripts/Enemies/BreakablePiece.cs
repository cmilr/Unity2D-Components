using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;

public class BreakablePiece : CacheBehaviour
{
	private float originX;
	private float originY;
	private float newX;
	private float newY;
	private bool alreadyCollided;
	private bool levelCompleted;
	private bool playerDead;

	void Start()
	{
		Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
		rigidbody.mass        = Rand.Range(.5f, 20f);
		rigidbody.drag        = Rand.Range(0f, .5f);
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
		Invoke("FadeOut", Rand.Range(MIN_BEFORE_FADE, MAX_BEFORE_FADE));
	}

	// used when fading naturally
	void FadeOut()
	{
		MFX.Fade(spriteRenderer, 0f, 0f, 3f);
	}

	// use when picked up by player
	void FadeOutFast()
	{
		MFX.Fade(spriteRenderer, 0f, 0f, .15f);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (!alreadyCollided && !levelCompleted && !playerDead)
		{
			int layer = coll.gameObject.layer;

			if (layer == BODY_COLLIDER)
			{
				alreadyCollided = true;

				EventKit.Broadcast<int>("prize collected", 5);

				FadeOutFast();
			}
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("level completed", OnLevelCompleted);
		EventKit.Subscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
	}

	void OnDisable()
	{
		EventKit.Unsubscribe<bool>("level completed", OnLevelCompleted);
		EventKit.Unsubscribe<int, Weapon.WeaponType>("player dead", OnPlayerDead);
	}

	void OnPlayerDead(int hitFrom, Weapon.WeaponType weaponType)
	{
		playerDead = true;
	}

	void OnLevelCompleted(bool status)
	{
		levelCompleted = status;
	}
}
