﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : Singleton<SceneHandler>
{

    public enum SceneType : int
    {
        MainMenu,
        Level1,
        Level2,
    }

    public void ChangeSceneAsync(SceneType sceneType, System.Action onComplete)
    {
        Debug.Log("Changing Scene");

        string sceneName = "";

        switch (sceneType)
        {
            case SceneType.MainMenu:
                sceneName = "MainMenu";
                break;
            case SceneType.Level1:
                sceneName = "Level1";
                break;
            case SceneType.Level2:
                sceneName = "Level2";
                break;
        }

        StartCoroutine(StartLoading(sceneName, onComplete));
    }

    private IEnumerator StartLoading(string sceneName, System.Action onComplete)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);
        
        while (!operation.isDone)
            yield return null;

        Debug.Log("Loaded Scene: " + sceneName);

        if (onComplete != null)
            onComplete.Invoke();
    }

}