using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTextManager : MonoBehaviour
{

    public static StatTextManager Instance;

    public TMP_Text redStatText;
    public TMP_Text greenStatText;
    public TMP_Text blueStatText;

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
        redStatText = transform.Find("RedStatText").GetComponent<TMP_Text>();
        greenStatText = transform.Find("GreenStatText").GetComponent<TMP_Text>();
        blueStatText = transform.Find("BlueStatText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
