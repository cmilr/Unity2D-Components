using Matcha.Unity;
using Rotorz.Tile;
using System.Collections.Generic;
using System;
using UnityEngine;

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
	private int mapMarginX    = 10;
	private int mapMarginY    = 10;
	private int roomMarginX   = 4;
	private int roomMarginY   = 4;
	private int numberOfRooms = 50;
	private int direction     = RIGHT;
	List<ProcRoom> rooms;
	List<ProcHall> halls;
	List<ProcHall> crawlways;

	void Continue()
	{
		mapColumns = map.ColumnCount;
		mapRows    = map.RowCount;
		rooms      = new List<ProcRoom>();
		halls      = new List<ProcHall>();
		crawlways  = new List<ProcHall>();

		GenerateRandomDungeons();
	}

	void GenerateRandomDungeons()
	{
		PaintBaseTiles();
		CarveRandomRooms();
		CarveHalls();
		CarveCrawlways();
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
		room.width  = (int) Rand.GaussianDivFour(8, 8, 2, 50);
		room.height = (int) Rand.GaussianDivFour(4, 8, 8, 20);
	}

	void PaintRoomRandomly(ProcRoom room)
	{
		bool successful = false;
		int attempts = 0;

		map.BulkEditBegin();

		while (!successful && attempts < 5)
		{
			// random ints divisible by four 4 — keeps some distance between elements
			int originX = Rand.RangeDivFour(mapMarginX, mapColumns - mapMarginX);
			int originY = Rand.RangeDivFour(mapMarginY, mapRows - mapMarginY);

			// check that room will fit within map bounds
			if (RoomInBounds(originX, originY, room) && !TouchingRooms(originX, originY, room))
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
			int rand = Rand.Range(0, 1);
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
			int rand2 = Rand.Range(0, 10);
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

	void CarveCrawlways()
	{
		map.BulkEditBegin();

		int originX;
		int originY;
		int x;

		foreach (ProcRoom room in rooms)
		{
			int rand2 = Rand.Range(0, 1);

			if (rand2 == 0)
			{
				ProcHall crawlway = new ProcHall();

				direction = RIGHT;

				originX = room.TopRightX();
				originY = room.TopRightY() + 1;
				x = 1;


				// get random height to make halls either two or four tiles tall
				rand2 = Rand.Range(0, 10);
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
						// map.ClearTile(originX + x, originY - 2);
						// map.ClearTile(originX + x, originY - 3);
					}

					x = (direction == RIGHT ? x + 1 : x - 1);
				}

				// with hall succesfully placed, set its origin, width, and height, then add to List
				crawlway.width   = Math.Abs(x) - 1;
				crawlway.height  = i;
				crawlway.originY = originY - (i - 1);
				crawlway.originX = (direction == RIGHT ? originX + 1 : originX - crawlway.width);

				crawlways.Add(crawlway);
			}
		}

		map.BulkEditEnd();
	}

	void AssessForStairs()
	{
		foreach (ProcHall hall in halls)
		{
			int rand = Rand.Range(0, 1);

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

		// while coordinates are blank, and within the map's bounds
		while (map.GetTile(originY + y, originX) == null &&
				TileInBounds(originX, originY + y))
		{
			for (int x = 0; x < y; x++)
			{
				switch (buildDirection)
				{
					case RIGHT:
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
						brush.PaintTile(map, originX - 2, originY + y);
						break;
					}

					case LEFT:
					{
						if (TileInBounds(originX - x, originY + y))
						{
							// build stairs
							brush.PaintTile(map, originX - x, originY + y);
							// erase walls to the left of stairs
							map.ClearTile(originX - x - 1, originY + y);
							map.ClearTile(originX - x - 2, originY + y);
							map.ClearTile(originX - x - 3, originY + y);
							map.ClearTile(originX - x - 4, originY + y);
						}

						// backfill stairs by one tile
						brush.PaintTile(map, originX + 1, originY + y);
						brush.PaintTile(map, originX + 2, originY + y);
						break;
					}
				}
			}

			y++;
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
				steps = (int) Rand.Gaussian(5f, 3f);

				for (int i = 0; i < steps; i++)
				{
					x = (int) Rand.Gaussian
								(room.originX, room.originX / 2, room.originX + 1, room.originX + room.width - 1);
					y = (int) Rand.Gaussian
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

	void OnTileSystemAnnounced(TileSystem incomingTileSystem)
	{
		map = incomingTileSystem;

		Continue();
	}

	void OnEnable()
	{
		EventKit.Subscribe<TileSystem>("tilesystem announced", OnTileSystemAnnounced);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<TileSystem>("tilesystem announced", OnTileSystemAnnounced);
	}
}
