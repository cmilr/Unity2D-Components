// This set of methods extends the most excellent Rotorz api. I built this set because,
// while I love Rotorz, I find its method paramaters frustrating to use. Specifically,
// I tend to send x coordinates first in my params, followed by y coordinates; while
// Rotorz transposes this. It has lead to many crazy-making bugs in my code.
//
// These extensions also act as an interface to guarantee against future api changes, etc.

using Rotorz.Tile;
using System;
using UnityEngine;

namespace Matcha.Unity
{
	public static class TileExtensions
	{
		public static void BulkEditBegin(this TileSystem map)
		{
			map.BeginBulkEdit();
		}

		public static void BulkEditEnd(this TileSystem map)
		{
			map.EndBulkEdit();
		}

		public static bool ClearTile(this TileSystem map, int x, int y)
		{
			return map.EraseTile(y, x);
		}

		public static TileData GetTileInfo(this TileSystem map, int x, int y)
		{
			return map.GetTile(y, x);
		}

		public static void RefreshNearbyTiles(this TileSystem map, int x, int y)
		{
			map.RefreshSurroundingTiles(y, x);
		}

		public static void RefreshTiles(this TileSystem map)
		{
			map.RefreshAllTiles();
		}

		public static TileData PaintTile(this Brush brush, TileSystem map, int x, int y)
		{
			return brush.Paint(map, y, x);
		}

		// transform extensions for checking nearby tile data
		public static GameObject GetTileInfo(this Transform transform, TileSystem tileSystem, int xDirection, int yDirection)
		{
			// grabs tile info at a given transform:
			// x = 0, y = 0 will get you the tile the transform occupies
			// plus or minus x or y will get you tiles backwards, forwards, up or down

			var convertedX = (int)Math.Floor(transform.position.x);
			var convertedY = (int)Math.Ceiling(Math.Abs(transform.position.y));

			TileData tile = tileSystem.GetTile(convertedY + yDirection, convertedX + xDirection);

			if (tile != null)
			{
				return tile.gameObject;
			}

			return null;
		}

		public static GameObject GetTileBelow(this Transform transform, TileSystem tileSystem, int direction)
		{
			var convertedX = (int)Math.Floor(transform.position.x);
			var convertedY = (int)Math.Floor(Math.Abs(transform.position.y));

			TileData tile = tileSystem.GetTile(convertedY, convertedX + direction);

			if (tile != null)
			{
				return tile.gameObject;
			}

			return null;
		}
	}
}
