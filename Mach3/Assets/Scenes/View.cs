using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public GameObject[] item;

    public void DrawGrid(int[,] m, int width, int height)
    {
        Vector3 size = item[0].transform.localScale;

        for(int i=0;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                GameObject go = Instantiate(item[m[j,i]], new Vector3(j*size.x,i*size.y),Quaternion.identity);
            }
        }
    }
}
