using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AddManager : MonoBehaviour
{
    private string gameIDAndroid = "3317324";
    private string videoID = "video";
    private string rewardedVideoID = "rewardedVideo";

    public AnalyticsManager anm;

    private void Awake()
    {
        Advertisement.Initialize(gameIDAndroid, true);
    }

    public void UIWatchAd()
    {
        WatchVideoAd(VideoAdEnded);
    }

    public void UIWatchRewardedAd()
    {
        WatchRewardedVideoAd(RewardedVideoAdEnded);
    }

    public void WatchVideoAd(Action<ShowResult> result)
    {
        if (Advertisement.IsReady(videoID))
        {
            ShowOptions so = new ShowOptions();
            so.resultCallback = result;
            Advertisement.Show(videoID, so);
        }
        else
        {
            Debug.Log("no cargo el video");
        }
    }

    public void VideoAdEnded(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Failed:
                Debug.Log("add fallo");
                break;
            case ShowResult.Finished:
                Debug.Log("add termino");
                break;
            case ShowResult.Skipped:
                Debug.Log("add skippeado");
                break;
        }
    }

    public void WatchRewardedVideoAd(Action<ShowResult> result)
    {
        if (Advertisement.IsReady(rewardedVideoID))
        {
            ShowOptions so = new ShowOptions();
            so.resultCallback = result;
            Advertisement.Show(rewardedVideoID, so);
        }
        else
        {
            Debug.Log("no cargo el video rewarded");
        }
    }

    public void RewardedVideoAdEnded(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.Log("rewarded add fallo");
                anm.RewardAddResult(false);
                break;
            case ShowResult.Finished:
                Debug.Log("rewarded add termino");
                anm.RewardAddResult(true);
                break;
            //case ShowResult.Skipped:
            //    Debug.Log("rewarded add skippeado");
            //    break;
        }
    }
}
