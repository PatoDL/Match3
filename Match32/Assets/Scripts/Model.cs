using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    int[,] grid;

    public int[,] GetGrid()
    {
        return grid;
    }

    public int GetValue(int posX, int posY)
    {
        return grid[posX, posY];
    }

    public void InitData(int width, int height)
    {
        grid = new int[width, height];

        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                grid[i, j] = Random.Range(0, 4);
            }
        }
    }

    public void SetGrid(int[,] newGrid)
    {
        grid = newGrid;
    }
}
