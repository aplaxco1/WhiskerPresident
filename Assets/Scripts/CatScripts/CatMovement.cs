using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/*
    Kirsten wrote this. ask me if u have any questions :]
    This script is meant to make the cat's arm rotate to face and periodically smack the attention point
    - it also creates a pawprint object where it smacks
    notes:
        -   should clamp head rotation so it doesnt clip with the body
        -   should probably limit the arm extension more cuz you can kind of tell that it isnt connected to the body when stretched
            (alternatively could make arm longer)
*/
public class CatMovement : MonoBehaviour
{
    // REFERENCES TO GAME OBJECTS
    public GameObject AttentionPoint;       // reference to where the cat's attention is
    public GameObject BasePivot;             // reference to the president
    public GameObject ArmPivot;             // reference to where the "Arm" component of president
    public GameObject ArmMesh;              // reference to the "Mesh" child of "Arm"
    public GameObject HeadPivot;            // reference to where the "Head" component of president
    public FocusController focusController; // reference to the script that manages the cat's focus
    public PawCollision pawCollisionDetection;
    public GameObject PawPrintPrefab;             // reference to pawprint prefab

    // VARIABLES
    public float timer = 0;                 // timer to keep track of how long since last smack
    private Quaternion target_rotation;     // arm rotation to move to
    private float x_rotate = 0;             // target x value of arm rotation
    private float y_rotate = 0;             // target y value of arm rotation
    private float x2_rotate = 0;             // target x value of arm rotation when smacked
    private float armExtension;     // how far the arm is stretched out
    private Vector3 lookTarget;             // position of the attentionpoint, but sometimes lags behind for flavor
    private bool smacking;                  // bool to ensure only one pawprint is left
    private BoxCollider pawCollider;        // reference to the paw's box collider
    private Color printColor;
    private int numPrints;
    private ParticleSystem dust;
    public ParticleSystem indicator;
    public Material Checkmark;
    public Material X;

    // CONSTANTS    
    private const float WaitInterval = 1.75f;       // base time to wait between swings
    private const float SmackRecoveryTime = 0.3f;   // time for paw to linger on table
    private const float SmackWarningTime = 0.5f;    // warning time to raise paw
    private const float SmackLockTime = 0.25f;      // time before smack when attention no longer moves
    private const float BaseArmExtension = -0.77f;
    
	private Material _smearMat = null;

    // FUNCTIONS
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 30;
        // instantiate pawprint objects
        smacking = false;
        pawCollider = ArmMesh.GetComponentInChildren<BoxCollider>();
        printColor = new Color(0,0,0,0);
        numPrints = 0;
        _smearMat = ArmMesh.GetComponent<Renderer>().material;
        dust = ArmPivot.transform.parent.parent.GetComponentInChildren<ParticleSystem>(true);
        //Debug.Log(dust);
        armExtension = BaseArmExtension;
        _smearMat.SetFloat("_Smearing",0);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // increment to smacking time

        float focusLevel = focusController.focusLevel;

        // initiate the smacking
        if (timer >= WaitInterval/focusLevel) { Smack(); }

        // update arm positioning goal
        TrackFocus(focusLevel);

        // when not recovering from smacking, rotate the arm to track the lookTarget
        MoveArm(focusLevel);

