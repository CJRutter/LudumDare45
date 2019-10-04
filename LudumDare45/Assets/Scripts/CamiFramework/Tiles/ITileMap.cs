using System.Collections.Generic;

namespace Cami.Tiles
{
    public interface ITileMap
    {
        bool IsValidTile(int tileX, int tileY);
        float GetTileBlockingValue(int tileX, int tileY);
    }
}