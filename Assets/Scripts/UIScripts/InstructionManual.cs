using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionManual : MonoBehaviour
{
    public GameObject[] imageArray;
    public int currentImage;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < imageArray.Length; i++)
        {
            imageArray[i].SetActive(false);
        }
        currentImage = 0;
        imageArray[0].SetActive(true);
    }
    
    public void NextImage ()
    {
        if (currentImage < imageArray.Length-1)
        {
            imageArray[currentImage].SetActive(false);
            currentImage++;
            imageArray[currentImage].SetActive(true);


        }
    }

    public void PrevImage ()
    {
        if (currentImage > 0)
        {
            imageArray[currentImage].SetActive(false);
            currentImage--;
            imageArray[currentImage].SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
