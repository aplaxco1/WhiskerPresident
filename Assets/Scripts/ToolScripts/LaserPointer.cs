using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class LaserPointer : ToolClass
{
    [Header("Adjustable Laser Variables")]
    [SerializeField]
    public float laserWidth = 0.01f;
    [SerializeField]
    public Vector3 laserOffset = new Vector3(0.0f, -0.45f, 0.0f);
    [SerializeField]
    public GameObject laserDot;
    public Material laserMaterial;

    [Header("Global Laser Variables")]
    // variables to capture location on desk for cats attention
    [SerializeField]
    public bool isOnDesk = false;
    [SerializeField]
    public Vector3 laserDeskLocation = new Vector3(0f, 0f, 0f);
    private Vector3 previousPointerLocation = new Vector3(0, 0, 0);
    private bool wasOnTable = false;
    public float pointerSpeed = 0;
    public float attentionLevel = 0f; // float between 0 and 1

    public BillMovement billScript;

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

        if (isActive) {
            // set laser dot active if laser is on desk
            laserDot.SetActive(isOnDesk);
            lineObj.SetActive(true);

            // update pointer speed and corresponding attention level if on desk
            if (isOnDesk) { updatePointerSpeed(); updateAttentionLevel();}

            // create ray from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // calculate position of visual laser
            Vector3 laserStartPos = new Vector3(Camera.main.transform.position.x + laserOffset.x, Camera.main.transform.position.y + laserOffset.y, Camera.main.transform.position.z + laserOffset.z);

            // determine raycast collision
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit)) {
                drawLine(laserStartPos, mouseHit.point);
                Vector3 direction = mouseHit.point - laserStartPos;
                RaycastHit laserHit;
                // make sure actual visual laser doesnt go through any objects
                if (Physics.Raycast(laserStartPos, direction, out laserHit, Mathf.Infinity) && !isOnDesk) {
                    drawLine(laserStartPos, laserHit.point);
                }
                // check if on desk
                if (mouseHit.collider.gameObject.layer == LayerMask.NameToLayer("Desk")) {
                    if (mouseHit.collider.gameObject.CompareTag("Bill") && billScript.inspectingBill) {
                        isOnDesk = false;
                        laserDot.SetActive(true);
                    }
                    else {
                        isOnDesk = true;
                    }
                    laserDeskLocation = mouseHit.point;
                    laserDot.transform.position = mouseHit.point;
                    laserDot.transform.rotation = Quaternion.FromToRotation(laserDot.transform.up, mouseHit.normal) * laserDot.transform.rotation;
                }
                else {
                    isOnDesk = false;
                    wasOnTable = false; 
                    pointerSpeed = 0;
                }
            }
            else {
                float distance = 100f;
                drawLine(laserStartPos, ray.direction * distance);
                isOnDesk = false;
                wasOnTable = false; 
                pointerSpeed = 0;
            }
        }
    }

    void drawLine(Vector3 start, Vector3 end) {
        lineRender.SetPosition(0, start);
        lineRender.SetPosition(1, end);
    }

    void updatePointerSpeed() {
        pointerSpeed = wasOnTable ? Mathf.Clamp((laserDeskLocation - previousPointerLocation).magnitude/Time.deltaTime, 0.0f, 16.0f) : 1;
        wasOnTable = true;
        previousPointerLocation = laserDeskLocation;
    }

    void updateAttentionLevel() {
        // handle attention level decrease/increase here
        if (pointerSpeed == 0f) {
            // decrease attention is laser held still
            attentionLevel -= 0.0005f;
        }
        else {
            attentionLevel += 0.01f;
        }

        attentionLevel = Mathf.Clamp(attentionLevel, 0f, 1f);
    }

    public void toggleOn() {
        if (attentionLevel == 0f) {
            // give some immediate attention when laser toggled on
            attentionLevel += 0.5f;
        } 
        else {
            // small attention boost
        }
    }

    // remove laser when its not the currently selected tool
    public void removeLaser() {
        isOnDesk = false;
        if (laserDot) { laserDot.SetActive(false); }
        if (lineObj) { lineObj.SetActive(false); }
    }
}
