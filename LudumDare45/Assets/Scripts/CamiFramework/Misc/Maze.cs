using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGrid
{
    int GetCellState(int x, int y);
    void SetCellState(int x, int y, int state);
    bool HasNeighbourWithState(int x, int y, int state);
    void ForeachCell(System.Action<int, int, int> action);

    int Width { get; }
    int Height { get; }
}

public class Maze
{
    public Maze(IGrid grid, int emptyState, int wallState)
    {
        this.grid = grid;
        this.emptyState = emptyState;
        this.wallState = wallState;
    }

    public void GenerateMaze(bool preserveState)
    {
        if (!preserveState)
            grid.ForeachCell(SetToWallState);

        int x, y;

        firstHuntRow = 1;
        x = (Random.Range(0, grid.Width / 3) * 3) + 1;
        y = (Random.Range(0, grid.Height / 3) * 3) + 1;
        x = 1;
        y = 1;

        bool complete = false;

        int maxHuntIterations = int.MaxValue;
        int n = 0;
        while (!complete)
        {
            AddRandomPath(ref x, ref y);
            if (FindFreeCell(ref x, ref y))
            {
                ConnectToEmptyNeighbour(x, y);
            }
            else
            {
                complete = true;
            }
            if (++n > maxHuntIterations)
                break;
        }
    }

    private void AddRandomPath(ref int x, ref int y)
    {
        while (true)
        {
            grid.SetCellState(x, y, emptyState);

            dirs.Clear();
                
            if (x - 2 >= 0 &&
                grid.GetCellState(x - 2, y) == wallState &&
                !DoesCellHaveEmptyNeighbour(x - 2, y))
            {
                dirs.Add(0);
            }
            if (x + 2 < grid.Width &&
                grid.GetCellState(x + 2, y) == wallState &&
                !DoesCellHaveEmptyNeighbour(x + 2, y))
            {
                dirs.Add(1);
            }
            if (y - 2 >= 0 &&
                grid.GetCellState(x, y - 2) == wallState &&
                !DoesCellHaveEmptyNeighbour(x, y - 2))
            {
                dirs.Add(2);
            }
            if (y + 2 < grid.Height &&
                grid.GetCellState(x, y + 2) == wallState &&
                !DoesCellHaveEmptyNeighbour(x, y + 2))
            {
                dirs.Add(3);
            }

            if (dirs.Count == 0)
                break;

            switch (dirs[Random.Range(0, dirs.Count)])
            {
                case 0:
                    for (int i = x; i > x - 2; --i)
                        grid.SetCellState(i, y, emptyState);
                    x -= 2;
                    break;
                case 1:
                    for (int i = x; i < x + 2; ++i)
                        grid.SetCellState(i, y, emptyState);
                    x += 2;
                    break;
                case 2:
                    for (int i = y; i > y - 2; --i)
                        grid.SetCellState(x, i, emptyState);
                    y -= 2;
                    break;
                case 3:
                    for (int i = y; i < y + 2; ++i)
                        grid.SetCellState(x, i, emptyState);
                    y += 2;
                    break;
            }

        }
    }

    private bool FindFreeCell(ref int x, ref int y)
    {
        for (y = firstHuntRow; y < grid.Height; y += 2)
        {
            for (x = 1; x < grid.Width; x += 2)
            {
                if (grid.GetCellState(x, y) == wallState &&
                    !DoesCellHaveEmptyNeighbour(x, y))
                {
                    firstHuntRow = y;
                    return true;
                }
            }
        }
        return false;
    }

    private void ConnectToEmptyNeighbour(int x, int y)
    {
        dirs.Clear();

        if (x - 2 >= 0 &&
            grid.GetCellState(x - 2, y) == emptyState)
        {
            dirs.Add(0);
        }
        if (x + 2 < grid.Width &&
            grid.GetCellState(x + 2, y) == emptyState)
        {
            dirs.Add(1);
        }
        if (y - 2 >= 0 &&
            grid.GetCellState(x, y - 2) == emptyState)
        {
            dirs.Add(2);
        }
        if (y + 2 < grid.Height &&
            grid.GetCellState(x, y + 2) == emptyState)
        {
            dirs.Add(3);
        }

        if (dirs.Count == 0)
            return;

        switch (dirs[Random.Range(0, dirs.Count)])
        {
            case 0:
                for (int i = x; i > x - 2; --i)
                    grid.SetCellState(i, y, emptyState);
                break;
            case 1:
                for (int i = x; i < x + 2; ++i)
                    grid.SetCellState(i, y, emptyState);
                break;
            case 2:
                for (int i = y; i > y - 2; --i)
                    grid.SetCellState(x, i, emptyState);
                break;
            case 3:
                for (int i = y; i < y + 2; ++i)
                    grid.SetCellState(x, i, emptyState);
                break;
        }
    }

    private bool DoesCellHaveEmptyNeighbour(int x, int y)
    {
        return grid.HasNeighbourWithState(x, y, emptyState);
    }

    private void SetToWallState(int x, int y, int state)
    {
        grid.SetCellState(x, y, wallState);
    }

    #region Properties
    #endregion Properties

    #region Fields
    private IGrid grid;
    private int emptyState;
    private int wallState;

    // Used for maze generation
    private List<int> dirs = new List<int>();
    private int firstHuntRow;
    #endregion Fields
}
