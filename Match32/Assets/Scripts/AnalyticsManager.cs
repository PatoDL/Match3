using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    private void Awake()
    {
        //LevelFinished(1, 3, 500);
    }

    public void TimeOut(int score)
    {
        Analytics.CustomEvent("session_finished", new Dictionary<string, object>
        {
            { "score", score }
        });
    }

    public void RewardAddResult(bool finished)
    {
        Analytics.CustomEvent("rewardAdd_result", new Dictionary<string, object>
        {
            { "result", finished }
        });
    }
}
