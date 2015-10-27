using UnityEngine;
using System;
using System.Collections.Generic;
using Rotorz.Tile;
using Matcha.Lib;
using Matcha.Tiles;

// all map and tile operations below make use of the Matcha.Tiles extensions
// which extend Rotorz.Tile and act as a safeguard against api changes, etc
public class DungeonBehaviour : CacheBehaviour
{
    public Brush brush;
    public Brush boundsBrush;
    public Brush stepBrush;
    public Brush hallOriginBrush;
    public Brush roomOriginBrush;

    protected TileSystem map;
    protected int mapColumns;
    protected int mapRows;
    protected int mapMarginX;
    protected int mapMarginY;
    protected int roomMarginX;
    protected int roomMarginY;
    protected int direction;
    protected List<ProcSpace> roomList;
    protected List<ProcSpace> hallList;
    protected List<ProcSpace> crawlspaceList;
    protected ProcSpace currentRoom;

    protected void PaintBaseTiles()
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

    protected void CarveRoom()
    {
        ProcSpace roomToDraw = new ProcSpace();

        GetRoom(roomToDraw);
        PaintRoomRandomly(roomToDraw, 100000);
    }

    protected void CarveRandomRooms(int numberOfRooms)
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            ProcSpace roomToDraw = new ProcSpace();

