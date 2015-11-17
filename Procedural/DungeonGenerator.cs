using Matcha.Unity;
using Rotorz.Tile;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DungeonGenerator : DungeonBehaviour
{
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
		ChooseExitType();



		// CarveRandomRooms(numberOfRooms);
		// CarveHalls();
		// CarveCrawlspaces();
		// AssessForStairs();
		RefreshAllTiles();
	}
}
