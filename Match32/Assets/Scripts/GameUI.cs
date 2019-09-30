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

    public Text LogResult;

    public void LogIn()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Social.localUser.Authenticate((bool success) => 
        {
            if (success)
            {
                ShowLogResult("Logged In");
                GooglePlayManager.gpm.SetLoggedIn(true);
            }
            else
                ShowLogResult("Couldn't Log In");
        });
#endif
    }

    public void LogOut()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GooglePlayManager.gpm.GetLoggedIn())
        {
            PlayGamesPlatform.Instance.SignOut();
            ShowLogResult("Logged Out");
        }
        else
        {
            ShowLogResult("Not Logged in");
        }
#endif
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
