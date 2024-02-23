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
        bool registerCollision = !colliding;
        if (colliding) {
            if (other.gameObject.CompareTag("Desk") && surface.CompareTag("Bill")) {
                Bounds otherCollider = surface.GetComponent<BoxCollider>().bounds;
                Bounds newBounds = new Bounds(otherCollider.center, new Vector3(otherCollider.size.x, 20, otherCollider.size.z)); 
                if(!newBounds.Contains(gameObject.transform.position)){ registerCollision = true; }
            } else if (other.gameObject.CompareTag("Bill")) {
                Bounds newBounds = new Bounds(other.bounds.center, new Vector3(other.bounds.size.x, 20, other.bounds.size.z));
                if(newBounds.Contains(gameObject.transform.position)){ registerCollision = true; }
            } else if (!other.gameObject.CompareTag("Desk")) { registerCollision = true; }
        }
        if (registerCollision) {
            colliding = true;
            collisionPos = new Vector3(gameObject.transform.position.x, other.bounds.center.y + other.bounds.extents.y + 0.02f, gameObject.transform.position.z);
            surface = other.gameObject;
            Debug.Log(other.tag);
            //StencilID = other.gameObject.GetComponentInParent<MeshRenderer>().material.GetFloat("_StencilID");
        }
    }
    void OnTriggerExit(Collider other) {
        //Debug.Log("leaves Collider");
        colliding = false;
    }
}
