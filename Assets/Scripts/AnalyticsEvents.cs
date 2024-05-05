using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsEvents : MonoBehaviour
{
    // sends tutorial completed custom event to unity analytics sdk
    static public void tutorialCompleted() {
        Debug.Log(UnityServices.State);
        AnalyticsService.Instance.RecordEvent("tutorialCompleted");
        Debug.Log("TUTORIAL COMPLETE EVENT");
    }
}
