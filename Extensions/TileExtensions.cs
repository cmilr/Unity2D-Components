//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using Rotorz.Tile;

namespace Matcha.Tiles {

    // This set of methods extends the most excellent Rotorz api. I built this set because,
    // while I love Rotorz, I find their method paramaters frustrating to use. Specifically,
    // I tend to send x coordinates first in my params, followed by y coordinates; while
    // Rotorz transposes this. It has lead to many crazy-making bugs in my code.
    //
    // These extensions also act as an interface to guarantee against future api changes, etc.

    public static class TileExtensions
    {
        public static void BulkEditBegin(this TileSystem map)
        {
            map.BeginBulkEdit();
        }

        public static void BulkEditEnd(this TileSystem map)
        {
            map.EndBulkEdit();
        }

        public static bool ClearTile(this TileSystem map, int x, int y)
        {
            return map.EraseTile(y, x);
        }

        public static TileData GetTileInfo(this TileSystem map, int x, int y)
        {
            return map.GetTile(y, x);
        }

        public static void RefreshNearbyTiles(this TileSystem map, int x, int y)
        {
            map.RefreshSurroundingTiles(y, x);
        }

        public static void RefreshTiles(this TileSystem map)
        {
            map.RefreshAllTiles();
        }

        public static TileData PaintTile(this Brush brush, TileSystem map, int x, int y)
        {
            return brush.Paint(map, y, x);
        }
    }
}
