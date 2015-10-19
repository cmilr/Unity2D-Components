using UnityEngine;
using System.Collections;
using DG.Tweening;


public class MovingPlatform : CacheBehaviour
{
	public enum Direction { right, left, up, down };
	public Direction direction;

	[Tooltip("Distance for platform to move, measured in tiles.")]
	public float distance  = 1.0f;
	[Tooltip("How many seconds to travel the above distance?")]
	public float time      = 1.0f;
	[Tooltip("How long should the platform pause at each end?")]
	public float pauseTime = .1f;

	private int layer;
	private bool fastPlatform;

	void Start()
	{
		CheckPlatformSpeed();
		transform.DOKill();

		switch (direction)
		{
		case Direction.right:
			DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveX(distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
			break;

		case Direction.left:
			DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveX(-distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
			break;

		case Direction.up:
			DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveY(distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
			break;

		case Direction.down:
			DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveY(-distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
			break;

		default:
			DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveX(distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == PLAYER_COLLIDER && coll.transform.position.y - .3f > gameObject.transform.position.y)
		{
			coll.transform.parent = gameObject.transform;
		}
		else if (layer != PLAYER_COLLIDER)
		{
			coll.transform.parent = gameObject.transform;
		}

		if (layer == PLAYER_COLLIDER && fastPlatform)
			Messenger.Broadcast<bool>("riding fast platform", true);
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		layer = coll.gameObject.layer;

		coll.transform.parent = null;

		if (layer == PLAYER_COLLIDER && fastPlatform)
			Messenger.Broadcast<bool>("riding fast platform", false);
	}

	// if platform is particularly fast and long, send a message to PlayerState, so PlayerMovement can
	// throttle y-speed to alleviate jitteriness. formula below was attained through experimentation.
	void CheckPlatformSpeed()
	{
		if (direction == Direction.up || direction == Direction.down)
		{
			if (distance > 4 && distance / time >= 5)
				fastPlatform = true;
		}
	}
}
