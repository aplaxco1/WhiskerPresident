using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToolbar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (ToolSwitching.selectedWeapon is LaserPointer)
        {
            Debug.Log("true");
        }
        Debug.Log("false");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
