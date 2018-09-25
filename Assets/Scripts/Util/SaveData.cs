using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{

    public enum DataString
    {
        SFXVolume,
        MusicVolume,
        GraphicsLevel,
    }

    public static void SaveSettings(float sfxVolume, float musicVolume, int graphicsLevel)
    {
        PlayerPrefs.SetFloat(DataString.SFXVolume.ToString(), sfxVolume);
        PlayerPrefs.SetFloat(DataString.MusicVolume.ToString(), musicVolume);
        PlayerPrefs.SetInt(DataString.GraphicsLevel.ToString(), graphicsLevel);

    }

}
