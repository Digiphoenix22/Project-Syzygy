using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio; // Required for dealing with audio

public class AudioController : MonoBehaviour
{
    public AudioMixer musicMixer; // Drag your MusicMixer here

    void Start()
    {
        SetMusicVolume(-40); // Set volume (in decibels)
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("MusicVolume", volume);
    }
}

