using Matcha.Unity;
using Rotorz.Tile;
using UnityEngine;

public class DontLeaveMap : CacheBehaviour
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

	void Start()
	{
		tileSystem   = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();
		spriteWidth  = renderer.bounds.size.x;
		spriteHeight = renderer.bounds.size.y;

		Vector3 tileSystemSize = new Vector3(
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
			EventKit.Broadcast<int, Weapon.WeaponType>("player dead", -1, Weapon.WeaponType.OutOfBounds);
		}
	}
}
