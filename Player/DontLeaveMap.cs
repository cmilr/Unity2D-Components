using Matcha.Unity;
using Rotorz.Tile;
using UnityEngine;
using UnityEngine.Assertions;

public class DontLeaveMap : BaseBehaviour
{
	[Tooltip("Amount of sprite allowed to leave the map.")]
	public float leftOffset;
	[Tooltip("Amount of sprite allowed to leave the map.")]
	public float rightOffset;
	[Tooltip("Amount of sprite allowed to leave the map.")]
	public float upperOffset;
	[Tooltip("Amount of sprite allowed to leave the map.")]
	public float lowerOffset;

	private float rightBound;
	private float leftBound;
	private float upperBound;
	private float lowerBound;
	private float spriteWidth;
	private float spriteHeight;
	private TileSystem tileSystem;
	private new Transform transform;
	private new Renderer renderer;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);

		renderer = GetComponent<Renderer>();
		Assert.IsNotNull(renderer);
	}
	
	void Start()
	{		
		tileSystem = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();
		Assert.IsNotNull(tileSystem);
		
		spriteWidth  = renderer.bounds.size.x;
		spriteHeight = renderer.bounds.size.y;

		var tileSystemSize = new Vector3(
			tileSystem.ColumnCount * tileSystem.CellSize.x,
			tileSystem.RowCount * tileSystem.CellSize.y,
			tileSystem.CellSize.z
		);

		leftBound  = 0f;
		rightBound = tileSystemSize.x;
		lowerBound = -(tileSystemSize.y);
		upperBound = 0f;
	}

	void LateUpdate()
	{
		// check left bound.
		if (transform.position.x - (spriteWidth / 2 - leftOffset) < leftBound) {
			transform.SetPositionX(leftBound + (spriteWidth / 2 - leftOffset));
		}

		// check right bound.
		if (transform.position.x + (spriteWidth / 2 - rightOffset) > rightBound) {
			transform.SetPositionX(rightBound - (spriteWidth / 2 - rightOffset));
		}

		// check upper bound.
		if (transform.position.y + (spriteHeight - upperOffset) > upperBound) {
			transform.SetPositionY(upperBound - (spriteHeight - upperOffset));
		}

		// check lower bound.
		if (transform.position.y - lowerOffset < lowerBound)
		{
			transform.SetPositionY(lowerBound - lowerOffset);
		}
	}
}
