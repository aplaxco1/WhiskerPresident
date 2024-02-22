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
    public GameObject ArmPivot;             // reference to where the "Arm" component of president
    public GameObject ArmMesh;              // reference to the "Mesh" child of "Arm"
    public GameObject HeadPivot;            // reference to where the "Head" component of president
    public FocusController focusController; // reference to the script that manages the cat's focus
    public GameObject PawPrint;             // reference to pawprint prefab

    // VARIABLES
    public float timer = 0;                 // timer to keep track of how long since last smack
    private Quaternion target_rotation;     // arm rotation to move to
    private float x_rotate = 0;             // target x value of arm rotation
    private float y_rotate = 0;             // target y value of arm rotation
    private float armExtension = -0.8f;     // how far the arm is stretched out
    private Vector3 lookTarget;             // position of the attentionpoint, but sometimes lags behind for flavor
    private bool smacking;                  // bool to ensure only one pawprint is left
    private BoxCollider pawCollider;        // reference to the paw's box collider

    // CONSTANTS    
    private const float WaitInterval = 1.75f;       // base time to wait between swings
    private const float SmackRecoveryTime = 0.3f;   // time for paw to linger on table
    private const float SmackWarningTime = 0.5f;    // warning time to raise paw
    private const float SmackLockTime = 0.25f;      // time before smack when attention no longer moves

    // FUNCTIONS
    // Start is called before the first frame update
    void Start()
    {
        // instantiate pawprint objects
        smacking = false;
        pawCollider = ArmMesh.GetComponentInChildren<BoxCollider>();
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
        target_rotation = Quaternion.Euler(-2 - (-0.8f/armExtension), target_rotation.eulerAngles.y, 0); // slam into the table
        smacking = true;
        timer = 0;
    }
    void TrackFocus(float focusLevel)
    {
        // when smacking is imminent, raise arm up as a warning
        x_rotate = (timer >= WaitInterval/focusLevel - SmackWarningTime/focusLevel) ? 20f : 5f;

        // dont adjust look_target right before smacking, that way it lingers a little bit behind
        if (timer < WaitInterval/focusLevel - SmackLockTime/focusLevel) {
            lookTarget = AttentionPoint.transform.position;
        }
    }
    void MoveArm(float focusLevel)
    {
        if (timer > SmackRecoveryTime/focusLevel) {
            // leave a pawprint behind at the end of the smack
            if (smacking) { LeavePrint(pawCollider.transform.position, y_rotate); }

            // calculate and rotate arm pivot
            y_rotate = Quaternion.LookRotation((ArmPivot.transform.position - lookTarget).normalized).eulerAngles.y;
		    target_rotation = Quaternion.Euler(x_rotate, y_rotate, 0);
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, Time.deltaTime*5f);

            // calculate and extend/retract arm mesh
            armExtension = -0.8f - Mathf.Clamp(Vector3.Distance(ArmPivot.transform.position, lookTarget)-2.1f, -0.5f, 0.2f);
            ArmMesh.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(ArmMesh.transform.localPosition.z, armExtension, Time.deltaTime*10f));
        } else { // during smack!
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, Time.deltaTime*50f);
        }
    }
    void MoveHead()
    {
        HeadPivot.transform.rotation = Quaternion.Slerp(HeadPivot.transform.rotation, Quaternion.LookRotation((HeadPivot.transform.position - lookTarget).normalized), Time.deltaTime*5f);
    }
    void LeavePrint(Vector3 pos, float yRotation)
    {
        Instantiate(PawPrint, pos, Quaternion.Euler(0,yRotation,0));
        smacking = false;
    }
}