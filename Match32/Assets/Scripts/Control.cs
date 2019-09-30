using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    GameObject[,] gridObjs;

    public int width;
    public int height;

    public int score = 0;

    public GameObject BaseGO;

    public Sprite[] colours;

    int[,] grid;

    Model m;

    public GameObject panel;
    View v;


    public void points()
    {
        //GooglePlayManager.RevealAchievement();
    }

    private void Awake()
    {
        //GooglePlayManager.Init();
        //GooglePlayManager.ActivatePlatform();
    }

    void Start()
    {
        m = new Model();

        m.InitData(width, height);

        grid = m.GetGrid();

        while (ReviewData(true, true) || ReviewData(false, true)) ;

        m.SetGrid(grid);

        v = panel.GetComponent<View>();

        gridObjs = new GameObject[width, height];

        DrawGrid();

        //GooglePlayManager.SignIn();
    }

    void DrawGrid()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (gridObjs[i, j] != null)
                    Destroy(gridObjs[i, j]);
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gridObjs[i, j] = v.DrawTile(BaseGO, i, j, colours[grid[i, j]]);
            }
        }
    }

    GameObject selected1;
    GameObject selected2;

    int s1I;
    int s1J;

    int s2I;
    int s2J;

    public bool canInteract = true;

    void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && canInteract)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (hit)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (hit.transform.gameObject == gridObjs[i, j])
                        {
                            selected1 = hit.transform.gameObject;
                            s1I = i;
                            s1J = j;
                            break;
                        }
                    }
                }
            }
        }
        if(Input.GetMouseButtonUp(0) && canInteract)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (hit)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (hit.transform.gameObject == gridObjs[i, j])
                        {
                            selected2 = hit.transform.gameObject;
                            s2I = i;
                            s2J = j;

                            if (!ChangeTile(s1I, s1J, s2I, s2J))
                                ChangeTile(s2I, s2J, s1I, s1J);

                            break;
                        }
                    }
                }
                selected1 = null;
                selected2 = null;
            }
        }
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        if(Input.touchCount>0 && canInteract)
        {
            Touch t = Input.GetTouch(0);

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(t.position), Vector2.zero);

            if(hit)
            {
                switch(t.phase)
                {
                case TouchPhase.Began:
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            if (hit.transform.gameObject == gridObjs[i, j])
                            {
                                selected1 = hit.transform.gameObject;
                                s1I = i;
                                s1J = j;
                                break;
                            }
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            if (hit.transform.gameObject == gridObjs[i, j])
                            {
                                selected2 = hit.transform.gameObject;
                                s2I = i;
                                s2J = j;

                                if (!ChangeTile(s1I, s1J, s2I, s2J))
                                    ChangeTile(s2I, s2J, s1I, s1J);

                                break;
                            }
                        }
                    }
                    break;
                case TouchPhase.Canceled:
                    selected1 = null;
                    selected2 = null;
                    break;
                }
            }
            selected1 = null;
            selected2 = null;
        }
#endif

        if (ReviewData(false, false) || ReviewData(true, false))
        {
            canInteract = false;
            PassData();
            DrawGrid();
        }
        else
            canInteract = true;
    }

    bool ChangeTile(int i, int j, int i2, int j2)
    {
        bool changed = false;

        if (gridObjs[i, j] != null)
        {
            Destroy(gridObjs[i, j]);
            gridObjs[i, j] = null;
        }

        if (gridObjs[i, j] != null)
        {
            Destroy(gridObjs[i2, j2]);
            gridObjs[i2, j2] = null;
        }

        int sValue = grid[i, j];
        grid[i, j] = grid[i2, j2];
        grid[i2, j2] = sValue;

        DrawGrid();

        bool hor = ReviewData(false, false);
        bool ver = ReviewData(true, false);

        return changed = (hor || ver);
    }

    void Erase3(int i,int j, bool vertical)
    {
        if(!vertical)
        {
            for (int d = 0; d < 3; d++)
            {
                if (gridObjs[i-d, j] != null && (i - d < width) && (i - d >= 0))
                {
                    Destroy(gridObjs[i - d, j]);
                    gridObjs[i - d, j] = null;
                    score += 10;
                }
            }
        }
        else
        {
            for (int d = 0; d < 3; d++)
            {
                if (gridObjs[j, i - d] != null && (i-d < width) && (i - d >= 0))
                {
                    Destroy(gridObjs[j, i - d]);
                    gridObjs[j, i - d] = null;
                    score += 10;
                }
            }
        }
    }

    void PassData()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (gridObjs[i, j] == null)
                {
                    grid[i, j] = -1;
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(grid[i, j] == -1)
                {
                    int k = j+1;
                    while (k < width)
                    {
                        if(grid[i, k] != -1)
                        {
                            grid[i, j] = grid[i, k];
                            grid[i, k] = -1;
                            break;
                        }
                        else
                        {
                            k++;
                        }
                    }
                    if (k >= width)
                        grid[i, j] = Random.Range(0, 4);
                }
            }
        }
    }

    bool ReviewData(bool vertical, bool changeRep)
    {
        int aValue;
        int pValue = -2;
        int repCounter = 1;
        bool stillChanging = false;

        for(int j=0;j<height;j++)
        {
            for(int i=0;i<width;i++)
            {
                if (!vertical)
                {
                    aValue = grid[i, j];
                }
                else
                {
                    aValue = grid[j, i];
                }

                if (aValue == pValue)
                {
                    repCounter++;
                }
                else
                {
                    repCounter = 1;
                }

                if(repCounter>=3)
                {
                    if (changeRep)
                    {
                        int iter = 0;
                        while (aValue == pValue)
                        {
                            iter++;
                            aValue = Random.Range(0, 3);
                            if (iter > 25)
                                aValue++;
                        }
                        if (!vertical)
                            grid[i, j] = aValue;
                        else
                            grid[j, i] = aValue;
                    }
                    else
                    {
                        if (!vertical)
                        {
                            Debug.Log((i - 2) + "-" + j);
                            Debug.Log((i - 1) + "-" + j);
                            Debug.Log(i + "-" + j);
                        }
                        else
                        {
                            Debug.Log(j + "-" + (i - 2));
                            Debug.Log(j + "-" + (i - 1));
                            Debug.Log(j + "-" + i);
                        }
                        Erase3(i, j, vertical);
                    }
                    stillChanging = true;
                    repCounter = 1;
                }
                pValue = aValue;
            }
            repCounter = 1;
        }
        return stillChanging;
    }
}