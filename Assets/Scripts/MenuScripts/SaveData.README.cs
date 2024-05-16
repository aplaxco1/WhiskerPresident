/*
----IMPORTANT INFO FOR SAVE DATA, DAY INFO, SCENE TRANSITIONING----

--SaveData--
    !! Ensure all relevant scenes have a SaveManager object !!
    
    Temporary Dev Controls In Scene (Controlled by SaveManager):
    I - Attempt to Force Save Settings
    O - Attempt to Force Load Settings
    J - Attempt to Force Save Data
    K - Attempt to Force Load Data
    Semicolon - Attempt to reset current day info;

--DayInfo / DayManager--
    
    Want to access the current save data/day info?
        Use SaveManager.Instance.currentSaveData
        Use SaveManager.Instance.currentSaveData.dayInfo
        
        ^ Probably will need the above in the newspaper scene (I assume).
        AFAIK, bill review is NOT handled using actual saving/loading, instead using DDOL on all bills
        
    Want to access the current settings? (might not work after scene transition)
        Use SaveManager.Instance.currentSettingsData
        

--SceneTransitioning--

    All scene transitions should use static functions from SceneTransitionManager.cs instead of Unity's SceneManager

    Relevant function signature: 
        public static void TransitionScene(int index, bool save = true, bool reload = true)
        
    Scene transitioning based on scene name is weird right now, try to use scene index instead.
        
    This function (and related ones) will handle scene loading AS WELL AS saving/loading relevant data.

    -If transitioning from scene A to scene B-
        Does scene A make changes to save data? If so, ensure save = true
        Does scene B require access to save data? If so, ensure reload = true
    
    Right now, settings are not saved/loaded on scene transitions. (So accessing currentSettingsData might not work after a scene transition)
*/