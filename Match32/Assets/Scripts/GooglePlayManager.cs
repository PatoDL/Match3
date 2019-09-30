using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    public static GooglePlayManager gpm;

    void Awake()
    {
        if (gpm == null)
        {
            gpm = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    bool loggedIn;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.Activate();
    }

    public void ShowAchievements()
    {
        if (loggedIn)
            Social.ShowAchievementsUI();
    }

    public void ShowLeaderboard()
    {
        if (loggedIn)
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_highscores);
    }

    public void UploadScore(int score)
    {
        if (loggedIn)
        {
            Social.ReportScore(score, GPGSIds.leaderboard_highscores, (bool success) =>
            {
                ShowLeaderboard();
            });
        }
    }

    public void UnlockAchievement()
    {
        if (loggedIn)
        {
            Social.ReportProgress(GPGSIds.achievement_100_puntos, 100.0f, (bool success) =>
            {
                
            });
        }
    }

    public bool LogIn()
    {
        bool result = false;
#if UNITY_ANDROID && !UNITY_EDITOR
        Social.localUser.Authenticate((bool success) => 
        {
            if (success)
            {
                SetLoggedIn(true);
                result = true;
            }
            else
                result = false;
        });
#endif
        return result;
    }

    public bool LogOut()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GetLoggedIn())
        {
            PlayGamesPlatform.Instance.SignOut();
            SetLoggedIn(false);
            return true;
        }
        else
        {
            return false;
        }
#else
        return false;
#endif
    }

    public void SetLoggedIn(bool log)
    {
        loggedIn = log;
    }

    public bool GetLoggedIn()
    {
        return loggedIn;
    }
}