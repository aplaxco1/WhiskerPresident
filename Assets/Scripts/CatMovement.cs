using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/*
    Kirsten wrote this. ask me if u have any questions :]
    This script is meant to make the cat's arm rotate to face and periodically smack the attention point
    notes:
        -   should probably rename timer and timeCount variables to better distinguish their uses
            (countdown till smack and time since smack, respectively)
        -   should clamp head rotation so it doesnt clip with the body
*/
public class CatMovement : MonoBehaviour
{
    // REFERENCES TO GAME OBJECTS
    public GameObject AttentionPoint; // reference to where the cat's attention is
    public GameObject ArmPivot; // reference to where the "Arm" component of president
    public GameObject ArmMesh; // reference to the "Mesh" child of "Arm"
    public GameObject HeadPivot; // reference to where the "Head" component of president

    // CONSTANTS
    public float WaitInterval = 1.75f; // base time to wait between swings
    public float SmackRecoveryTime = 0.3f; // time for paw to linger on table
    public float SmackWarningTime = 0.5f; // warning time to raise paw
    public float SmackLockTime = 0.25f; // time before smack when attention no longer moves

    // VARIABLES
    private float timer; // timer to keep track of how long till next swing
    private Quaternion target_rotation; // arm rotation to move to
    private float x_rotate = 0; // target x value of arm rotation
    private float y_rotate = 0; // target y value of arm rotation
    private float timeCount = 1.0f; // counting time during smack
    private float armExtension = -0.8f; // how far the arm is stretched out
    private Vector3 lookTarget;

    // FUNCTIONS
    // Start is called before the first frame update
    void Start()
    {
        timer = WaitInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0) {
            timer = WaitInterval + (int) (Random.value * WaitInterval);
            Smack(); // initiate the smacking
        } else {
            timer -= Time.deltaTime; // increment countdown to smacking
            if (timer <= SmackWarningTime) { // when smacking is imminent, raise arm up as a warning (x_rotate)
                x_rotate = 20f;
            } else {
                x_rotate = 0;
            }
        }
        if (timer > SmackLockTime) { // dont adjust look_target right before smacking, that way it lingers a little bit behind
            lookTarget = AttentionPoint.transform.position;
        }
        y_rotate = Quaternion.LookRotation((ArmPivot.transform.position - lookTarget).normalized).eulerAngles.y;

        armExtension = -0.8f - Mathf.Clamp(Vector3.Distance(ArmPivot.transform.position, lookTarget)-2.1f, -0.5f, 0.2f); // calculate arm extension

        if (timeCount > SmackRecoveryTime) {
		    target_rotation = Quaternion.Euler(x_rotate, y_rotate, 0);
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, 0.005f);
            ArmMesh.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(ArmMesh.transform.localPosition.z, armExtension, 0.01f)); // extend/retract arm
        } else { // during smack!
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, 0.05f);
        }
        timeCount += Time.deltaTime;

        HeadPivot.transform.rotation = Quaternion.Slerp(HeadPivot.transform.rotation, Quaternion.LookRotation((HeadPivot.transform.position - lookTarget).normalized), 0.005f); // look at the attention target
    }
    void Smack() {
        timeCount = 0.0f;
        target_rotation = Quaternion.Euler(-5, target_rotation.eulerAngles.y, 0); // slam into the table
    }
}
