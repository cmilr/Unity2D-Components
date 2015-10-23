using UnityEngine;
using System;
using System.Collections.Generic;
using Rotorz.Tile;
using Matcha.Lib;
using Matcha.Tiles;

public class DungeonGenerator : DungeonBehaviour
{
	void Awake()
	{
		mapMarginX    = 10;
		mapMarginY    = 10;
		roomMarginX   = 4;
		roomMarginY   = 4;
		numberOfRooms = 50;

		map            = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();
		mapColumns     = map.ColumnCount;
		mapRows        = map.RowCount;
		roomList       = new List<ProcSpace>();
		hallList       = new List<ProcSpace>();
		crawlspaceList = new List<ProcSpace>();

		GenerateRandomDungeons();
	}

	void GenerateRandomDungeons()
	{
		PaintBaseTiles();

		// CarveRandomRooms();
		// CarveHalls();
		// CarveCrawlspaces();
		// AssessForStairs();
		RefreshAllTiles();
	}
}