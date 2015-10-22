using UnityEngine;
using System;
using System.Collections.Generic;
using Rotorz.Tile;
using Matcha.Lib;
using Matcha.Tiles;

public class LevelGenerator : CacheBehaviour
{

	public Brush brush;
	public Brush testBrush;
	public Brush stepBrush;
	public Brush hallOriginBrush;
	public Brush roomOriginBrush;

	private TileSystem map;
	private int mapColumns;
	private int mapRows;
	private int mapMarginX = 10;
	private int mapMarginY = 10;
	private int roomMarginX = 4;
	private int roomMarginY = 4;
	private int numberOfRooms = 40;
	private int direction = RIGHT;
	List<ProcRoom> rooms;
	List<ProcHall> halls;


	void Awake()
	{
		map        = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();
		mapColumns = map.ColumnCount;
		mapRows    = map.RowCount;
		rooms      = new List<ProcRoom>();
		halls      = new List<ProcHall>();

		GenerateRandomDungeons();
	}

	void GenerateRandomDungeons()
	{
		PaintBaseTiles();
		CarveRandomRooms();
		CarveHalls();
		AssessForStairs();
		RefreshAllTiles();
	}

	void PaintBaseTiles()
	{

		map.BulkEditBegin();

		for (int x = 0; x < mapColumns; x++)
		{
			for (int y = 0; y < mapRows; y++)
			{
				brush.PaintTile(map, x, y);
			}
		}

		map.BulkEditEnd();
	}

	void CarveRandomRooms()
	{
		for (int i = 0; i < numberOfRooms; i++)
		{
			ProcRoom roomToDraw = new ProcRoom();

			GetRoom(roomToDraw);
			PaintRoomRandomly(roomToDraw);
		}
	}

	void GetRoom(ProcRoom room)
	{
		room.width  = (int) M.NextGaussian(8f, 8f, 2f, 50f);
		room.height = (int) M.NextGaussian(4f, 8f, 8f, 20f);

		// round up to nearest even number
		room.width = M.RoundToDivFour(room.width);
		room.height = M.RoundToDivFour(room.height);
	}

	void PaintRoomRandomly(ProcRoom room)
	{
		bool successful = false;
		int attempts = 0;

		map.BulkEditBegin();

		while (!successful && attempts < 5)
		{
			// get random coordinates to attempt to place new room
			// int randX = (int) M.NextGaussian(mapColumns / 2, mapColumns / 2, mapMarginX, mapColumns);
			// int randY = (int) M.NextGaussian(mapRows / 2, mapRows / 2, mapMarginY, mapRows);
			int randX = UnityEngine.Random.Range(mapMarginX, mapColumns - mapMarginX);
			int randY = UnityEngine.Random.Range(mapMarginY, mapRows - mapMarginY);

			// convert coordinates to divisors of 4; keeps elements from being too close to each other
			int originX = M.RoundToDivFour(randX);
			int originY = M.RoundToDivFour(randY);

			// check that room will fit within map bounds
			if (RoomInBounds(originX, originY, room) &&
					!TouchingRooms(originX, originY, room))
			{
				// paint room
				for (int x = 0; x < room.width; x++)
				{
					for (int y = 0; y < room.height; y++)
					{
						map.ClearTile(originX + x, originY + y);
					}
				}

				// with room succesfully placed, set origin then add room to List
				room.originX = originX;
				room.originY = originY;
				rooms.Add(room);

				successful = true;
			}

			attempts++;
		}

		map.BulkEditEnd();
	}

	void CarveHalls()
	{
		map.BulkEditBegin();

		int originX;
		int originY;
		int x;

		foreach (ProcRoom room in rooms)
		{
			ProcHall hall = new ProcHall();

			// get random direction
			int rand = UnityEngine.Random.Range(0, 2);
			direction = (rand == 0 ? RIGHT : LEFT);

			// set origin point
			if (direction == RIGHT)
			{
				originX = room.BottomRightX();
				originY = room.BottomRightY();
				x = 1;
			}
			else
			{
				originX = room.BottomLeftX();
				originY = room.BottomLeftY();
				x = -1;
			}

			// get random height to make halls either two or four tiles tall
			int rand2 = UnityEngine.Random.Range(0, 10);
			int i = (rand2 == 0 ? 2 : 4);

			while (map.GetTileInfo(originX + x, originY) != null &&
					TileInBounds(originX + x, originY))
			{
				if (i == 2)
				{
					map.ClearTile(originX + x, originY);
					map.ClearTile(originX + x, originY - 1);
				}
				else
				{
					map.ClearTile(originX + x, originY);
					map.ClearTile(originX + x, originY - 1);
					map.ClearTile(originX + x, originY - 2);
					map.ClearTile(originX + x, originY - 3);
				}

				x = (direction == RIGHT ? x + 1 : x - 1);
			}

			// with hall succesfully placed, set its origin, width, and height, then add to List
			hall.width   = Math.Abs(x) - 1;
			hall.height  = i;
			hall.originY = originY - (i - 1);
			hall.originX = (direction == RIGHT ? originX + 1 : originX - hall.width);

			halls.Add(hall);
		}

		map.BulkEditEnd();
	}

