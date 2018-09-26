using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer
{

    List<AudioSource> m_AudioList = new List<AudioSource>();

    public AudioPlayer(Entity entity)
    {
        m_AudioList.AddRange(entity.GetComponents<AudioSource>());
    }

    public void PlayClip(string clipName, float pitch = 1)
    {
        AudioSource audio = m_AudioList.Find((audioSource) => audioSource.clip.name == clipName);

        if (audio == null)
        {
            Debug.LogError("Failed to play the AudioClip: " + clipName);
        }
        else
        {
            audio.pitch = pitch;
            audio.Play();
        }
    }

}
