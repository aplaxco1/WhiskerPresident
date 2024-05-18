using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StatTextManager : MonoBehaviour
{

    public static StatTextManager Instance;

    [FormerlySerializedAs("redStatText")] public TMP_Text statAText;
    [FormerlySerializedAs("greenStatText")] public TMP_Text statBText;
    [FormerlySerializedAs("blueStatText")] public TMP_Text statCText;
    
    // temporary day info UI
    [FormerlySerializedAs("blueStatText")] public TMP_Text dayInfoText;

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
        statAText = transform.Find("RedStatText").GetComponent<TMP_Text>();
        statBText = transform.Find("GreenStatText").GetComponent<TMP_Text>();
        statCText = transform.Find("BlueStatText").GetComponent<TMP_Text>();
        dayInfoText = transform.Find("DayInfoText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: TEMPORARY!!
        dayInfoText.text = DayManager.Instance.dayInfo.ConvertToString();
    }
}
