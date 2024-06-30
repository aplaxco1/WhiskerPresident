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
    public List<GameObject> popups = new List<GameObject>();
    
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
        if (popups.Count > 0) {
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

        if (statAChange != 0) {
            popups[0].SetActive(true);
            if (statAChange < 0) {
                popups[0].GetComponent<TMP_Text>().text = statAChange.ToString();
                popups[0].GetComponent<TMP_Text>().color = new Color(255f, 0f, 0f);
            }
            else {
                popups[0].GetComponent<TMP_Text>().text = "+" + statAChange.ToString();
                popups[0].GetComponent<TMP_Text>().color = new Color(0f, 255f, 0f);
            }
        }

        if (statBChange != 0) {
            popups[1].SetActive(true);
            if (statBChange < 0) {
                popups[1].GetComponent<TMP_Text>().text = statBChange.ToString();
                popups[1].GetComponent<TMP_Text>().color = new Color(255f, 0f, 0f);
            }
            else {
                popups[1].GetComponent<TMP_Text>().text = "+" + statBChange.ToString();
                popups[1].GetComponent<TMP_Text>().color = new Color(0f, 255f, 0f);
            }
        }

        if (statCChange != 0) {
            popups[2].SetActive(true);
            if (statCChange < 0) {
                popups[2].GetComponent<TMP_Text>().text = statCChange.ToString();
                popups[2].GetComponent<TMP_Text>().color = new Color(255f, 0f, 0f);
            }
            else {
                popups[2].GetComponent<TMP_Text>().text = "+" + statCChange.ToString();
                popups[2].GetComponent<TMP_Text>().color = new Color(0f, 255f, 0f);
            }
        }

        if (popupTime < 0f) {
            statChangePopup = false;
            removePopups();
        }

    }

    void removePopups() {
        foreach (GameObject popup in popups) {
            popup.SetActive(false);
        }
    }
}
