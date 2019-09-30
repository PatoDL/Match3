using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text scoreText;

    public Text statusText;

    public Control con;
   
    void Update()
    {
        scoreText.text = "Score: " + con.score;
        if (con.canInteract)
            statusText.text = "Select 2 pieces to switch them and make a match!";
        else
            statusText.text = "Processing, please wait...";
    }
}
