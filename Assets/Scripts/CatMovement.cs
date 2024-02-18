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
*/
public class CatMovement : MonoBehaviour
{
    public GameObject AttentionPoint; // reference to where the cat's attention is
    public GameObject ArmPivot; // reference to where the "Arm" component of president
    public GameObject ArmMesh; // reference to the "Mesh" child of "Arm"
    private const float WaitInterval = 5.0f; // base time to wait between swings
    private float timer; // timer to keep track of how long till next swing
    private Quaternion target_rotation; // arm rotation to move to
    private float x_rotate = 0; // target x value of arm rotation
    private float y_rotate = 0; // target y value of arm rotation
    private float timeCount = 2.0f; // counting time during smack
    private float armExtension = -0.8f;
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
            if (timer <= 1.0f) { // when smacking is imminent, raise arm up as a warning (x_rotate)
                if (timer > 0.25f) {
                    y_rotate = Quaternion.LookRotation((ArmPivot.transform.position - AttentionPoint.transform.position).normalized).eulerAngles.y;
                }
                // dont adjust y_rotate right before smacking, that way it lingers a little bit behind
                x_rotate = 20f;
            } else {
                y_rotate = Quaternion.LookRotation((ArmPivot.transform.position - AttentionPoint.transform.position).normalized).eulerAngles.y;
                x_rotate = 0;
            }
        }
        if (timeCount > 1.0f) {
		    target_rotation = Quaternion.Euler(x_rotate, y_rotate, 0);
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, 0.005f);
            ArmMesh.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(ArmMesh.transform.localPosition.z, armExtension, 0.01f)); // extend/retract arm
        } else { // during smack!
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, 0.03f);
        }
        armExtension = -0.8f - Mathf.Clamp(Vector3.Distance(ArmPivot.transform.position, AttentionPoint.transform.position)-2.1f, -0.5f, 0.2f); // calculate arm extension
        timeCount += Time.deltaTime;
    }
    void Smack() {
        timeCount = 0.0f;
        target_rotation = Quaternion.Euler(-5, target_rotation.eulerAngles.y, 0); // slam into the table
    }
}
