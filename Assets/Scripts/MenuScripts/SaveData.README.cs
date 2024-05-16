/*
----IMPORTANT INFO FOR SAVE DATA, DAY INFO, SCENE TRANSITIONING----

--SaveData--
    Ensure all relevant scenes have a SaveManager object
    
    Temporary Dev Controls In Scene (Controlled by SaveManager):
    I - Attempt to Force Save Settings
    O - Attempt to Force Load Settings
    J - Attempt to Force Save Data
    K - Attempt to Force Load Data

--DayInfo / DayManager--
    
    Want to access the current save data/day info?
        Use SaveManager.Instance.currentSaveData
        Use SaveManager.Instance.currentSaveData.dayInfo
        
        ^ Probably will need the above in the newspaper scene?
        AFAIK, bill review is NOT handled using actual saving/loading, instead using DDOL on all bills
        
    Want to access the current settings?
        Use SaveManager.Instance.currentSettingsData

--SceneTransitioning--

    All scene transitions should use static functions from SceneTransitionManager.cs instead of SceneManager

    Relevant function signature: 
        public static void TransitionScene(int index, bool save = true, bool reload = true)

    If transitioning from scene A to scene B

    Does scene A make changes to save data? If so, ensure save = true
    Does scene B require access to save data? If so, ensure reload = true
*/