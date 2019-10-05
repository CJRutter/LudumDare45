using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GridStateGen
{
    public GridStateGen(int width, int height)
    {
        this.width = width;
        this.height = height;

        this.cells = new int[width, height];
        this.nextState = new int[width, height];
    }

    public void StepCaveGen()
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                int cell = cells[x, y];
                int count = GetNeighbourCount(x, y, 1);
                if (cell == 1 && count >= 4)
                {
                    nextState[x, y] = 1;
                }
                else if (cell == 0 && count >= 5)
                {
                    nextState[x, y] = 1;
                }
                else
                {
                    nextState[x, y] = 0;
                }
            }
        }
    }

    public void UpdateCells()
    {
        var temp = cells;
        cells = nextState;
        nextState = temp;
    }

    public bool ValidCell(int x, int y)
    {
        if (x < 0 || x >= width) return false;
        if (y < 0 || y >= height) return false;
        return true;
    }

    public bool CellHasState(int x, int y, int state)
    {
        if(ValidCell(x, y) == false) return false;

        return cells[x, y] == state;
    }

    private int GetNeighbourCount(int x, int y, int state)
    {
        int count = 0;
        if (CellHasState(x - 1, y,  state)) count++;
        if (CellHasState(x - 1, y + 1,  state)) count++;
        if (CellHasState(x, y + 1,  state)) count++;
        if (CellHasState(x + 1, y + 1,  state)) count++;
        if (CellHasState(x + 1, y,  state)) count++;
        if (CellHasState(x + 1, y - 1,  state)) count++;
        if (CellHasState(x, y - 1,  state)) count++;
        if (CellHasState(x - 1, y - 1,  state)) count++;

        return count;
    }

    public void Randomise(float prob, int state)
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);
                if(chance <= prob)
                    nextState[x, y] = state;
            }
        }
    }

    public void Clear(int state)
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                nextState[x, y] = state;
            }
        }
    }

    #region Properties
    public int[,] Cells { get { return cells; } }
    #endregion Properties

    #region Fields
    private int width;
    private int height;
    private int[,] cells;
    private int[,] nextState;
    #endregion Fields
}
