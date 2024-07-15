using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // NOTE: Used tiny parts of my other Unity project's audio code here -Justin

    public readonly Dictionary<SoundName, AudioClip> soundToClip =
        new Dictionary<SoundName, AudioClip>();

    public static AudioManager Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        
        foreach (SoundName sound in Enum.GetValues(typeof(SoundName)))
        {
            var clip = Resources.Load<AudioClip>("Sounds/" + sound.ToString().ToLower());
            soundToClip[sound] = clip;
        }

        audioSource = GetComponent<AudioSource>();
    }
    
    public void Play(SoundName sound, float volume)
    {
        var clip = soundToClip[sound];
        audioSource.PlayOneShot(clip, volume);
    }


    // for buttons calling specific sfx
    public void PlayMeowButton() {
        Play(SoundName.kitty_meow, 0.5f);
    }

    public void PlayButton() {
        Play(SoundName.button, 0.5f);
    }

}
