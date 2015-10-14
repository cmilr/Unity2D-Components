using UnityEngine;
using System;
using System.Collections.Generic;
using Rotorz.Tile;
using Matcha.Lib;
using Matcha.Rotorz;

public class LevelGenerator : CacheBehaviour {

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
        ShowBounds(rooms);
        RefreshAllTiles();
    }

    void PaintBaseTiles() {

        map.BeginBulkEdit_M();

            for (int r = 0; r < mapRows; r++)
            {
                for (int c = 0; c < mapColumns; c++)
                {
                    brush.Paint(map, r, c);
                }
            }

        map.EndBulkEdit_M();
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
        room.width  = (int) MLib.NextGaussian(8f, 8f, 2f, 50f);
        room.height = (int) MLib.NextGaussian(4f, 8f, 8f, 20f);

        // round up to nearest even number
        room.width = MLib.RoundToDivFour(room.width);
        room.height = MLib.RoundToDivFour(room.height);
    }

    void PaintRoomRandomly(ProcRoom room)
    {
        bool successful = false;
        int attempts = 0;

        map.BeginBulkEdit_M();

        while (!successful && attempts < 5)
        {
            // get random coordinates to attempt to place new room
            // int randX = (int) MLib.NextGaussian(mapColumns / 2, mapColumns / 2, mapMarginX, mapColumns);
            // int randY = (int) MLib.NextGaussian(mapRows / 2, mapRows / 2, mapMarginY, mapRows);
            int randX = UnityEngine.Random.Range(mapMarginX, mapColumns - mapMarginX);
            int randY = UnityEngine.Random.Range(mapMarginY, mapRows - mapMarginY);

            // convert coordinates to divisors of 4; keeps elements from being too close to each other
            int originX = MLib.RoundToDivFour(randX);
            int originY = MLib.RoundToDivFour(randY);

            // check that room will fit within map bounds
            if (RoomInBounds(originX, originY, room) &&
               !TouchingRooms(originX, originY, room))
            {
                // paint room
                for (int x = 0; x < room.width; x++)
                {
                    for (int y = 0; y < room.height; y++)
                    {
                        map.EraseTile_M(originX + x, originY + y);
                    }
                }

                // with room succesfully placed, set origin then add to List
                room.originX = originX;
                room.originY = originY;
                rooms.Add(room);

                successful = true;
            }

            attempts++;
        }

        map.EndBulkEdit_M();
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
                if (map.GetTile(y, x) == null)
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
            Debug.Log("Room " + i +": x." + room.originX +", y." + room.originY);
            i++;
        }
    }

    void CarveHalls()
    {
        map.BeginBulkEdit_M();

            int originX;
            int originY;
            int x;
            int y;

            foreach (ProcRoom room in rooms)
            {
                ProcHall hall = new ProcHall();

                // get random direction
                int rand = UnityEngine.Random.Range(0, 2);
                // direction = (rand == 0 ? RIGHT : LEFT);
                direction = RIGHT;

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

                y = 0;

                // get random height to make halls either two or four tiles tall
                int rand2 = UnityEngine.Random.Range(0, 10);
                int i = (rand2 == 0 ? 2 : 4);

                while (map.GetTile(originY, originX + x) != null &&
                       TileInBounds(originX + x, originY))
                {
                    if (i == 2)
                    {
                        map.EraseTile(originY, originX + x);
                        map.EraseTile(originY - 1, originX + x);
                    }
                    else
                    {
                        map.EraseTile(originY, originX + x);
                        map.EraseTile(originY - 1, originX + x);
                        map.EraseTile(originY - 2, originX + x);
                        map.EraseTile(originY - 3, originX + x);
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

        map.EndBulkEdit_M();
    }

    void AssessForStairs()
    {
        foreach (ProcHall hall in halls)
        {
            int rand = UnityEngine.Random.Range(0, 2);

            if (rand == 0)
            {
                if (map.GetTile(hall.BottomRightY() + 1, hall.BottomRightX() + 1) == null &&
                    TileInBounds(hall.BottomRightX() + 1, hall.BottomRightY() + 1) &&
                    map.GetTile(hall.BottomRightY() + 1, hall.BottomRightX()) != null)
                {
                    BuildStairs(RIGHT, hall.BottomRightX() + 1, hall.BottomRightY() + 1);
                }

                hallOriginBrush.Paint(map, hall.BottomRightY() + 1, hall.BottomRightX() + 1);
            }
        }
    }

    void BuildStairs(int buildDirection, int originX, int originY)
    {
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
                        brush.Paint(map, originY + y, originX + x);
                        // erase walls to the right of stairs
                        map.EraseTile(originY + y, originX + x + 1);
                        map.EraseTile(originY + y, originX + x + 2);
                        map.EraseTile(originY + y, originX + x + 3);
                        map.EraseTile(originY + y, originX + x + 4);
                    }

                    // backfill stairs by one tile
                    brush.Paint(map, originY + y, originX -1);
               }

               y++;
            }
            else
            {
                for (int x = 0; x < y; x++)
                {
                    if (TileInBounds(originX - x, originY + y))
                    {
                        brush.Paint(map, originY + y, originX - x);
                    }
                }

                y++;
            }

        }
    }

    int DistanceToGround(int originX, int originY)
    {
        int y = 0;
        while (map.GetTile(originY + y, originX) == null &&
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

    void PlaceRandomSteps()
    {
        int steps          = 0;
        int x              = 0;
        int y              = 0;

        map.BeginBulkEdit_M();

            foreach (ProcRoom room in rooms)
            {
                if (room.height > 4)
                {
                    steps = (int) MLib.NextGaussian(5f, 3f);

                    for (int i = 0; i < steps; i++)
                    {
                        x = (int) MLib.NextGaussian
                                    (room.originX, room.originX / 2, room.originX + 1, room.originX + room.width -1);
                        y = (int) MLib.NextGaussian
                                    (room.originY, room.originY, room.originY + 1, room.originY + room.height - 1);

                        // if (WithinRoomBounds(room, room.originX + x, room.originY - y))
                        // {
                            stepBrush.Paint(map, y, x);
                        // }
                    }
                }
            }

        map.EndBulkEdit_M();
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
            testBrush.Paint(map, element.BottomRightY(), element.BottomRightX());
            testBrush.Paint(map, element.TopRightY(), element.TopRightX());
            testBrush.Paint(map, element.BottomLeftY(), element.BottomLeftX());
            testBrush.Paint(map, element.TopLeftY(), element.TopLeftX());
            hallOriginBrush.Paint(map, element.originY, element.originX);
        }
    }

    void ShowBounds(List<ProcHall> list)
    {
        foreach (ProcHall element in list)
        {
            testBrush.Paint(map, element.BottomRightY(), element.BottomRightX());
            testBrush.Paint(map, element.TopRightY(), element.TopRightX());
            testBrush.Paint(map, element.BottomLeftY(), element.BottomLeftX());
            testBrush.Paint(map, element.TopLeftY(), element.TopLeftX());
            hallOriginBrush.Paint(map, element.originY, element.originX);
        }
    }

    void RefreshAllTiles()
    {
        map.BeginBulkEdit_M();

            for (int r = 0; r < mapRows; r++)
            {
                for (int c = 0; c < mapColumns; c++)
                {
                    map.RefreshSurroundingTiles(r, c);
                }
            }

        map.EndBulkEdit_M();
    }
}

















// void CalculateTileSystemSize()
// {
//     mapSize = new Vector3(
//         map.ColumnCount * map.CellSize.x,
//         map.RowCount * map.CellSize.y,
//         map.CellSize.z
//     );
// }