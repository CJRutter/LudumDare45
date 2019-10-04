using System.Collections.Generic;

namespace Cami.Tiles
{
    public class FieldOfViewScanner
    {
        public FieldOfViewScanner()
        {
        }

        public void FindVisibleTiles(ITileMap tileMap, int viewerX, int viewerY, int visibleRange)
        {
            this.tileMap = tileMap;
            this.viewerX = viewerX;
            this.viewerY = viewerY;
            this.VisualRange = visibleRange;

            if (visibleRange == 0)
                return;

            sqrVisRange = (float)(visibleRange * visibleRange);
            visibleTiles.Clear();

            AddVisible(viewerX, viewerY, 0);

            ScanOctantRows(1, 1f, 0f, -1, 1); // 1 - nnw
            ScanOctantRows(1, 1f, 0f, 1, 1); // 2 - nne
            ScanOctantColumns(1, 1f, 0f, 1, 1); // 3 - ene
            ScanOctantColumns(1, 1f, 0f, 1, -1); // 4 - ese
            ScanOctantRows(1, 1f, 0f, 1, -1); // 5 - sse
            ScanOctantRows(1, 1f, 0f, -1, -1); // 6 - ssw
            ScanOctantColumns(1, 1f, 0f, -1, -1); // 7 - wsw
            ScanOctantColumns(1, 1f, 0f, -1, 1); // 8 - wnw        
        }

        private void ScanOctantRows(int depth, float startSlope, float endSlope, int xDir, int yDir)
        {
            int x = 0;
            int y = 0;
            float sqrDis;

            y = viewerY + (depth * yDir);
            x = viewerX + (int)(startSlope * (float)depth) * xDir;

            bool priorBlocked = true;

#if DEBUG
            const int MaxIters = 1000;
            int currentIters = 0;
#endif

            while (GetSlope(x, y, xDir, yDir, false) >= endSlope)
            {
                if (InVisibleRange(x, y, out sqrDis))
                {
                    if (IsBlocking(x, y))
                    {
                        if (priorBlocked == false)
                        {
                            float sectionEndSlope = GetSlope(x + xDir, y, xDir, yDir, false);
                            ScanOctantRows(depth + 1, startSlope, sectionEndSlope, xDir, yDir);
                        }

                        priorBlocked = true;
                    }
                    else
                    {
                        if (priorBlocked)
                        {
                            startSlope = GetSlope(x, y, xDir, yDir, false);
                        }

                        AddVisible(x, y, sqrDis);
                        priorBlocked = false;
                    }
                }

                x += -xDir;

#if DEBUG
                ++currentIters;
                if (currentIters >= MaxIters)
                    break;
#endif
            }

            if (priorBlocked == false && depth < VisualRange)
            {
                ScanOctantRows(depth + 1, startSlope, endSlope, xDir, yDir);
            }
        }

        private void ScanOctantColumns(int depth, float startSlope, float endSlope, int xDir, int yDir)
        {
            int x = 0;
            int y = 0;
            float sqrDis;

            y = viewerY + (int)(startSlope * (float)depth) * yDir;
            x = viewerX + (depth * xDir);

            bool priorBlocked = true;

#if DEBUG
            const int MaxIters = 1000;
            int currentIters = 0;
#endif
            while (GetSlope(x, y, xDir, yDir, true) >= endSlope)
            {
                if (InVisibleRange(x, y, out sqrDis))
                {
                    if (IsBlocking(x, y))
                    {
                        if (priorBlocked == false)
                        {
                            float sectionEndSlope = GetSlope(x, y + yDir, xDir, yDir, true);
                            ScanOctantColumns(depth + 1, startSlope, sectionEndSlope, xDir, yDir);
                        }

                        priorBlocked = true;
                    }
                    else
                    {
                        if (priorBlocked)
                        {
                            startSlope = GetSlope(x, y, xDir, yDir, true);
                        }

                        AddVisible(x, y, sqrDis);
                        priorBlocked = false;
                    }
                }

                y += -yDir;

#if DEBUG
                ++currentIters;
                if (currentIters >= MaxIters)
                    break;
#endif
            }

            if (priorBlocked == false && depth < VisualRange)
            {
                ScanOctantColumns(depth + 1, startSlope, endSlope, xDir, yDir);
            }
        }

        private void AddVisible(int x, int y, float sqrDis)
        {
            if (tileMap.IsValidTile(x, y) == false)
                return;

            var info = new FovInfo()
            {
                TileX = x,
                TileY = y,
                SqrDis = sqrDis
            };
            visibleTiles.Add(info);
        }

        private bool IsBlocking(int x, int y)
        {
            float opacity = tileMap.GetTileBlockingValue(x, y);
            return opacity >= 1f;
        }

        private bool InVisibleRange(int x, int y, out float sqrDis)
        {
            int deltaX = (x - viewerX);
            int deltaY = (y - viewerY);

            sqrDis = (float)(deltaX * deltaX) + (float)(deltaY * deltaY);

            return sqrDis <= sqrVisRange;
        }

        private float GetSlope(int x, int y, int xDir, int yDir, bool invert)
        {
            x -= viewerX;
            y -= viewerY;

            if (xDir < 0)
                x = -x;
            if (yDir < 0)
                y = -y;

            float slope = 0f;
            if (invert)
            {
                slope = (float)y / (float)x;
            }
            else
            {
                slope = (float)x / (float)y;
            }
            return slope;
        }

        #region Properties
        public int ViewerX { get { return viewerX; } }
        public int ViewerY { get { return viewerY; } }
        public int VisualRange { get; private set; }
        public IEnumerable<FovInfo> VisibleTiles { get { return visibleTiles; } }
        public int VisibleTilesCount { get { return visibleTiles.Count; } }
        #endregion Properties

        #region Fields
        private ITileMap tileMap;

        private int viewerX;
        private int viewerY;
        private float sqrVisRange;
        private List<FovInfo> visibleTiles = new List<FovInfo>();
        #endregion Fields

        public struct FovInfo
        {
            public int TileX;
            public int TileY;
            public float SqrDis;
        }
    }
}