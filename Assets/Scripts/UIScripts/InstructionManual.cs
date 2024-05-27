using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionManual : MonoBehaviour
{
    public GameObject[] imageArray;
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject exitButton;

    public int currentImage;
    private int currMinPage = 0;
    private int currMaxPage = 5;
    private bool gameStarted = true;

    public GameObject laserPointer;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < imageArray.Length; i += 1) {
            imageArray[i].SetActive(false);
        }

        // handle tutorial popups
        if (SaveManager.Instance.currentSaveData.dayInfo.day <= 4) { 

            // pause game
            Time.timeScale = 0;
            laserPointer.SetActive(false);
            gameStarted = false;

            // toggle popups based on day
            if (SaveManager.Instance.currentSaveData.dayInfo.day <= 1) {
                currMinPage = 0;
                currMaxPage = 2;
                currentImage = 0;
                imageArray[0].SetActive(true);
            }
            if (SaveManager.Instance.currentSaveData.dayInfo.day == 2) {
                currMinPage = 3;
                currMaxPage = 3;
                currentImage = 3;
                imageArray[3].SetActive(true);
            }
            if (SaveManager.Instance.currentSaveData.dayInfo.day == 3) {
                currMinPage = 4;
                currMaxPage = 4;
                currentImage = 4;
                imageArray[4].SetActive(true);
            }
            if (SaveManager.Instance.currentSaveData.dayInfo.day == 4) {
                currMinPage = 5;
                currMaxPage = 5;
                currentImage = 5;
                imageArray[5].SetActive(true);
            }

            if (currMinPage != currMaxPage) { exitButton.SetActive(false); }
        }
        else {
            imageArray[0].SetActive(true);
            gameObject.SetActive(false);
        }
    }
    
    public void NextImage ()
    {
        if (currentImage < currMaxPage)
        {
            imageArray[currentImage].SetActive(false);
            currentImage++;
            imageArray[currentImage].SetActive(true);
            // make sure players go through all tutorial pages before starting game
            if (!gameStarted && currentImage == currMaxPage) { exitButton.SetActive(true); }
        }
        
    }

    public void PrevImage ()
    {
        if (currentImage > currMinPage)
        {
            imageArray[currentImage].SetActive(false);
            currentImage--;
            imageArray[currentImage].SetActive(true);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (currMaxPage == currMinPage) {
            nextButton.SetActive(false);
            prevButton.SetActive(false);
        }
        else if (currentImage == currMinPage && currentImage != currMaxPage)
        {
            nextButton.SetActive(true);
            prevButton.SetActive(false);
        }
        else if (currentImage == currMaxPage && currentImage != currMinPage)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(true);
        }
        else {
            nextButton.SetActive(true);
            prevButton.SetActive(true);
        }
    }

    // called when exit button is pressed for the first time
    public void StartGame() {
        if (!gameStarted) {
            gameStarted = true;
            if (SaveManager.Instance.currentSaveData.dayInfo.day <= 1) {
                currMinPage = 0;
                currMaxPage = 2;
            }
            else if (SaveManager.Instance.currentSaveData.dayInfo.day == 2) {
                currMinPage = 0;
                currMaxPage = 3;
            }
            else if (SaveManager.Instance.currentSaveData.dayInfo.day == 3) {
                currMinPage = 0;
                currMaxPage = 4;
            }
            else if (SaveManager.Instance.currentSaveData.dayInfo.day >= 4) {
                currMinPage = 0;
                currMaxPage = 5;
            }
        }
    }
}
