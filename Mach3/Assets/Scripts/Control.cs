using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    GameObject[,] gridObjs;

    public int width;
    public int height;

    GameObject selected1;
    GameObject selected2;

    public GameObject BaseGO;

    public Sprite[] colours;

    int[,] grid;

    Model m;

    public GameObject panel;
    View v;


    int savedI;
    int savedJ;

    void Start()
    {
        m = new Model();

        m.InitData(width,height);

        grid = m.GetGrid();

        while (ReviewData(width, height, false) || ReviewData(width, height, true)) ;

        m.SetGrid(grid);

        v = panel.GetComponent<View>();

        gridObjs = new GameObject[width,height];

        DrawGrid();
    }

    void DrawGrid()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gridObjs[i, j] = v.DrawTile(BaseGO, i, j, colours[grid[i, j]]);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (hit)
            {
                if (!selected1)
                {
                    selected1 = hit.transform.gameObject;
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if(selected1 == gridObjs[i,j])
                            {
                                savedI = i;
                                savedJ = j;
                            }
                        }
                    }
                }
                else
                {
                    selected2 = hit.transform.gameObject;
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if (selected2 == gridObjs[i, j])
                            {
                                if((i== savedI && (j==savedJ || j==savedJ+1 || j==savedJ-1)) || (j==savedJ && (i==savedI || i==savedI+1 || i==savedI-1)))
                                {
                                    //Sprite s = selected1.GetComponent<SpriteRenderer>().sprite;
                                    //selected1.GetComponent<SpriteRenderer>().sprite = selected2.GetComponent<SpriteRenderer>().sprite;
                                    //selected2.GetComponent<SpriteRenderer>().sprite = s;

                                    Destroy(gridObjs[i, j]);
                                    gridObjs[i, j] = null;

                                    Destroy(gridObjs[savedI, savedJ]);
                                    gridObjs[savedI, savedJ] = null;

                                    int value = grid[i, j];
                                    grid[i, j] = grid[savedI, savedJ];
                                    grid[savedI, savedJ] = value;

                                    gridObjs[i, j] = v.DrawTile(BaseGO, i, j, colours[grid[i, j]]);
                                    gridObjs[savedI, savedJ] = v.DrawTile(BaseGO, savedI, savedJ, colours[grid[savedI, savedJ]]);
                                }
                            }
                        }
                    }

                    //Debug.Log("second");
                    
                    selected1 = null;
                    selected2 = null;
                }
            }
        }
    }

    bool ReviewData(int width, int height, bool verticalCheck)
    {
        int repeatedCounter = 1;
        int value;
        int prevValue = -1;
        bool stillChanging = false;

        for(int i=0;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                if (!verticalCheck)
                    value = grid[i, j];
                else
                    value = grid[j, i];
                if(value == prevValue)
                {
                    repeatedCounter++;
                }

                if(repeatedCounter>=3)
                {
                    int iterations = 0;
                    while(value==prevValue)
                    {
                        value = Random.Range(0, 3);
                        iterations++;
                        if (iterations > 25)
                            value++;
                    }
                    if (!verticalCheck)
                        grid[i, j] = value;
                    else
                        grid[j, i] = value;

                    repeatedCounter = 1;
                    stillChanging = true;
                }
                prevValue = value;
            }
        }
        return stillChanging;
    }
}