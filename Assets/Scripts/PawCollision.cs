using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawCollision : MonoBehaviour
{
    public bool colliding = false;
    public Vector3 collisionPos;
    public GameObject surface;
    //public float StencilID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other) {
        if (!colliding || !other.gameObject.CompareTag("Desk")) {
            colliding = true;
            collisionPos = new Vector3(gameObject.transform.position.x, other.bounds.center.y + other.bounds.extents.y + 0.02f, gameObject.transform.position.z);
            surface = other.gameObject;
            Debug.Log(collisionPos);
            //StencilID = other.gameObject.GetComponentInParent<MeshRenderer>().material.GetFloat("_StencilID");
        }
    }
    void OnTriggerExit(Collider other) {
        //Debug.Log("leaves Collider");
        colliding = false;
    }
}
