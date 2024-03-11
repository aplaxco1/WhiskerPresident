using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToolbar : MonoBehaviour
{
    public GameObject LaserPointerHighLight;
    public GameObject BillMovementHighLight;
    public ToolSwitching toolSwitchingScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toolSwitchingScript.selectedWeapon == 0) // this is the billmovement 
        {
            LaserPointerHighLight.SetActive(false);
            BillMovementHighLight.SetActive(true);
        }

        if (toolSwitchingScript.selectedWeapon == 1) // this is the laser pointer s
        {
            LaserPointerHighLight.SetActive(true);
            BillMovementHighLight.SetActive(false);
        }
    }

    public void switchToHand() {
        toolSwitchingScript.selectedWeapon = 0;
        toolSwitchingScript.SelectWeapon();
    }

    public void switchToLaser() {
        toolSwitchingScript.selectedWeapon = 1;
        toolSwitchingScript.SelectWeapon();
    }
}
