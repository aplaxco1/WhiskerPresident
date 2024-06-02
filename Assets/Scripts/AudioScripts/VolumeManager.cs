using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    public AudioClip[] soundtrack;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundsVolumeSlider;

    public float currentMasterVolume;
    public float currentMusicVolume;
    public float currentSoundsVolume;

    AudioSource audioSource;

    public AudioMixer audioMixer;
    //public string mixerGroupName;
    enum MixerGroups{ MasterVolume, MusicVolume, SoundsVolume }


    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();

        //if (!audioSource.playOnAwake)
        //{
        //    audioSource.clip = soundtrack[Random.Range(0, soundtrack.Length)];
        //    audioSource.Play();
        //}

        //if (SaveManager.Instance.currentSettingsData != null)
        //{
        //    volumeSlider.value = SaveManager.Instance.currentSettingsData.volume;
        //}
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (!audioSource.isPlaying)
    //    {
    //        audioSource.clip = soundtrack[Random.Range(0, soundtrack.Length)];
    //        audioSource.Play();
    //    }
    //}

    //void OnEnable()
    //{
    //    //Register Slider Events
    //    volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(volumeSlider.value); });
    //}

    public void ChangeMasterVolume(float newVolume)
    {
        currentMasterVolume = ChangeVolume(MixerGroups.MasterVolume, newVolume);
    }

    public void ChangeMusicVolume(float newVolume)
    {
        currentMusicVolume = ChangeVolume(MixerGroups.MusicVolume, newVolume);
    }

    public void ChangeSoundsVolume(float newVolume)
    {
        currentSoundsVolume = ChangeVolume(MixerGroups.SoundsVolume, newVolume);
    }

    //Called when Slider is moved
    float ChangeVolume(MixerGroups mixerGroupName, float sliderValue)
    {
        audioMixer.SetFloat(mixerGroupName.ToString(), Mathf.Log10(sliderValue) * 20); // log operation is to compensate for how audio mixing works to allow for a smoother volume change
        return sliderValue;
    }

    public void UpdateSliders()
    {
        masterVolumeSlider.value = currentMasterVolume;
        musicVolumeSlider.value = currentMusicVolume;
        soundsVolumeSlider.value = currentSoundsVolume;
    }

    //void OnDisable()
    //{
    //    //Un-Register Slider Events
    //    volumeSlider.onValueChanged.RemoveAllListeners();
    //}

    // For use with the real volumeslider (i hope it works)
    // Update the audioSorce volume with the value of the slider
    //public void UpdateVolume()
    //{
    //    audioSource.volume = currentVolume = volumeSlider.value;
    //}
}
