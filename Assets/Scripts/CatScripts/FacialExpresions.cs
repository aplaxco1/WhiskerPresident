using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpresions : MonoBehaviour
{

    [Header("Facial Expressions")]
    public GameObject catHead;
    public Material idleFace;
    public Material idleFaceBlink;
    public Material happyFace;
    public Material frenzyFace;

    private float timeTillBlink;
    private float blinkTime;

    [Header("Distraction Reference")]
    public DistractionManager distractions;

    [Header("Cat Movement Reference")]
    public CatMovement catMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        timeTillBlink = 2f;
        blinkTime = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (distractions.frenzyActive) {
            catHead.GetComponent<MeshRenderer>().material = frenzyFace;
        }
        else if (catMovement.holdingPhone) {
            catHead.GetComponent<MeshRenderer>().material = happyFace;
        }
        else {
            // change back to idle
            if (timeTillBlink > 0f) {
                catHead.GetComponent<MeshRenderer>().material = idleFace; 
            }

            // blinking animation
            timeTillBlink -= Time.deltaTime;
            if (timeTillBlink < 0f) {
                blink();
            }
        }
    }


    void blink() {
        blinkTime -= Time.deltaTime;
        catHead.GetComponent<MeshRenderer>().material = idleFaceBlink;
        if (blinkTime < 0f) {
            timeTillBlink = 2f;
            blinkTime = 0.2f;
            catHead.GetComponent<MeshRenderer>().material = idleFace;
        }
    }
}
