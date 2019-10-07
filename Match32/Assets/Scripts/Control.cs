using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public int width;
    public int height;

    public int score = 0;
    public float time = 120;

    int[,] grid;

    Model m;

    public GameObject panel;
    View v;

    public AddManager am;

    public AnalyticsManager anm;

    public GameObject adPanel;

    bool stopTime;

    void Start()
    {
        m = new Model();

        grid = m.GetGrid();

        Restart();
    }

    public void Restart()
    {
        stopTime = false;

        score = 0;

        time = 10;

        m.InitData(width, height);

        grid = m.GetGrid();

        v = panel.GetComponent<View>();

        m.SetGrid(grid);

        v.ClearEmptyTiles(grid, width, height, false);

        

        while (ReviewData(true, true) || ReviewData(false, true)) ;

        v.StartViewGrid(width, height);

        v.ClearEmptyTiles(grid, width, height, true);

        v.DrawNewTiles(grid, width, height, true);

        canInteract = true;
        selected1 = null;
        selected2 = null;
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
                        if (hit.transform.gameObject == v.GetViewGrid()[i, j])
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
                        if (hit.transform.gameObject == v.GetViewGrid()[i, j])
                        {
                            if (((i == s1I) && (j == s1J + 1 || j == s1J - 1)) || ((j == s1J) && ((i == s1I + 1) || (i == s1I - 1))))
                            {
                                selected2 = hit.transform.gameObject;
                                s2I = i;
                                s2J = j;

                                if (!CheckIfCanChange(s1I, s1J, s2I, s2J))
                                    CheckIfCanChange(s2I, s2J, s1I, s1J);
                                else
                                    PassData();

                                break;
                            }
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
                            if (hit.transform.gameObject == v.GetViewGrid()[i, j])
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
                            if (hit.transform.gameObject == v.GetViewGrid()[i, j])
                            {
                                if (((i == s1I) && (j == s1J + 1 || j == s1J - 1)) || ((j == s1J) && ((i == s1I + 1) || (i == s1I - 1))))
                                {
                                    selected2 = hit.transform.gameObject;
                                    s2I = i;
                                    s2J = j;
                               
                                    if (!CheckIfCanChange(s1I, s1J, s2I, s2J))
                                        CheckIfCanChange(s2I, s2J, s1I, s1J);
                               
                                    break;
                                }
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
        }
        else
            canInteract = true;

        
        if(time<=0f)
        {
            time = 0f;
            GooglePlayManager.gpm.UploadScore(score);
            am.UIWatchAd();
            anm.TimeOut(score);
            adPanel.SetActive(true);
            time = 10;
            canInteract = false;
            stopTime = true;
        }
        else if(!stopTime)
            time -= Time.deltaTime;
    }

    bool CheckIfCanChange(int i, int j, int i2, int j2)
    {
        bool changed = false;

        int sValue = grid[i, j];
        grid[i, j] = grid[i2, j2];
        grid[i2, j2] = sValue;

        v.ChangeTile(grid, width, height, i, j, i2, j2);

        bool hor = ReviewData(false, false);
        bool ver = ReviewData(true, false);

        return changed = (hor || ver);
    }

    bool Erase3(int i,int j, bool vertical,bool check)
    {
        bool correctMatch = true;
        if (!vertical)
        {
            for (int d = 0; d < 3; d++)
            {
                if (i - d >= 0 && i < width)
                {
                    if (!check)
                        grid[i - d, j] = -1;
                }
                else
                {
                    correctMatch = false;
                    break;
                }
            }
        }
        else
        {
            for (int d = 0; d < 3; d++)
            {
                if (i - d >= 0 && i < height)
                {
                    if (!check)
                        grid[j, i - d] = -1;
                }
                else
                {
                    correctMatch = false;
                    break;
                }
            }
        }

        if(!check)
        {
            score += 10;
            v.ClearEmptyTiles(grid, width, height, false);
        }

        if (score>100)
        {
            GooglePlayManager.gpm.UnlockAchievement();
        }

        return correctMatch;
    }

    void PassData()
    {
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
                            v.ClearEmptyTiles(grid, width, height, false);
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

        v.DrawNewTiles(grid, width, height,false);
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

                        stillChanging = true;
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
                        if(Erase3(i, j, vertical,true))
                        {
                            Erase3(i, j, vertical, false);
                            stillChanging = true;
                        }
                    }
                    repCounter = 1;
                }
                pValue = aValue;
            }
            repCounter = 1;
        }
        return stillChanging;
    }
}