	void AssessForStairs()
	{
		foreach (ProcHall hall in halls)
		{
			int rand = UnityEngine.Random.Range(0, 3);

			if (rand == 0)
			{
				if (map.GetTileInfo(hall.BottomRightX() + 1, hall.BottomRightY() + 1) == null &&
						TileInBounds(hall.BottomRightX() + 1, hall.BottomRightY() + 1) &&
						map.GetTileInfo(hall.BottomRightX(), hall.BottomRightY() + 1) != null)
				{
					BuildStairs(RIGHT, hall.BottomRightX() + 1, hall.BottomRightY() + 1);
				}
			}
			else if (rand == 1)
			{
				if (map.GetTileInfo(hall.BottomLeftX() - 1, hall.BottomLeftY() + 1) == null &&
						TileInBounds(hall.BottomLeftX() - 1, hall.BottomLeftY() + 1) &&
						map.GetTileInfo(hall.BottomLeftX(), hall.BottomLeftY() + 1) != null)
				{
					BuildStairs(LEFT, hall.BottomLeftX() - 1, hall.BottomLeftY() + 1);
				}

				hallOriginBrush.PaintTile(map, hall.BottomLeftX() - 1, hall.BottomRightY() + 1);
			}
		}
	}

	void BuildStairs(int buildDirection, int originX, int originY)
	{
		map.BulkEditBegin();

		int y = 0;

		while (map.GetTile(originY + y, originX) == null &&
				TileInBounds(originX, originY + y))
		{
			if (buildDirection == RIGHT)
			{
				for (int x = 0; x < y; x++)
				{
					if (TileInBounds(originX + x, originY + y))
					{
						// build stairs
						brush.PaintTile(map, originX + x, originY + y);
						// erase walls to the right of stairs
						map.ClearTile(originX + x + 1, originY + y);
						map.ClearTile(originX + x + 2, originY + y);
						map.ClearTile(originX + x + 3, originY + y);
						map.ClearTile(originX + x + 4, originY + y);
					}

					// backfill stairs by one tile
					brush.PaintTile(map, originX - 1, originY + y);
				}

				y++;
			}
			else
			{
				for (int x = 0; x < y; x++)
				{
					if (TileInBounds(originX - x, originY + y))
					{
						// build stairs
						brush.PaintTile(map, originX - x, originY + y);
						// erase walls to the right of stairs
						map.ClearTile(originX - x - 1, originY + y);
						map.ClearTile(originX - x - 2, originY + y);
						map.ClearTile(originX - x - 3, originY + y);
						map.ClearTile(originX - x - 4, originY + y);
					}

					// backfill stairs by one tile
					brush.PaintTile(map, originX + 1, originY + y);
				}

				y++;
			}

		}

		map.BulkEditEnd();
	}

	int DistanceToGround(int originX, int originY)
	{
		int y = 0;
		while (map.GetTileInfo(originX, originY + y) == null &&
				TileInBounds(originX, originY + y))
		{
			y++;
		}

		return y;
	}

	bool TileInBounds(int originX, int originY)
	{
		if (originX < (mapColumns - mapMarginX) &&
			originX > (mapMarginX) &&
			originY < (mapRows - mapMarginY) &&
			originY > (mapMarginY))
		{
			return true;
		}

		return false;
	}

	bool RoomInBounds(int originX, int originY, ProcRoom room)
	{
		if (originX + room.width < (mapColumns - mapMarginX) &&
				originY + room.height < (mapRows - mapMarginY))
		{
			return true;
		}

		return false;
	}

	bool TouchingRooms(int originX, int originY, ProcRoom room)
	{
		// iterate through each potential tile placement
		for (int x = originX - roomMarginX; x < room.width + originX + roomMarginX; x++)
		{
			for (int y = originY - roomMarginY; y < room.height + originY + roomMarginY; y++)
			{
				// if a room has already been carved out here, return true
				if (map.GetTileInfo(x, y) == null)
				{
					return true;
				}
			}
		}

		return false;
	}

	void LogRooms()
	{
		int i = 0;

		foreach (ProcRoom room in rooms)
		{
			Debug.Log("Room " + i + ": x." + room.originX + ", y." + room.originY);
			i++;
		}
	}

	void PlaceRandomSteps()
	{
		int steps          = 0;
		int x              = 0;
		int y              = 0;

		map.BulkEditBegin();

		foreach (ProcRoom room in rooms)
		{
			if (room.height > 4)
			{
				steps = (int) M.NextGaussian(5f, 3f);

				for (int i = 0; i < steps; i++)
				{
					x = (int) M.NextGaussian
						(room.originX, room.originX / 2, room.originX + 1, room.originX + room.width - 1);
					y = (int) M.NextGaussian
						(room.originY, room.originY, room.originY + 1, room.originY + room.height - 1);

					// if (WithinRoomBounds(room, room.originX + x, room.originY - y))
					// {
					stepBrush.PaintTile(map, x, y);
					// }
				}
			}
		}

		map.BulkEditEnd();
	}

	bool WithinRoomBounds(ProcRoom room, int x, int y)
	{
		if ((x >= room.originX && x <= room.width) &&
				(y >= room.originY && y <= room.height))
		{
			return true;
		}

		return false;
	}

	void ShowBounds(List<ProcRoom> list)
	{
		foreach (ProcRoom element in list)
		{
			testBrush.PaintTile(map, element.BottomRightX(), element.BottomRightY());
			testBrush.PaintTile(map, element.TopRightX(), element.TopRightY());
			testBrush.PaintTile(map, element.BottomLeftX(), element.BottomLeftY());
			testBrush.PaintTile(map, element.TopLeftX(), element.TopLeftY());
			hallOriginBrush.PaintTile(map, element.originX, element.originY);
		}
	}

	void ShowBounds(List<ProcHall> list)
	{
		foreach (ProcHall element in list)
		{
			testBrush.PaintTile(map, element.BottomRightX(), element.BottomRightY());
			testBrush.PaintTile(map, element.TopRightX(), element.TopRightY());
			testBrush.PaintTile(map, element.BottomLeftX(), element.BottomLeftY());
			testBrush.PaintTile(map, element.TopLeftX(), element.TopLeftY());
			hallOriginBrush.PaintTile(map, element.originX, element.originY);
		}
	}

	void RefreshAllTiles()
	{
		map.RefreshTiles();
	}
}