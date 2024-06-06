using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayProgressionManager : MonoBehaviour
{

    // This script is responsible for handling which distractions are active each day.

    public List<GameObject> distractions;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.Instance.currentSaveData.dayInfo.day <= 1) {
            toggleDistractions(0);
        }
        if (SaveManager.Instance.currentSaveData.dayInfo.day == 2) {
            toggleDistractions(1);
        }
        if (SaveManager.Instance.currentSaveData.dayInfo.day == 3) {
            toggleDistractions(2);
        }
        if (SaveManager.Instance.currentSaveData.dayInfo.day >= 4) {
            toggleDistractions(3);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void toggleDistractions(int numToggle) {
        for (int i = 0; i < distractions.Count; i += 1) {
            if ((i + 1) <= numToggle) {
                distractions[i].SetActive(true);
            }
            else{
                distractions[i].SetActive(false);
            }
        }
    }
}
