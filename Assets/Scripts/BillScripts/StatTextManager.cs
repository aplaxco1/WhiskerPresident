using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StatTextManager : MonoBehaviour
{

    public static StatTextManager Instance;

    public TMP_Text statAText;
    public TMP_Text statBText;
    public TMP_Text statCText;

    static public bool statChangePopup = false;
    static public int statAChange, statBChange, statCChange;
    static public float popupTime = 3f;
    public List<GameObject> negativePopups = new List<GameObject>();
    public List<GameObject> positivePopups = new List<GameObject>();
    
    // temporary day info UI
    public TMP_Text dayInfoText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        statAText = transform.Find("food").GetComponent<TMP_Text>();
        statBText = transform.Find("technology").GetComponent<TMP_Text>();
        statCText = transform.Find("infrastructure").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: TEMPORARY!!
        statAText.text = DayManager.Instance.dayInfo.statA.ToString();
        statBText.text = DayManager.Instance.dayInfo.statB.ToString();
        statCText.text = DayManager.Instance.dayInfo.statC.ToString();
        if (DayManager.Instance.dayInfo.day >= 5) { dayInfoText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "final-day"); }
        else { dayInfoText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "day") + " " + DayManager.Instance.dayInfo.day; }

        // displayPopups
        if (negativePopups.Count > 0 && positivePopups.Count > 0) {
            if (statChangePopup) {
                displayPopups();
                Debug.Log("displaying popups");
            }
        }
    }

    static public void initializePopups(StatVector statChanges) {
        statChangePopup = true;
        statAChange = statChanges.StatA;
        statBChange = statChanges.StatB;
        statCChange = statChanges.StatC;
        popupTime = 3f;
    } 

    void displayPopups() {
        popupTime -= Time.deltaTime;

        if (statAChange < 0) {
            negativePopups[0].SetActive(true);
        }
        if (statAChange > 0) {
            positivePopups[0].SetActive(true);
        }
        if (statBChange < 0) {
            negativePopups[1].SetActive(true);
        }
        if (statBChange > 0) {
            positivePopups[1].SetActive(true);
        }
        if (statCChange < 0) {
            negativePopups[2].SetActive(true);
        }
        if (statCChange > 0) {
            positivePopups[2].SetActive(true);
        }

        if (popupTime < 0f) {
            statChangePopup = false;
            removePopups();
        }

    }

    void removePopups() {
        foreach (GameObject popup in negativePopups) {
            popup.SetActive(false);
        }

        foreach (GameObject popup in positivePopups) {
            popup.SetActive(false);
        }
    }
}
