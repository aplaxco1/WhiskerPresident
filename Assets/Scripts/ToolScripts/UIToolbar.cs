using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToolbar : MonoBehaviour
{
    public GameObject LaserPointerHighLight;
    public GameObject BillMovementHighLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ToolSwitching.selectedWeapon == 0) // this is the billmovement 
        {
            LaserPointerHighLight.SetActive(false);
            BillMovementHighLight.SetActive(true);
        }

        if (ToolSwitching.selectedWeapon == 1) // this is the laser pointer s
        {
            LaserPointerHighLight.SetActive(true);
            BillMovementHighLight.SetActive(false);
        }
    }
}
