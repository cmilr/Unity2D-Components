using Matcha.Unity;
using Rotorz.Tile;

public class DungeonGenerator : BaseBehaviour
{
	public Brush stoneBrush;
	public Brush boundsBrush;
	public Brush stepBrush;
	public Brush hallOriginBrush;
	public Brush roomOriginBrush;

	private TileSystem map;
	private int mapColumns;
	private int mapRows;

	void SetTileMapSpecs(TileSystem mapGo)
	{
		map            = mapGo;
		mapColumns     = mapGo.ColumnCount;
		mapRows        = mapGo.RowCount;
	}

	public void FillTileMapWithStone(TileSystem mapGo)
	{

		SetTileMapSpecs(mapGo);
		PaintBaseTiles(stoneBrush);
	}

	void PaintBaseTiles(Brush currentBrush)
	{
		map.BulkEditBegin();

		for (int x = 0; x < mapColumns; x++)
		{
			for (int y = 0; y < mapRows; y++)
			{
				currentBrush.PaintTile(map, x, y);
			}
		}

		map.BulkEditEnd();
	}

	void RefreshAllTiles()
	{
		map.RefreshTiles();
	}
}
