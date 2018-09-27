using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIBedroomAssistant : MonoBehaviour
{

    public Image m_FadeTexture;
    public Text m_LblCurrency;
    public Text m_LblRedPotion;
    public Text m_LblBluePotion;

    public enum TransitType
    {
        In,
        Out
    }

    private void Start()
    {
        Transition(TransitType.In, 0.5f);

        var playerInventory = SaveData.LoadInventory();
    }

    public void Transition(TransitType transitType, float duration, System.Action onComplete = null)
    {
        if (transitType == TransitType.In)
        {
            var newColor = m_FadeTexture.color;
            newColor.a = 1;
            m_FadeTexture.color = newColor;
            m_FadeTexture.DOFade(0, duration).OnComplete(() => { if (onComplete != null) onComplete(); });
        }
        else
        {
            var newColor = m_FadeTexture.color;
            newColor.a = 0;
            m_FadeTexture.color = newColor;
            m_FadeTexture.DOFade(1, duration).OnComplete(() => { if (onComplete != null) onComplete(); });
        }
    }

    public void StartGame(string sceneName)
    {
        m_FadeTexture.raycastTarget = true; // Prevent double clicking
        m_FadeTexture.DOFade(1, 1.5f).OnComplete(() =>
        {
            SceneHandler.GetInstance().ChangeSceneAsync(SceneHandler.GetInstance().SceneNameToSceneType(sceneName), null);
        });
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
