using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsEvents : MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod]
    async static void OnRuntimeInitialized()
    {
        // INITIALIZE ANALYTICS DATA COLLECTION
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("CONFIRM ANALYTICS: " + UnityServices.State);
    }

    // sends tutorial completed custom event to unity analytics sdk
    static public void tutorialCompleted() {
        AnalyticsService.Instance.RecordEvent("tutorialCompleted");
        Debug.Log("TUTORIAL COMPLETE EVENT");
    }
}
