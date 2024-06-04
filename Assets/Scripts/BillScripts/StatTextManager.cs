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
        dayInfoText.text = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "day") + " " + DayManager.Instance.dayInfo.day;
    }
}
