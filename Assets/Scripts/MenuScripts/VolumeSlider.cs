using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public static VolumeSlider Instance;
    
    public AudioClip[] soundtrack;
    public Slider volumeSlider;
    public float currentVolume;

    AudioSource audioSource;


    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (!audioSource.playOnAwake)
        {
            audioSource.clip = soundtrack[Random.Range(0, soundtrack.Length)];
            audioSource.Play();
        }

        if (SaveManager.Instance.currentSettings != null)
        {
            volumeSlider.value = SaveManager.Instance.currentSettings.volume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = soundtrack[Random.Range(0, soundtrack.Length)];
            audioSource.Play();
        }
    }

    void OnEnable()
    {
        //Register Slider Events
        volumeSlider.onValueChanged.AddListener(delegate { changeVolume(volumeSlider.value); });
    }

    //Called when Slider is moved
    public void changeVolume(float sliderValue)
    {
        audioSource.volume = sliderValue;
    }

    // For use with the real volumeslider (i hope it works)
    // Update the audioSorce volume with the value of the slider
    public void updateVolume()
    {
        audioSource.volume = currentVolume = volumeSlider.value;
    }

    void OnDisable()
    {
        //Un-Register Slider Events
        volumeSlider.onValueChanged.RemoveAllListeners();
    }
}
