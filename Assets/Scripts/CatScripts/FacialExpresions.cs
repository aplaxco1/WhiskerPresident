using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpresions : MonoBehaviour
{

    public GameObject catHead;
    public Material idleFace;
    public Material frenzyFace;

    [Header("Distraction Reference")]
    public DistractionManager distractions;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (distractions.frenzyActive) {
            if (catHead.GetComponent<MeshRenderer>().material != frenzyFace) { catHead.GetComponent<MeshRenderer>().material = frenzyFace; }
        }
        else {
            if (catHead.GetComponent<MeshRenderer>().material != idleFace) { catHead.GetComponent<MeshRenderer>().material = idleFace; }
        }
    }
}
