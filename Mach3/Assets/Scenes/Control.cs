using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public int width;
    public int height;

    Model m;

    public GameObject panel;

    View v;

    void Start()
    {
        m = new Model();

        m.InitData(width, height);

        v = panel.GetComponent<View>();

        v.DrawGrid(m.zarlanga, width, height);
    }
}