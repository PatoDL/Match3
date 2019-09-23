using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public GameObject DrawTile(GameObject baseGO, int i, int j, Sprite s)
    {
        baseGO.GetComponent<SpriteRenderer>().sprite = s;
        GameObject go = Instantiate(baseGO);
        go.transform.position = new Vector3(i, j);
        go.name = i + "-" + j;
        return go;
    }

    public void EraseTile(int i, int j, GameObject[,] grid)
    {
        
    }
}
