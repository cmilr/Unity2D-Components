using UnityEngine;
using System;
using System.Collections.Generic;
using Rotorz.Tile;
using Matcha.Lib;
using Matcha.Tiles;

public class DungeonGenerator : DungeonBehaviour
{
	int numberOfRooms = 50;

	void Awake()
	{
		// fields inherited from DungeonBehaviour
		mapMarginX     = 12;
		mapMarginY     = 12;
		roomMarginX    = 4;
		roomMarginY    = 4;

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
		CarveRoom();

		// install hall or stairs
		int structure = UnityEngine.Random.Range(0, 1);

		if (structure == 0)
		{
			AssessForStairsNew();
		}
		else
		{
			AssessForStairsNew();
		}

		// CarveRandomRooms(numberOfRooms);
		// CarveHalls();
		// CarveCrawlspaces();
		// AssessForStairs();
		RefreshAllTiles();
	}
}