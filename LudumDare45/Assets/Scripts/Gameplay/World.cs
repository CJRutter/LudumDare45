using Cami.Tiles;
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

        ForegroundTilemap.SetTile(new Vector3Int(-1000, 0, 0), lightTile);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int cellIndex = LightingTilemap.WorldToCell(MouseManager.WorldPosition);
            LightingTilemap.SetTile(cellIndex, lightTile);
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

    public float GetTileBlockingValue(int tileX, int tileY)
    {
        return 0;
    }

    #region Properties
    #endregion Properties        

    #region Fields
    public int Width = 100;
    public int Height = 100;
    public Tilemap BackgroundTilemap;
    public Tilemap ForegroundTilemap;
    public Tilemap LightingTilemap;

    public TileBase lightTile;
    #endregion Fields
}
