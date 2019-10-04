using CamiFramwork.Geometry;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid2dViewChunk : BaseBehaviour
{
    public override void Init()
    {
        base.Init();
    }

    void Update()
    {
        if(meshValid == false)
            Rebuild();
    }

    public void Create(Grid2dView gridView, int chunkX, int chunkY)
    {
        this.chunkX = chunkX;
        this.chunkY = chunkY;
        this.gridView = gridView;

        geometryValid = true;
        meshValid = true;

        gridView.Grid.CellTileChanged += Grid_CellTileChanged;

        Rebuild();
    }

    public void Rebuild()
    {
        if (geometry == null)
        {
            geometry = new Geometry();
        }

        int startYCell = gridView.ChunkHeight * chunkY;
        int startXCell = gridView.ChunkWidth * chunkX;
        int endYCell = startYCell + gridView.ChunkHeight;
        int endXCell = startXCell + gridView.ChunkWidth;

        var colliderPoints = new List<Vector2>();
        int solidTileCount = 0;
        for (int y = startYCell; y < endYCell; ++y)
        {
            for (int x = startXCell; x < endXCell; ++x)
            {
                GridCell cell = gridView.Grid[x, y];
                if (cell.Tile != 0)
                    solidTileCount++;
            }
        }
        ChunkCollider.pathCount = solidTileCount;

        geometry.Clear();
        int pathIndex = 0;
        for (int y = startYCell; y < endYCell; ++y)
        {
            for (int x = startXCell; x < endXCell; ++x)
            {
                GridCell cell = gridView.Grid[x, y];
                if (cell.Tile == 0)
                    continue;

                float xPos = (x - startXCell) * gridView.CellViewSize;
                float yPos = (y - startYCell) * gridView.CellViewSize;
                colliderPoints.Clear();
                GeometryHelper.AddSquare(xPos, yPos, geometry, gridView.CellViewSize, colliderPoints);

                ChunkCollider.SetPath(pathIndex, colliderPoints);
                ++pathIndex;
            }
        }
        geometry.Build(BackgroundMeshFilter);
        geometry.Build(LightingMeshFilter);

        geometryValid = true;
        meshValid = true;
    }

    private void Grid_CellTileChanged(int x, int y, GridCell cell)
    {
        geometryValid = false;
        meshValid = false;
    }

    #region Properties
    #endregion Properties        

    #region Fields
    public MeshFilter ForegroundMeshFilter;
    public MeshFilter BackgroundMeshFilter;
    public MeshFilter LightingMeshFilter;
    public PolygonCollider2D ChunkCollider;

    private Geometry geometry;
    private bool geometryValid = true;
    private bool meshValid = true;

    private int chunkX;
    private int chunkY;
    private Grid2dView gridView;
    #endregion Fields
}
