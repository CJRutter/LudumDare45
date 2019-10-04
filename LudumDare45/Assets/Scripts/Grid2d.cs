using System.Collections;
using System.Collections.Generic;

public class Grid2d
{
    public Grid2d()
    {
    }

    public void Create(int width, int height)
    {
        this.width = width;
        this.height = height;

        cells = new GridCell[width, height];
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                cells[x, y] = new GridCell()
                {
                    Tile = (byte)UnityEngine.Random.Range(0, 2)
                };
            }
        }
    }

    public void ProcCells(System.Func<int, int, GridCell, GridCell> func)
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                this[x, y] = func(x, y, cells[x, y]);
            }
        }
    }

    #region Properties
    public int Width { get { return width; } }
    public int Height { get { return height; } }

    public GridCell this[int x, int y]
    {
        get { return cells[x, y]; }
        set
        {
            cells[x, y] = value;
            CellTileChanged?.Invoke(x, y, value);
        }
    }
    #endregion Properties        

    #region Fields
    private int width;
    private int height;
    private GridCell[,] cells;
    #endregion Fields

    #region Events
    public delegate void Grid2dCellEventHandler(int x, int y, GridCell cell);

    public event Grid2dCellEventHandler CellTileChanged;
    #endregion Events
    }

public struct GridCell
{
    public byte Tile;
}
