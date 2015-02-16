using UnityEngine;
using System.Collections;
using DG.Tweening;


public class MovingPlatform : EntityBehaviour
{
	public enum Direction { right, left, up, down };
	public Direction direction;
	[Tooltip("Distance for platform to move, measured in tiles.")]
	public float distance = 1.0f;
	[Tooltip("How many seconds to travel the above distance?")]
	public float time = 1.0f;
	[Tooltip("How long should the platform pause at each end?")]
	public float pauseTime = .1f;
	private bool fastPlatform;
	private bool playerJumping;

	void Start()
	{
		base.CacheComponents();
		gameObject.tag = "MovingPlatform";
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
		// do not collide with player when she's on the upward curve of a jump
		if (coll.name == "Player" && playerJumping)
        {
			return;
        }
		else
        {
			coll.transform.parent = gameObject.transform;
        }

		// send message if this is a particularly fast moving vertical platform
		if (fastPlatform && coll.name == "Player")
        {
			Messenger.Broadcast<bool>("riding fast platform", true);
        }
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		coll.transform.parent = null;

		if (fastPlatform && coll.name == "Player")
        {
			Messenger.Broadcast<bool>("riding fast platform", false);
        }
	}

	// if platform is particularly fast and long, send a message to JumpAndRun so it can throttle
	// y-speed to alleviate jitteriness. formula below was attained through experimentation.
	void CheckPlatformSpeed()
	{
		if (direction == Direction.up || direction == Direction.down)
		{
			if (distance > 4 && distance / time > 8)
				fastPlatform = true;
		}
	}

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<bool>( "player jumping (RunAndJump)", OnPlayerJumping);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>( "player jumping (RunAndJump)", OnPlayerJumping);
	}

	// EVENT LISTENERS
	void OnPlayerJumping(bool status)
	{
		playerJumping = status;
	}
}
