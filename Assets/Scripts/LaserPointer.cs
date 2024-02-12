using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

// NOTE: For now, the point on the desk can be gathered by finding the "LaserPointerLine" GameObject
// in the scene, and getting the position at index 1 of its LineRenderer component.

public class LaserPointer : MonoBehaviour
{

    [SerializeField]
    public float laserWidth = 0.01f;
    public RaycastHit laserHit;

    [SerializeField]
    public Vector3 LaserOffset = new Vector3(0.0f, -0.45f, 0.0f);

    private bool isOn = false;
    private GameObject lineObj;
    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        // initialize laser pointer line
        lineObj = new GameObject("LaserPointerLine");
        lineRender = lineObj.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Sprites/Default"));
        lineRender.widthMultiplier = laserWidth;
        lineRender.positionCount = 2;
        lineRender.startColor = Color.red;
        lineRender.endColor = Color.red;
        lineObj.SetActive(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        // check right click
        if (Input.GetMouseButtonDown(1)) {
            isOn = !isOn;
            lineObj.SetActive(isOn);
        }

        // move laser if on
        if (isOn) {
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // calculate position of visual laser
            Vector3 laserStartPos = new Vector3(Camera.main.transform.position.x + LaserOffset.x, Camera.main.transform.position.y + LaserOffset.y, Camera.main.transform.position.z + LaserOffset.z);

            // determine raycast collision
            RaycastHit mouseHit;
            //hitInfo = mouseHit;
            if (Physics.Raycast(ray, out mouseHit)) {
                //Debug.Log("hit");
                drawLine(laserStartPos, mouseHit.point);
                Vector3 direction = mouseHit.point - laserStartPos;
                RaycastHit laserHit;
                // make sure actual visual laser doesnt go through any objects
                if (Physics.Raycast(laserStartPos, direction, out laserHit, Mathf.Infinity)) {
                    drawLine(laserStartPos, laserHit.point);
                }
            }
            else {
                float distance = 100f;
                drawLine(laserStartPos, ray.direction * distance);
            }
        }
    }

    void drawLine(Vector3 start, Vector3 end) {
        lineRender.SetPosition(0, start);
        lineRender.SetPosition(1, end);
    }
}
