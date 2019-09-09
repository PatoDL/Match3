using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    public int[,] zarlanga;

    public void InitData(int width, int height)
    {
        zarlanga = new int[width, height];

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                zarlanga[i, j] = Random.Range(0, 4);
            }
        }
        ReviewData(width, height);
        ReviewData(width, height);
    }

    void ReviewData(int width, int height)
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (i>1)
                {
                    bool prev2Same = zarlanga[i - 1, j] == zarlanga[i - 2, j];
                    bool sameAsPrev = zarlanga[i, j] == zarlanga[i - 1, j];

                    if(sameAsPrev && prev2Same)
                    {
                        int iterations = 0;
                        while(sameAsPrev)
                        {
                            iterations++;
                            zarlanga[i, j] = Random.Range(0, 3);
                            if (iterations > 25)
                            {
                                iterations = 0;
                                zarlanga[i, j]++;
                            }
                            sameAsPrev = zarlanga[i, j] == zarlanga[i - 1, j];
                        }
                    }
                }
                if (j > 1)
                {
                    bool down2Same = zarlanga[i, j - 1] == zarlanga[i, j - 2];
                    bool sameAsDown = zarlanga[i, j] == zarlanga[i, j - 1];

                    if (down2Same && sameAsDown)
                    {
                        int iterations = 0;
                        while (sameAsDown)
                        {
                            iterations++;
                            zarlanga[i, j] = Random.Range(0, 3);
                            if (iterations > 25)
                            {
                                iterations = 0;
                                zarlanga[i, j]++;
                            }
                            sameAsDown = zarlanga[i, j] == zarlanga[i, j - 1];
                        }
                    }
                }
            }
        }
    }
}