        // turn the head to face the lookTarget
        MoveHead();
    }
    void Smack()
    {
        //target_rotation = Quaternion.Euler(-3 - Mathf.Abs(BaseArmExtension/armExtension), target_rotation.eulerAngles.y, 0); // slam into the table
        smacking = true;
        _smearMat.SetFloat("_Smearing",1);
        timer = 0;
        //Debug.Log(target_rotation.eulerAngles.x);
        //Debug.Log((BaseArmExtension/armExtension));
        //_smearMat.SetFloat("_Smearing",1);
    }
    void TrackFocus(float focusLevel)
    {
        // when smacking is imminent, raise arm up as a warning
        x_rotate = (timer >= WaitInterval/focusLevel - SmackWarningTime/focusLevel) ? 20f : 5f;

        // dont adjust look_target right before smacking, that way it lingers a little bit behind
        if (timer < WaitInterval/focusLevel - SmackLockTime/focusLevel) {
            lookTarget = AttentionPoint.transform.position;
        } else {
            //_smearMat.SetFloat("_Smearing",1);
        }
    }
    void MoveArm(float focusLevel)
    {
        if (timer > SmackRecoveryTime/focusLevel) {
            // leave a pawprint behind at the end of the smack
            if (smacking) { 
                LeavePrint(pawCollider.transform.position, y_rotate);
                _smearMat.SetFloat("_Smearing",0);
                //dust.gameObject.SetActive(false);
            }

            // calculate and rotate arm pivot
            pivot();

            // calculate and extend/retract arm mesh
            armExtension = BaseArmExtension - Mathf.Clamp(Vector3.Distance(ArmPivot.transform.position, lookTarget)-2.1f, -0.8f, 0.2f);
            ArmMesh.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(ArmMesh.transform.localPosition.z, armExtension, Time.deltaTime*10f));
        } else { // during smack!
            if (pawCollisionDetection.colliding)
            {
                //_smearMat.SetFloat("_Smearing",0);
                x2_rotate = Quaternion.LookRotation((ArmPivot.transform.position - pawCollisionDetection.collisionPos).normalized).eulerAngles.x;
                target_rotation = Quaternion.Euler(x_rotate, target_rotation.eulerAngles.y, 0);
                if (!dust.gameObject.activeSelf) {
                    if (pawCollisionDetection.surface.tag == "Desk") {
                        dust.GetComponent<ParticleSystem>().emission.SetBursts(new ParticleSystem.Burst[]{new ParticleSystem.Burst(0.05f, 5, 10)});
                    } else {
                        dust.GetComponent<ParticleSystem>().emission.SetBursts(new ParticleSystem.Burst[]{new ParticleSystem.Burst(0.05f, 3, 5)});
                    }
                    Vector3 pos = pawCollider.transform.position;
                    dust.transform.position = new Vector3(pos.x, pos.y + 0.018f, pos.z);
                    dust.gameObject.SetActive(true);
                }
                if (!indicator.gameObject.activeSelf && pawCollisionDetection.surface.tag == "Bill" && printColor.a>0) {
                    indicator.GetComponent<ParticleSystem>().emission.SetBursts(new ParticleSystem.Burst[]{new ParticleSystem.Burst(0, 1)});
                    Renderer rend = indicator.GetComponent<Renderer>();
                    if (printColor.r > 0.6) {
                        rend.material = X;
                    } else {
                        rend.material = Checkmark;
                    }
                    Vector3 pos = pawCollider.transform.position;
                    indicator.transform.position = new Vector3(pos.x, pos.y + 0.018f, pos.z);
                    indicator.gameObject.SetActive(true);
                }
            }
            target_rotation = Quaternion.Euler(x2_rotate, y_rotate, 0);
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, Time.deltaTime*50f);
           
        }
    }
    float fix(float n) {
		float a = n <= 180 ? n : (360-n)*-1;
		return a;
	}
    void pivot() {
        Quaternion tempAngle = Quaternion.LookRotation((ArmPivot.transform.position - lookTarget).normalized);
        y_rotate = tempAngle.eulerAngles.y;
        float a = fix(ArmPivot.transform.localEulerAngles.y);
        if (Mathf.Abs(a-5) > 5) {
            Quaternion baseTarget = Quaternion.Euler(0, y_rotate, 0);
            BasePivot.transform.rotation = Quaternion.Slerp(BasePivot.transform.rotation, baseTarget, Time.deltaTime*Mathf.Abs(a-10)/20f);

            tempAngle = Quaternion.LookRotation((ArmPivot.transform.position - lookTarget).normalized);
            y_rotate = tempAngle.eulerAngles.y;
        }
        x2_rotate = tempAngle.eulerAngles.x;
	    target_rotation = Quaternion.Euler(x_rotate, y_rotate, 0);
        ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, Time.deltaTime*5f);
    }
    void MoveHead()
    {
        HeadPivot.transform.rotation = Quaternion.Slerp(HeadPivot.transform.rotation, Quaternion.LookRotation((HeadPivot.transform.position - lookTarget).normalized), Time.deltaTime*5f);
    }
    private Transform attached = null;
    void LeavePrint(Vector3 pos, float yRotation)
    {
        smacking = false;
        if (pawCollisionDetection.surface == null) { Debug.Log("gone :("); return; }
        if (pawCollisionDetection.surface.CompareTag("Inkpad")) {
            printColor = pawCollisionDetection.surface.GetComponentInParent<MeshRenderer>().material.color;
            return;
        }
        if (pawCollisionDetection.surface.CompareTag("Organizer")) { return; } // get rid of this
        if (printColor.a == 0) {return;}
        if (attached) { // release glue!
            attached.parent = null;
            attached.position = new Vector3(attached.position.x, attached.position.y+ 0.08f, attached.position.z);
            attached.rotation = Quaternion.Euler(0, attached.rotation.eulerAngles.y + Random.Range(-45f, 45f), 0);
            attached.GetComponentInChildren<Collider>().enabled = true;
            attached = null;
            return;
        }
        if (printColor.r < 0.6 && !attached) { // glue!
            if (pawCollisionDetection.surface.CompareTag("Bill")) {
                Debug.Log("sticking!");
                attached = pawCollisionDetection.surface.GetComponentInParent<BillController>().transform;
                attached.parent = pawCollider.transform;
                attached.position = new Vector3(attached.position.x, attached.position.y - 0.07f, attached.position.z);
                pawCollisionDetection.surface.GetComponent<Collider>().enabled = false;
            }
            return;
        }
        GameObject newPrint = Instantiate(PawPrintPrefab, new Vector3(pos.x, pos.y + 0.018f, pos.z), Quaternion.Euler(0,yRotation,0));
        newPrint.transform.SetParent(pawCollisionDetection.surface.transform, true);
        PawPrint script = newPrint.GetComponent<PawPrint>();
        foreach (Material stencil in pawCollisionDetection.surface.GetComponentInParent<MeshRenderer>().materials) {
            script.StencilID = stencil.GetFloat("_StencilID");
        }
        //if (pawCollisionDetection.surface.CompareTag("Bill")) {
            //Debug.Log("bill");
            script.DisappearanceRate = 0.0f;
        //}
        script.color = printColor;
        printColor = new Color(printColor.r,printColor.g,printColor.b,printColor.a-0.25f);
        script.renderQueue = numPrints;
        numPrints++;
    }
}