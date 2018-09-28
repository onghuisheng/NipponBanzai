using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : Singleton<SceneHandler>
{

    public enum SceneType : int
    {
        None,
        MainMenu,
        Bedroom,
        Level1,
        Level2,
        JJPlayground
    }

    private SceneType m_CurrentSceneType;

    protected override void Awake()
    {
        base.Awake();
        m_dontBringOverOnLoad = true;
    }

    public void ChangeSceneAsync(SceneType sceneType, System.Action onComplete = null)
    {
        Debug.Log("Changing Scene");

        string sceneName = "";

        switch (sceneType)
        {
            case SceneType.MainMenu:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                sceneName = "MainMenu";
                break;
            case SceneType.Bedroom:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                sceneName = "Bedroom";
                break;
            case SceneType.Level1:
                sceneName = "Level1";
                break;
            case SceneType.Level2:
                sceneName = "Level2";
                break;
            case SceneType.JJPlayground:
                sceneName = "JJ_Playground";
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

    public SceneType GetCurrentSceneType()
    {
        return SceneNameToSceneType(SceneManager.GetActiveScene().name);
    }

    public SceneType SceneNameToSceneType(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                return SceneType.MainMenu;
            case "Bedroom":
                return SceneType.Bedroom;
            case "Level1":
                return SceneType.Level1;
            case "Level2":
                return SceneType.Level2;
            case "JJ_Playground":
                return SceneType.JJPlayground;
            default:
                return SceneType.None;
        }
    }

}
