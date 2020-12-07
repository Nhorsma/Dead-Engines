using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource[] audioSources;
    public AudioSource cameraSource;
    public AudioSource encampmentSource;
    public AudioSource musicSource;

    public AudioClip ambientMusic;

    private void Start()
    {
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        musicSource.clip = ambientMusic;
    }

    public void volumeUI(float vol)
    {
        cameraSource.volume = vol;
    }

    public void volumeEncamp(float vol)
    {
        encampmentSource.volume = vol;
    }

    public void volumeMusic(float vol)
    {
        musicSource.volume = vol;
    }

}
