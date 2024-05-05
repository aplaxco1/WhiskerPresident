using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionManager : MonoBehaviour
{

    public List<DistractionClass> distractionObjects;
    public List<DistractionClass> activeDistractions;

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        checkActiveDistractions();
    }

    void checkActiveDistractions() {
        foreach (DistractionClass distraction in distractionObjects) {
            if (distraction.isActive && !activeDistractions.Contains(distraction)) {
                activeDistractions.Add(distraction);
            }
        }

        for (int i = 0; i < activeDistractions.Count; i += 1) {
            if (!activeDistractions[i].isActive) {
                activeDistractions.RemoveAt(i);
            }
        }
    }
}
