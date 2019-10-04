using CamiFramwork.Geometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid2dView : BaseBehaviour
{
    public override void Init()
    {
        base.Init();

        grid = new Grid2d();
        CreateChunks();

    }

    void Update()
    {
    }

    private void CreateChunks()
    {
        chunks = new List<Grid2dViewChunk>();

        chunkWidth = (int)(Width / (float)ChunksInX);
        if (chunkWidth * ChunksInX != Width) chunkWidth++;
        chunkHeight = (int)(Height / (float)ChunksInY);
        if (chunkHeight * ChunksInY != Height) chunkHeight++;

        Width = ChunksInX * chunkWidth;
        Height = ChunksInY * chunkHeight;

        grid.Create(Width, Height);

        Debug.Log($"chunkWidth: {chunkWidth}, chunkHeight: {chunkHeight}");

        if (chunkParent == null)
        {
            chunkParent = new GameObject("chunks");
            chunkParent.transform.SetParent(transform);
        }
        for (int chunkY = 0; chunkY < ChunksInY; chunkY++)
        {
            for (int chunkX = 0; chunkX < ChunksInX; chunkX++)
            {
                var chunk = AddChild<Grid2dViewChunk>(ChunkPrefab);
                chunk.transform.SetParent(chunkParent.transform);

                chunk.Create(this, chunkX, chunkY);
                chunks.Add(chunk);
                chunk.Position = new Vector3(
                    chunkX * chunkWidth * CellViewSize,
                    chunkY * chunkHeight * CellViewSize);
            }
        }
    }

    #region Properties
    public Grid2d Grid { get { return grid; } }
    public int ChunkWidth { get { return chunkWidth; } }
    public int ChunkHeight { get { return chunkHeight; } }
    #endregion Properties        

    #region Fields
    public int Width = 100;
    public int Height = 100;
    public int ChunksInX = 10;
    public int ChunksInY = 10;
    public float CellViewSize = 1f;
    public GameObject ChunkPrefab;

    private int chunkWidth = 10;
    private int chunkHeight = 10;

    private GameObject chunkParent;
    private Grid2d grid;
    private List<Grid2dViewChunk> chunks;
    #endregion Fields
}
