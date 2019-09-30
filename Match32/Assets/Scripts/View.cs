using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    GameObject[,] gridObjs;

    public GameObject baseGO;

    public Sprite[] colours;

    public void StartViewGrid(int width, int height)
    {
        gridObjs = new GameObject[width, height];
    }

    public GameObject DrawTile(int[,] numGrid, int i, int j)
    {
        baseGO.GetComponent<SpriteRenderer>().sprite = colours[numGrid[i,j]];
        GameObject go = Instantiate(baseGO);
        go.transform.position = new Vector3(i, j);
        go.name = i + "-" + j;
        return go;
    }

    public GameObject[,] GetViewGrid()
    {
        return gridObjs;
    }

    public void ClearEmptyTiles(int[,] numGrid, int width, int height, bool trying)
    {
        for(int j=0;j<height;j++)
        {
            for(int i=0;i<width;i++)
            {
                if ((numGrid[i, j] == -1 || trying) && gridObjs[i, j] != null)
                {
                    Destroy(gridObjs[i, j]);
                    gridObjs[i, j] = null;
                }
            }
        }
    }

    public void DrawNewTiles(int[,] numGrid, int width, int height, bool trying)
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if(gridObjs[i,j] == null && (trying || numGrid[i,j]!=-1))
                {
                    gridObjs[i, j] = DrawTile(numGrid, i, j);
                }
            }
        }
    }

    public void ChangeTile(int[,] numGrid, int width, int height, int i, int j, int i2, int j2)
    {
        if (gridObjs[i, j] != null)
        {
            Destroy(gridObjs[i, j]);
            gridObjs[i, j] = null;
        }

        if (gridObjs[i2, j2] != null)
        {
            Destroy(gridObjs[i2, j2]);
            gridObjs[i2, j2] = null;
        }

        ClearEmptyTiles(numGrid, width, height,true);

        DrawNewTiles(numGrid,width,height,true); 
    }

    public void DeleteAll(int width, int height)
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Destroy(gridObjs[i, j]);
                gridObjs[i, j] = null;
            }
        }
    }
}
