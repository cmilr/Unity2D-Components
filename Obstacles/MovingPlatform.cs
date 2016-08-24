using DG.Tweening;
using UnityEngine.Assertions;
using UnityEngine;

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

	void Start()
	{
		transform.DOKill();

		switch (direction)
		{
			case Direction.right:
			{
				DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveX(distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
				break;
			}

			case Direction.left:
			{
				DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveX(-distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
				break;
			}

			case Direction.up:
			{
				DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveY(distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
				break;
			}

			case Direction.down:
			{
				DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
				.AppendInterval(pauseTime)
				.Append(transform.DOMoveY(-distance, time).SetRelative().SetEase(Ease.InOutQuad))
				.AppendInterval(pauseTime);
				break;
			}

			default:
			{
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		if (layer == PLAYER_COLLIDER)
		{
			coll.transform.parent = gameObject.transform;
		}
		else if (layer != PLAYER_COLLIDER)
		{
			coll.transform.parent = gameObject.transform;
		}
<<<<<<< HEAD

		if (layer == PLAYER_COLLIDER && fastPlatform) {
			EventKit.Broadcast<bool>("player riding fast platform", true);
		}
=======
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

<<<<<<< HEAD
		coll.transform.parent = null;

		if (layer == PLAYER_COLLIDER && fastPlatform) {
			EventKit.Broadcast<bool>("player riding fast platform", false);
=======
		if (layer == PLAYER_COLLIDER)
		{
			coll.transform.parent = gameObject.transform;
		}
		else if (layer != PLAYER_COLLIDER)
		{
			coll.transform.parent = gameObject.transform;
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		layer = coll.gameObject.layer;

		coll.transform.parent = null;
	}
}
