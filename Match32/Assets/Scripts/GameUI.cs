using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GameUI : MonoBehaviour
{
    public Text scoreText;

    public Text timeText;

    public Text statusText;

    public Control con;
   
    void Update()
    {
        scoreText.text = "Score: " + con.score;
        timeText.text = "Time: " + (int)con.time;

        if (con.canInteract)
            statusText.text = "Select 2 pieces to switch" + System.Environment.NewLine +
                " them and make a match!";
        else if(con.time <=0)
        {
            statusText.text = "Game Ended!" + System.Environment.NewLine + " press restart to play again";
        }
        else
            statusText.text = "Processing, please wait...";
    }

    public Text LogResult;

    public void LogIn()
    {
        if(GooglePlayManager.gpm.LogIn())
        {
            ShowLogResult("Logged In");
        }
        else
        {
            ShowLogResult("Couldn't Log In");
        }
    }

    public void LogOut()
    {
        if (GooglePlayManager.gpm.LogOut())
        {
            ShowLogResult("Logged Out");
        }
        else
        {
            ShowLogResult("Not Logged in");
        }
    }

    void ShowLogResult(string result)
    {
        LogResult.gameObject.SetActive(true);
        LogResult.text = result;
        if (!IsInvoking())
            Invoke("HideLogResult", 2f);
        else
        {
            CancelInvoke();
            Invoke("HideLogResult", 2f);
        }
    }

    public void ShowAchievements()
    {
        GooglePlayManager.gpm.ShowAchievements();
    }

    public void ShowLeaderboard()
    {
        GooglePlayManager.gpm.ShowLeaderboard();
    }

    void HideLogResult()
    {
        LogResult.gameObject.SetActive(false);
    }

    public void OpenLeaderboard()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        GooglePlayManager.gpm.ShowLeaderboard();
#endif
    }

    public void RestartGame()
    {
        con.Restart();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
