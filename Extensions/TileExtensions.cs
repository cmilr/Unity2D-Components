using Rotorz.Tile;

namespace Matcha.Tiles {

    // extensions to the Rotorz api, mostly for switching x & y coordinates,
    // but also acts as an interface to guarantee against future api changes

    public static class TileExtensions
    {
        public static void BeginBulkEdit_M(this TileSystem map)
        {
            map.BeginBulkEdit();
        }

        public static void EndBulkEdit_M(this TileSystem map)
        {
            map.EndBulkEdit();
        }

        public static bool EraseTile_M(this TileSystem map, int x, int y)
        {
            return map.EraseTile(y, x);
        }

        public static TileData GetTile_M(this TileSystem map, int x, int y)
        {
            return map.GetTile(y, x);
        }

        public static void RefreshSurroundingTiles_M(this TileSystem map, int x, int y)
        {
            map.RefreshSurroundingTiles(y, x);
        }

        public static void RefreshAllTiles_M(this TileSystem map)
        {
            map.RefreshAllTiles();
        }

        public static TileData Paint_M(this Brush brush, TileSystem map, int x, int y)
        {
            return brush.Paint(map, y, x);
        }
    }
}