            GetRoom(roomToDraw);
            PaintRoomRandomly(roomToDraw, 5);
        }
    }

    protected void GetRoom(ProcSpace room)
    {
        room.width  = (int) M.NextGaussian(8f, 8f, 2f, 50f);
        room.height = (int) M.NextGaussian(4f, 8f, 8f, 20f);

        // round up to nearest even number
        room.width = M.RoundToDivFour(room.width);
        room.height = M.RoundToDivFour(room.height);
    }

    protected void PaintRoomRandomly(ProcSpace room, int maxAttempts)
    {
        bool successful = false;
        int attempts = 0;

        map.BulkEditBegin();

        while (!successful && attempts < maxAttempts)
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
                roomList.Add(room);
                currentRoom = room;

                successful = true;
            }

            attempts++;
        }

        map.BulkEditEnd();
    }

    protected void CarveHalls()
    {
        map.BulkEditBegin();

        int originX;
        int originY;
        int x;

        foreach (ProcSpace room in roomList)
        {
            ProcSpace hall = new ProcSpace();

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

            // get random height to make hallList either two or four tiles tall
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

            hallList.Add(hall);
        }

        map.BulkEditEnd();
    }

    protected void CarveCrawlspaces()
    {
        map.BulkEditBegin();

        int originX;
        int originY;
        int x;

        foreach (ProcSpace room in roomList)
        {
            int rand2 = UnityEngine.Random.Range(0, 2);

            if (rand2 == 0)
            {
                ProcSpace crawlspace = new ProcSpace();

                direction = RIGHT;

                originX = room.TopRightX();
                originY = room.TopRightY() + 1;
                x = 1;


                // get random height to make hallList either two or four tiles tall
                rand2 = UnityEngine.Random.Range(0, 10);
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
                crawlspace.width   = Math.Abs(x) - 1;
                crawlspace.height  = i;
                crawlspace.originY = originY - (i - 1);
                crawlspace.originX = (direction == RIGHT ? originX + 1 : originX - crawlspace.width);

                crawlspaceList.Add(crawlspace);
            }
        }

        map.BulkEditEnd();
    }

    protected void AssessForStairs()
    {
        foreach (ProcSpace hall in hallList)
        {
            int rand = UnityEngine.Random.Range(0, 2);

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

    protected void AssessForStairsNew()
    {
        int rand = UnityEngine.Random.Range(0, 2);

        if (rand == 0)
        {
            if (TileInBounds(currentRoom.BottomRightX() - 2, currentRoom.BottomRightY() - 4))
            {
                BuildStairsNew(RIGHT, currentRoom.BottomRightX() - 2, currentRoom.BottomRightY() - 4);
            }
        }
        else
        {
            if (TileInBounds(currentRoom.BottomLeftX() + 2, currentRoom.BottomLeftY() - 4))
            {
                BuildStairsNew(LEFT, currentRoom.BottomLeftX() + 2, currentRoom.BottomLeftY() - 4);
            }
        }
    }

    protected void BuildStairsNew(int buildDirection, int originX, int originY)
    {
        map.BulkEditBegin();

        var depth = UnityEngine.Random.Range(12, 21);
        var y = 0;

        for (int i = 0; i < depth; i++)
        {
            // while coordinates are within the map's bounds
            if (TileInBounds(originX, originY + y))
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
                                if (map.GetTileInfo(originX, originY + y) != null )
                                {
                                    brush.PaintTile(map, originX + x, originY + y);
                                }
                                // erase walls to the right of stairs
                                map.ClearTile(originX + x + 1, originY + y);
                                map.ClearTile(originX + x + 2, originY + y);
                                map.ClearTile(originX + x + 3, originY + y);
                                map.ClearTile(originX + x + 4, originY + y);
                            }

                            // // backfill stairs by one tile
                            // brush.PaintTile(map, originX - 1, originY + y);
                            // brush.PaintTile(map, originX - 2, originY + y);
                            break;
                        }

                        case LEFT:
                        {
                            if (TileInBounds(originX - x, originY + y))
                            {
                                // build stairs
                                if (map.GetTileInfo(originX, originY + y) != null )
                                {
                                    brush.PaintTile(map, originX - x, originY + y);
                                }
                                // brush.PaintTile(map, originX - x, originY + y);
                                // erase walls to the left of stairs
                                map.ClearTile(originX - x - 1, originY + y);
                                map.ClearTile(originX - x - 2, originY + y);
                                map.ClearTile(originX - x - 3, originY + y);
                                map.ClearTile(originX - x - 4, originY + y);
                            }

                            // // backfill stairs by one tile
                            // brush.PaintTile(map, originX + 1, originY + y);
                            // brush.PaintTile(map, originX + 2, originY + y);
                            break;
                        }
                    }
                }

                y++;
            }
        }

        map.BulkEditEnd();
    }

    protected void BuildStairs(int buildDirection, int originX, int originY)
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

    protected int DistanceToGround(int originX, int originY)
    {
        int y = 0;
        while (map.GetTileInfo(originX, originY + y) == null &&
                TileInBounds(originX, originY + y))
        {
            y++;
        }

        return y;
    }

    protected bool TileInBounds(int originX, int originY)
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

    protected bool RoomInBounds(int originX, int originY, ProcSpace room)
    {
        if (originX + room.width < (mapColumns - mapMarginX) &&
                originY + room.height < (mapRows - mapMarginY))
        {
            return true;
        }

        return false;
    }

    protected bool TouchingRooms(int originX, int originY, ProcSpace room)
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

    protected void LogRooms()
    {
        int i = 0;

        foreach (ProcSpace room in roomList)
        {
            Debug.Log("Room " + i + ": x." + room.originX + ", y." + room.originY);
            i++;
        }
    }

    protected void PlaceRandomSteps()
    {
        int steps          = 0;
        int x              = 0;
        int y              = 0;

        map.BulkEditBegin();

        foreach (ProcSpace room in roomList)
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

    protected bool WithinRoomBounds(ProcSpace room, int x, int y)
    {
        if ((x >= room.originX && x <= room.width) &&
                (y >= room.originY && y <= room.height))
        {
            return true;
        }

        return false;
    }

    protected void ShowBounds(List<ProcSpace> list)
    {
        foreach (ProcSpace element in list)
        {
            boundsBrush.PaintTile(map, element.BottomRightX(), element.BottomRightY());
            boundsBrush.PaintTile(map, element.TopRightX(), element.TopRightY());
            boundsBrush.PaintTile(map, element.BottomLeftX(), element.BottomLeftY());
            boundsBrush.PaintTile(map, element.TopLeftX(), element.TopLeftY());
            hallOriginBrush.PaintTile(map, element.originX, element.originY);
        }
    }

    protected void RefreshAllTiles()
    {
        map.RefreshTiles();
    }
}