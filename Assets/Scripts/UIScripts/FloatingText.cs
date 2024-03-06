using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TMP_Text passText;
    public TMP_Text vetoText;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnableText", 1f);
        Invoke("DisableText", 1f);
    }

    void EnableText()
    {
        if (passText.GetComponentInChildren<BillController>().evaluatePassVeto() > 0)
        {
            passText.enabled = true;
        }
        if (vetoText.GetComponentInChildren<BillController>().evaluatePassVeto() < 0)
        {
            vetoText.enabled = true;
        }
            
    }

    void DisableText()
    {
        passText.enabled = false;
        vetoText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
