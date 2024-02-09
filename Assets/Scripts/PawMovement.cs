using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawMovement : MonoBehaviour
{
    private bool movingLeft;
    private int speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        movingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft == true)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= -4)
            {
                movingLeft = false;
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }
}
