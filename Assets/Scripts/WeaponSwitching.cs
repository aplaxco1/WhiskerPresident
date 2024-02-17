using UnityEngine;

public class WeaponSwitching : MonoBehaviour
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

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedWeapon >= transform.childCount -1)
                selectedWeapon = 0;
            else
                selectedWeapon++;

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(selectedWeapon <= 0)
                selectedWeapon = transform.childCount -1;
            else
                selectedWeapon--;
                
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
                weapon.gameObject.SetActive(true);
                Cursor.SetCursor(weapon.gameObject.GetComponent<ToolClass>().cursorTexture, Vector2.zero, CursorMode.Auto);
            }
            else {
                // really dumb way to make sure laser is removed when not selected
                if (weapon.gameObject.GetComponent<ToolClass>() is LaserPointer) {
                    weapon.gameObject.GetComponent<LaserPointer>().removeLaser();
                }
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
