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
        // TESTING AUDIO MIXER
        audioMixer.SetFloat(mixerGroupName.ToString(), Mathf.Log10(sliderValue) * 20); // log operation is to compensate for how audio mixing works to allow for a smoother volume change

        //audioSource.volume = sliderValue;
        // What happens if the volume slider was not used to change the volume (i'm looking at you save manager)

        //if(!Mathf.Approximately(audioSource.volume, newValue))
        //{
        float newValue;
        audioMixer.GetFloat(mixerGroupName.ToString(), out newValue);
        //switch (mixerGroupName)
        //{
        //    case MixerGroups.MasterVolume: 
        //        audioMixer.GetFloat(mixerGroupName.ToString(), out newValue);
        //        masterVolumeSlider.value = newValue;
        //        break;
        //    case MixerGroups.MusicVolume:
        //        audioMixer.GetFloat(mixerGroupName.ToString(), out newValue);
        //        musicVolumeSlider.value = newValue;
        //        break;
        //    case MixerGroups:
        //        audioMixer.GetFloat(mixerGroupName.ToString(), out newValue);
        //        soundsVolumeSlider.value = newValue;
        //        break;
        //}
        //}

        return newValue;
    }

    public void UpdateSliders()
    {
        audioMixer.GetFloat(MixerGroups.MasterVolume.ToString(), out float master);
        audioMixer.GetFloat(MixerGroups.MusicVolume.ToString(), out float music);
        audioMixer.GetFloat(MixerGroups.SoundsVolume.ToString(), out float sound);
        masterVolumeSlider.value = master;
        musicVolumeSlider.value = music;
        soundsVolumeSlider.value = sound;
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
