using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class TutorialLaserPointer : ToolClass
{
    [Header("Adjustable Laser Variables")]
    [SerializeField]
    public float laserWidth = 0.01f;
    [SerializeField]
    public Vector3 laserOffset = new(0.0f, -0.45f, 0.0f);
    [SerializeField]
    public GameObject laserDot;
    public Material laserMaterial;

    [Header("Global Laser Variables")]
    // variables to capture location on desk for cats attention
    [SerializeField]
    public bool isOnDesk = false;
    [SerializeField]
    public Vector3 laserDeskLocation = new(0f, 0f, 0f);
    public TutorialBillMovement billScript;

    //private bool isOn = false;
    private GameObject lineObj;
    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        // initialize laser pointer line
        lineObj = new GameObject("LaserPointerLine");
        lineRender = lineObj.AddComponent<LineRenderer>();
        lineRender.material = laserMaterial ? laserMaterial : new Material(Shader.Find("Sprites/Default"));
        lineRender.widthMultiplier = laserWidth;
        lineRender.positionCount = 2;
        lineRender.startColor = Color.red;
        lineRender.endColor = Color.red;
        lineObj.SetActive(true);
        // initialize laser pointer dot
        laserDot = Instantiate(laserDot, new Vector3(0,0,0), Quaternion.identity);
        laserDot.SetActive(isOnDesk);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            // set laser dot active if laser is on desk
            laserDot.SetActive(isOnDesk);
            lineObj.SetActive(true);
            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // calculate position of visual laser
            Vector3 laserStartPos = new Vector3(Camera.main.transform.position.x + laserOffset.x, Camera.main.transform.position.y + laserOffset.y, Camera.main.transform.position.z + laserOffset.z);

            // determine raycast collision
            RaycastHit mouseHit;
            //hitInfo = mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                //Debug.Log("hit");
                drawLine(laserStartPos, mouseHit.point);
                Vector3 direction = mouseHit.point - laserStartPos;
                RaycastHit laserHit;
                // make sure actual visual laser doesnt go through any objects
                if (Physics.Raycast(laserStartPos, direction, out laserHit, Mathf.Infinity) && !isOnDesk)
                {
                    drawLine(laserStartPos, laserHit.point);
                }
                // check if on desk
                if (mouseHit.collider.gameObject.layer == LayerMask.NameToLayer("Desk"))
                {
                    if (mouseHit.collider.gameObject.CompareTag("Bill") && billScript.inspectingBill)
                    {
                        isOnDesk = false;
                        laserDot.SetActive(true);
                    }
                    else
                    {
                        isOnDesk = true;
                    }
                    laserDeskLocation = mouseHit.point;
                    laserDot.transform.position = mouseHit.point;
                    laserDot.transform.rotation = Quaternion.FromToRotation(laserDot.transform.up, mouseHit.normal) * laserDot.transform.rotation;
                }
                else
                {
                    isOnDesk = false;
                }
            }
            else
            {
                float distance = 100f;
                drawLine(laserStartPos, ray.direction * distance);
                isOnDesk = false;
            }
        }

        //// check right click
        //if (Input.GetMouseButtonDown(0) && isActive) {
        //    TutorialSequenceTwo.NextStepInTutorial(2);
        //    isOn = !isOn;
        //    lineObj.SetActive(isOn);
        //}

        //// set laser dot active if laser is on desk
        //laserDot.SetActive(isOnDesk);

        //// move laser if on
        //if (isOn) {
        //    // create ray from camera to mouse
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    // calculate position of visual laser
        //    Vector3 laserStartPos = new(Camera.main.transform.position.x + laserOffset.x, Camera.main.transform.position.y + laserOffset.y, Camera.main.transform.position.z + laserOffset.z);

        //    // determine raycast collision
        //    RaycastHit mouseHit;
        //    //hitInfo = mouseHit;
        //    if (Physics.Raycast(ray, out mouseHit)) {
        //        //Debug.Log("hit");
        //        drawLine(laserStartPos, mouseHit.point);
        //        Vector3 direction = mouseHit.point - laserStartPos;
        //        RaycastHit laserHit;
        //        // make sure actual visual laser doesnt go through any objects
        //        if (Physics.Raycast(laserStartPos, direction, out laserHit, Mathf.Infinity) && !isOnDesk) {
        //            drawLine(laserStartPos, laserHit.point);
        //        }
        //        // check if on desk
        //        if (mouseHit.collider.gameObject.layer == LayerMask.NameToLayer("Desk")) {
        //            if (mouseHit.collider.gameObject.CompareTag("Bill") && billScript.inspectingBill) {
        //                isOnDesk = false;
        //                laserDot.SetActive(true);
        //            }
        //            else {
        //                isOnDesk = true;
        //            }
        //            laserDeskLocation = mouseHit.point;
        //            laserDot.transform.position = mouseHit.point;
        //            laserDot.transform.rotation = Quaternion.FromToRotation(laserDot.transform.up, mouseHit.normal) * laserDot.transform.rotation;
        //        }
        //        else {
        //            isOnDesk = false;
        //        }
        //    }
        //    else {
        //        float distance = 100f;
        //        drawLine(laserStartPos, ray.direction * distance);
        //        isOnDesk = false;
        //    }
        //}
        //else {
        //    isOnDesk = false;
        //}
    }

    void drawLine(Vector3 start, Vector3 end) {
        lineRender.SetPosition(0, start);
        lineRender.SetPosition(1, end);
    }

    // remove laser when its not the currently selected tool
    public void removeLaser() {
        isOnDesk = false;
        if (laserDot) { laserDot.SetActive(false); }
        if (lineObj) { lineObj.SetActive(false); }
    }
}
