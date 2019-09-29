using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

//public class GooglePlayManager {

//    public static void Init()
//    { 
//        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//        // enables saving game progress.
//        .EnableSavedGames()
//        // registers a callback to handle game invitations received while the game is not running.

//        // registers a callback for turn based match notifications received while the
//        // game is not running.

//        // requests the email address of the player be available.
//        // Will bring up a prompt for consent.
//        .RequestEmail()
//        // requests a server auth code be generated so it can be passed to an
//        //  associated back end server application and exchanged for an OAuth token.
//        .RequestServerAuthCode(false)
//        // requests an ID token be generated.  This OAuth token can be used to
//        //  identify the player to other services such as Firebase.
//        .RequestIdToken()
//        .Build();

//        PlayGamesPlatform.InitializeInstance(config);
//    }

//    void EnableDebugLog()
//    {
//        // recommended for debugging:
//        PlayGamesPlatform.DebugLogEnabled = true;
//    }

//    public static void ActivatePlatform()
//    {
//        // Activate the //Google Play Games platform
//        PlayGamesPlatform.Activate();
//    }

//    public static void SignIn()
//    {
//        Social.localUser.Authenticate((bool success) => {
//            if (success)
//                Debug.Log("yes");
//            else
//                Debug.Log("no");
//        });
//    }

//    public static void RevealAchievement()
//    {
//        Social.ReportProgress("CgkIw4zYz-ECEAIQAQ", 100.0f, (bool success) => {
            
//        });
//    }
//}