using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{

    private bool isOn = false;

    private GameObject lineObj;
    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        // initialize laser pointer line
        lineObj = new GameObject();
        lineRender = lineObj.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Sprites/Default"));
        lineRender.widthMultiplier = 0.05f;
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

            Vector3 laserStartPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 0.5f, Camera.main.transform.position.z);

            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit)) {
                drawLine(laserStartPos, mouseHit.point);
                Vector3 direction = mouseHit.point - laserStartPos;
                RaycastHit laserHit;
                // make sure actual visual laser doesnt go through any objects
                if (Physics.Raycast(laserStartPos, direction, out laserHit, Mathf.Infinity)) {
                    drawLine(laserStartPos, laserHit.point);
                }
            }
            // else {
            //     float distance = 100f; // temp
            //     drawLine(startPos, ray.direction * distance);
            // }
        }
    }

    void drawLine(Vector3 start, Vector3 end) {
        lineRender.SetPosition(0, start);
        lineRender.SetPosition(1, end);
    }
}
