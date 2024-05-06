using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsEvents : MonoBehaviour
{

    static public bool lowFramerateFound = false;
    static public float lowFramerateCount = 0f;

    // variables used to determine framerate
    static public float frameCount = 0f;
    static public float dt = 0.0f;
    static public float fps = 0.0f;
    static public float updateRate = 4.0f;  // 4 updates per sec.

    [RuntimeInitializeOnLoadMethod]
    async static void OnRuntimeInitialized()
    {
        // INITIALIZE ANALYTICS DATA COLLECTION
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("CONFIRM ANALYTICS: " + UnityServices.State);

        // check startup time
        if (Time.realtimeSinceStartup > 30f) {
            slowStartup();
        }

    }

    // sends tutorial completed custom event to unity analytics sdk
    static public void tutorialCompleted() {
        AnalyticsService.Instance.RecordEvent("tutorialCompleted");
        Debug.Log("TUTORIAL COMPLETE EVENT");
    }

    // code obtained from: https://discussions.unity.com/t/accurate-frames-per-second-count/21088/3
    static public void checkFramerate() {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f/updateRate) {
            fps = frameCount / dt ;
            frameCount = 0f;
            dt -= 1.0f/updateRate;
        }

        if (fps < 30f) {
            lowFramerateCount += 1;
        }

        if (lowFramerateCount >= 30f && lowFramerateFound == false) {
            lowFramerate();
            lowFramerateFound = true;
        }
    }

    static public void lowFramerate() {
        AnalyticsService.Instance.RecordEvent("lowFramerateDuringPlay");
        Debug.Log("LOW FRAMERATE EVENT");
    }

    static public void slowStartup() {
        AnalyticsService.Instance.RecordEvent("slowStartup");
        Debug.Log("SLOW STARTUP EVENT");
    }

    static public void playerScreenSettings() {
        if (Screen.fullScreen) {
            AnalyticsService.Instance.RecordEvent("playingInFullscreen");
            Debug.Log("FULLSCREEN");
        }
        else {
            AnalyticsService.Instance.RecordEvent("playingInWindowed");
            Debug.Log("WINDOWED");
        }
    }
}
