using UnityEngine;
using System.Collections;
using Rotorz.Tile;
using Matcha.Lib;

public class CavernGenerator : CacheBehaviour {

    public Brush brush;

    private TileSystem tileSystem;
    private float mapColumns;
    private float mapRows;
    private int numberOfRooms = 100;

	void Awake()
    {
        tileSystem = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();
        mapColumns = tileSystem.ColumnCount;
        mapRows    = tileSystem.RowCount;

        PaintBaseTiles();
        CarveRandomRooms();
	}

    void PaintBaseTiles() {

        tileSystem.BeginBulkEdit();

            for (int r = 0; r < mapRows; r++)
            {
                for (int c = 0; c < mapColumns; c++)
                {
                    brush.Paint(tileSystem, r, c);
                    tileSystem.RefreshSurroundingTiles(r, c);
                }
            }

        tileSystem.EndBulkEdit();
    }

    void CarveRandomRooms()
    {
        ProcRoom roomToDraw = new ProcRoom();

        for (int i = 0; i < numberOfRooms; i++)
        {
            GetRoom(roomToDraw);
            // Debug.Log("w = " + roomToDraw.width + ". H = " + roomToDraw.height);
            PaintRoom(roomToDraw);
        }
    }

    void GetRoom(ProcRoom room)
    {
        room.width  = (int) M.NextGaussian(20f, 5f, 2f, 50f);
        room.height = (int) M.NextGaussian(5f, 5f, 2f, 50f);
    }

    void PaintRoom(ProcRoom room)
    {
        tileSystem.BeginBulkEdit();

            // get random coordinates to attempt to place new room
            int originX = (int) M.NextGaussian(mapColumns / 2, mapColumns / 2, 2f, mapColumns);
            int originY = (int) M.NextGaussian(mapRows / 2, mapRows / 2, 2f, mapRows);

            // check that room will fit within map bounds
            if (InBounds(originX, originY, room))
            {
                // paint room
                for (int x = originX; x < room.width + originX; x++)
                {
                    for (int y = originY; y < room.height + originY; y++)
                    {
                        tileSystem.EraseTile(y, x);
                        tileSystem.RefreshSurroundingTiles(y, x);
                    }
                }
            }
            else
            {
                PaintRoom(room);
            }

        tileSystem.EndBulkEdit();
    }

    bool InBounds(int x, int y, ProcRoom room)
    {
        if (x + room.width < (mapColumns - 1) && y + room.height < (mapRows - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // void CalculateTileSystemSize()
    // {
    //     tileSystemSize = new Vector3(
    //         tileSystem.ColumnCount * tileSystem.CellSize.x,
    //         tileSystem.RowCount * tileSystem.CellSize.y,
    //         tileSystem.CellSize.z
    //     );
    // }
}
