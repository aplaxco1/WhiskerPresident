using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPawCollision : MonoBehaviour
{
    public bool colliding = false;
    public Vector3 collisionPos;
    public GameObject surface;
    //public float StencilID;
    void OnTriggerEnter(Collider other) {
        bool registerCollision = !colliding;
        if (colliding) {
            if (other.gameObject.CompareTag("Desk") && surface.CompareTag("Bill")) {
                Bounds otherCollider = surface.GetComponent<BoxCollider>().bounds;
                Bounds newBounds = new Bounds(otherCollider.center, new Vector3(otherCollider.size.x*1.1f, 20, otherCollider.size.z*1.1f)); 
                if(!newBounds.Contains(gameObject.transform.position)){ registerCollision = true; }
            } else if (other.gameObject.CompareTag("Bill")) {
                Bounds newBounds = new Bounds(other.bounds.center, new Vector3(other.bounds.size.x*1.1f, 20, other.bounds.size.z*1.1f));
                if(newBounds.Contains(gameObject.transform.position)){ registerCollision = true; }
            } else if (!other.gameObject.CompareTag("Desk")) { registerCollision = true; }
        }
        if (registerCollision) {
            AudioManager.Instance.Play(SoundName.wood_cut, 0.5f);
            colliding = true;
            collisionPos = new Vector3(gameObject.transform.position.x, other.bounds.center.y + other.bounds.extents.y + 0.02f, gameObject.transform.position.z);
            surface = other.gameObject;
            //Debug.Log(other.tag);
            //StencilID = other.gameObject.GetComponentInParent<MeshRenderer>().material.GetFloat("_StencilID");
        }
    }
    void OnTriggerExit(Collider other) {
        //Debug.Log("leaves Collider");
        colliding = false;
    }
}
