using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class MovingPlatform : BaseBehaviour
{
	public enum Direction { Invalid, Right, Left, Up, Down };
	public Direction direction;
	
	[Tooltip("Distance for platform to move, measured in tiles.")]
	public float distance  = 1.0f;
	[Tooltip("How many seconds to travel the above distance?")]
	public float time      = 1.0f;
	[Tooltip("How long should the platform pause at each end?")]
	public float pauseTime = .1f;

	private int layer;
	private new Transform transform;
	private Sequence movingPlatformTween;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);

		Assert.IsFalse(direction == Direction.Invalid,
			("Invalid direction @ " + gameObject));
	}
	
	void Start()
	{
		// cache & pause tweens.
		switch (direction)
		{
			case Direction.Right:
				(movingPlatformTween = MFX.MoveBackAndForthHorizontal(pauseTime, transform, distance, time)).Pause();
				break;
			case Direction.Left:
				(movingPlatformTween = MFX.MoveBackAndForthHorizontal(pauseTime, transform, -distance, time)).Pause();
				break;
			case Direction.Up:
				(movingPlatformTween = MFX.MoveBackAndForthVertical(pauseTime, transform, distance, time)).Pause();
				break;
			case Direction.Down:
				(movingPlatformTween = MFX.MoveBackAndForthVertical(pauseTime, transform, -distance, time)).Pause();
				break;
			default:
				Assert.IsTrue(false, ("Direction missing from switch @ " + gameObject));
				break;
		}

		movingPlatformTween.Restart();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == PLAYER_DEFAULT_LAYER)
		{
			coll.transform.parent = gameObject.transform;
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == PLAYER_DEFAULT_LAYER)
		{
			coll.transform.parent = gameObject.transform;
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		coll.transform.parent = null;
	}
}
