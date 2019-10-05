using Cami.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : GameComponent, ITileMap
{
    public override void Init()
    {
        base.Init();

        Physics2D.gravity = Vector2.zero;

        CreateForeground();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int cellIndex = LightingTilemap.WorldToCell(MouseManager.WorldPosition);
            LightingTilemap.SetTile(cellIndex, LightTile);
        }
    }

    public TileBase GetTileAtCursor()
    {
        return GetTileAtCursor(LightingTilemap);
    }

    private TileBase GetTileAtCursor(Tilemap tilemap)
    {
        Vector3Int cellIndex = tilemap.WorldToCell(MouseManager.WorldPosition);
        return tilemap.GetTile(cellIndex);
    }

    public bool IsValidTile(int tileX, int tileY)
    {
        if (tileX < 0 || tileX > Width) return false;
        if (tileY < 0 || tileY > Height) return false;
        return true;
    }

    public float GetTileBlockingValue(int x, int y)
    {
        WorldCell cell = GetCell(x, y, CellLayer.Foreground);
        return cell.LightBlock;
    }

    private void CreateForeground()
    {
        //SetCell(10, 10, RockCellIndex, CellLayer.Foreground);
        //SetCell(2, 1, TranslucentCellIndex, CellLayer.Background);
        //FloodFillCells(0, 0, RockCellIndex, CellLayer.Foreground);

        var stateGen = new GridStateGen(Width, Height);
        stateGen.Randomise(0.5f, 1);
        stateGen.UpdateCells();

        for (int i = 0; i < 10; ++i)
        {
            stateGen.StepCaveGen();
            stateGen.UpdateCells();
        }

        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                WorldCell cell = GetCell(x, y, CellLayer.Foreground);

                int state = stateGen.Cells[x, y];
                if (state == 1)
                {
                    SetCell(x, y, RockCellIndex, CellLayer.Foreground);
                }
            }
        }
    }

    private Tilemap GetTilemap(CellLayer layer)
    {
        switch (layer)
        {
            case CellLayer.Foreground:
                return ForegroundTilemap;
            case CellLayer.Background:
                return BackgroundTilemap;
            case CellLayer.Light:
                return LightingTilemap;
            default:
                return null;
        }
    }

    public WorldCell GetCell(int x, int y, CellLayer layer)
    {
        Tilemap tilemap = GetTilemap(CellLayer.Foreground);
        var tilePos = new Vector3Int(x, y, 1);
        return tilemap.GetTile<WorldCell>(tilePos);
    }

    public void SetCell(int x, int y, int cellIndex, CellLayer layer)
    {
        WorldCell cell = null;
        if(cellIndex >= 0 && cellIndex < BaseWorldCells.Count)
            cell = BaseWorldCells[cellIndex];

        var tilePos = new Vector3Int(x, y, 1);
        Tilemap tilemap = GetTilemap(layer);
        tilemap.SetTile(tilePos, cell);
    }

    public void SetCellColour(int x, int y, int cellIndex, CellLayer layer, Color colour)
    {
        WorldCell cell = null;
        if (cellIndex >= 0 && cellIndex < BaseWorldCells.Count)
            cell = BaseWorldCells[cellIndex];

        var tilePos = new Vector3Int(x, y, 1);
        Tilemap tilemap = GetTilemap(layer);
        tilemap.SetColor(tilePos, colour);
    }
    
    public void FloodFillCells(int x, int y, int cellIndex, CellLayer layer)
    {
        WorldCell cell = null;
        if (cellIndex >= 0 && cellIndex < BaseWorldCells.Count)
            cell = BaseWorldCells[cellIndex];

        var tilePos = new Vector3Int(x, y, 1);
        Tilemap tilemap = GetTilemap(layer);
        tilemap.FloodFill(tilePos, cell);
    }

    public void BoxFillCells(int x, int y, int cellIndex, CellLayer layer, int startX, int startY, int endX, int endY)
    {
        WorldCell cell = null;
        if (cellIndex >= 0 && cellIndex < BaseWorldCells.Count)
            cell = BaseWorldCells[cellIndex];

        var tilePos = new Vector3Int(x, y, 1);
        Tilemap tilemap = GetTilemap(layer);
        tilemap.BoxFill(tilePos, cell, startX, startY, endX, endY);
    }

    #region Properties
    #endregion Properties        

    #region Fields
    public int Width = 100;
    public int Height = 100;
    public Tilemap BackgroundTilemap;
    public Tilemap ForegroundTilemap;
    public Tilemap LightingTilemap;

    public TileBase LightTile;

    public List<WorldCell> BaseWorldCells = new List<WorldCell>();
    #endregion Fields

    #region Consts
    public const byte EmptyCellIndex = 0;
    public const byte RockCellIndex = 1;
    public const byte TranslucentCellIndex = 2;
    #endregion Consts

    public enum CellLayer
    {
        Foreground,
        Background,
        Light
    }
}
