using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarLaserPointer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject LaserPointerGameObject;
    public GameObject BillMovementGameObject;
    public GameObject LaserPointerHighlight; 
    public GameObject BillMovementHighlight;
    //public ToolSwitching toolSwitching;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LaserPointerGameObject.SetActive(true);  LaserPointerHighlight.SetActive(true);
            BillMovementGameObject.SetActive(false); BillMovementHighlight.SetActive(false);
            ToolSwitching.selectedWeapon = 1;

            Debug.Log("Clicked the Laser POinter button" + ToolSwitching.selectedWeapon);



        }

    }
}
