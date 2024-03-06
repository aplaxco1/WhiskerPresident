using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackSounds : MonoBehaviour
{
    public static AudioSource smackSource;
    // Start is called before the first frame update
    void Start()
    {
        smackSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
