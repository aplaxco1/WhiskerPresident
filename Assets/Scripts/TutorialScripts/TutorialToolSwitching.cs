using UnityEngine;

public class TutorialToolSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown("right"))
        {
            if(selectedWeapon >= transform.childCount -1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
            TutorialSequenceTwo.NextStepInTutorial(1);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown("left"))
        {
            if(selectedWeapon <= 0)
                selectedWeapon = transform.childCount -1;
            else
                selectedWeapon--;
            TutorialSequenceTwo.NextStepInTutorial(1);
        }

        if(previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon ()
    {
        int i = 0;
        foreach (Transform weapon in transform) 
        {
            if(i == selectedWeapon) {
                if (weapon.gameObject.GetComponent<ToolClass>() is BillMovement) {
                    weapon.gameObject.GetComponent<BillMovement>().addObjectHighlighting();
                }
                weapon.gameObject.GetComponent<ToolClass>().isActive = true;
                Cursor.SetCursor(weapon.gameObject.GetComponent<ToolClass>().cursorTexture, Vector2.zero, CursorMode.Auto);
            }
            else {
                if (weapon.gameObject.GetComponent<ToolClass>() is BillMovement) {
                    weapon.gameObject.GetComponent<BillMovement>().removeObjectHighlighting();
                }
                // really dumb way to make sure laser is removed when not selected
                if (weapon.gameObject.GetComponent<ToolClass>() is TutorialLaserPointer) {
                    weapon.gameObject.GetComponent<TutorialLaserPointer>().removeLaser();
                }
                weapon.gameObject.GetComponent<ToolClass>().isActive = false;
            }
            i++;
        }
    }
}