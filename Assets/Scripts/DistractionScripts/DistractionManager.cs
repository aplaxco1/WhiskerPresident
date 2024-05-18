using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DistractionManager : MonoBehaviour
{

    public List<DistractionClass> distractionObjects;
    public List<DistractionClass> activeDistractions;
    public bool frenzyActive = false;

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

        // sorts list by highest attention value
        activeDistractions = activeDistractions.OrderBy (d => d.attentionLevel).ToList();

        checkFrenzy();
    }

    void checkFrenzy() {
        float frenzyDistractions = 0;
        for (int i = 0; i < activeDistractions.Count; i += 1) {
            if (activeDistractions[i].frenzyDistraction) {
                frenzyDistractions += 1;
            }
        }

        if (frenzyDistractions > 0) {
            frenzyActive = true;
        }
        else {
            frenzyActive = false;
        }
    }
}
