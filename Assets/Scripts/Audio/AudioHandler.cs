using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : Singleton<AudioHandler>
{

    [SerializeField]
    AudioMixer m_GameAudioMixer;
    
    void Start()
    {
        LoadAudioSettings();
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        m_GameAudioMixer.SetFloat("SFXVolume", ScalarToDecibel(volume));
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        m_GameAudioMixer.SetFloat("MusicVolume", ScalarToDecibel(volume));
    }
    
    public void PlayClip(string clipName)
    {
        var audioSources = GetComponents<AudioSource>();

        foreach (AudioSource source in audioSources)
        {
            if (source.clip.name == clipName)
                source.Play();
        }
    }

    public void StopClip(string clipName)
    {
        var audioSources = GetComponents<AudioSource>();

        foreach (AudioSource source in audioSources)
        {
            if (source.clip.name == clipName)
                source.Stop();
        }
    }

    private void LoadAudioSettings()
    {
        SaveDataSettings settings = SaveData.LoadSettings();
        SetSFXVolume(settings.m_SFXVolume);
        SetMusicVolume(settings.m_MusicVolume);
    }

    // Helper to convert a scalar number to decibel used for the AudioMixer's sliders
    private float ScalarToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

}
