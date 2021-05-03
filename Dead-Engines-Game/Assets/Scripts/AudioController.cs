using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource[] audioSources;
    AudioSource[] SFXSources;

    public AudioSource musicSource;

    private void Start()
    {
        UpdateAudioHandlers();
    }

    public void UpdateAudioHandlers()
    {
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];       
    }

    public void volumeSFX(float vol)
    {
        foreach(AudioSource audio in SFXSources)
        {
            if(audio.gameObject.tag != "MusicPlayer")
                audio.volume = vol;
        }
    }

    public void volumeMusic(float vol)
    {
        musicSource.volume = vol;
    }

}
