using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/*
    This script is meant to make the cat's arm rotate to face and periodically smack the attention point
*/
public class CatMovement : MonoBehaviour
{
    public GameObject AttentionPoint; // reference to where the cat's attention is
    public GameObject ArmPivot; // reference to where the "Arm" component of president
    public GameObject ArmMesh; // reference to the "Mesh" child of "Arm"
    private const int WaitInterval = 5000; // base time to wait between swings
    private int timer; // timer to keep track of how long till next swing
    private Quaternion target_rotation; // arm rotation to move to
    private int x_rotate = 0; // target x value of arm rotation
    private float y_rotate = 0; // target y value of arm rotation
    private float timeCount = 2.0f; // counting time during smack
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
            timer -= 1;
            if (timer <= 1000) {
                if (timer > 250) {
                    y_rotate = Quaternion.LookRotation((ArmPivot.transform.position - AttentionPoint.transform.position).normalized).eulerAngles.y;
                }
                x_rotate = 20;
            } else {
                y_rotate = Quaternion.LookRotation((ArmPivot.transform.position - AttentionPoint.transform.position).normalized).eulerAngles.y;
                x_rotate = 0;
            }
        }
        if (timeCount > 2.0f) {
		    target_rotation = Quaternion.Euler(x_rotate, y_rotate, 0);
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, 0.005f);
        } else {
            ArmPivot.transform.rotation = Quaternion.Slerp(ArmPivot.transform.rotation, target_rotation, 0.03f);
        }
        timeCount += Time.deltaTime;
    }
    void Smack() {
        //Debug.Log(target_rotation);
        timeCount = 0.0f;
        target_rotation = Quaternion.Euler(-5, target_rotation.eulerAngles.y, 0);
        //ArmPivot.transform.rotation = target_rotation;
    }
}
