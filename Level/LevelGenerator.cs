using UnityEngine;
using System.Collections;
using Rotorz.Tile;

public class LevelGenerator : CacheBehaviour {

    public Brush brush;

    private TileSystem tileSystem;
    private float columns;
    private float rows;

	void Start()
    {
        tileSystem = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();

        columns = tileSystem.ColumnCount;

        Debug.Log(columns);

        rows = tileSystem.RowCount;

        Debug.Log(rows);

	}

    void PaintBaseTiles() {
        // tileSystem.BeginBulkEdit();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    brush.Paint(tileSystem, r, c);
                    tileSystem.RefreshSurroundingTiles(r, c);
                }
            }

        // tileSystem.EndBulkEdit();
    }

    // void PaintExampleTiles() {
    //     tileSystem.BeginBulkEdit();
    //         PaintTwoTiles(5, 5);
    //         PaintTwoTiles(6, 5);
    //     tileSystem.EndBulkEdit();
    // }

    // void Update()
    // {
    //     // Find mouse position in world space
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     // Nearest point where ray intersects tile system
    //     TileIndex ti = tileSystem.ClosestTileIndexFromRay(ray);

    //     if (Input.GetMouseButtonDown(0)) {
    //         // Paint with left mouse button
    //         brush.Paint(tileSystem, ti);
    //         tileSystem.RefreshSurroundingTiles(ti);
    //     }
    //     else if (Input.GetMouseButtonDown(1)) {
    //         // Erase with right mouse button
    //         tileSystem.EraseTile(ti);
    //         tileSystem.RefreshSurroundingTiles(ti);
    //     }
    // }

    // void CalculateTileSystemSize()
    // {
    //     tileSystemSize = new Vector3(
    //         tileSystem.ColumnCount * tileSystem.CellSize.x,
    //         tileSystem.RowCount * tileSystem.CellSize.y,
    //         tileSystem.CellSize.z
    //     );
    // }
}
