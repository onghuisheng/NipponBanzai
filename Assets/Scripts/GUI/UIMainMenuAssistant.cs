using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIMainMenuAssistant : MonoBehaviour
{

    public GameObject m_MainMenuElements;
    public Image m_FadeTexture;

    private RectTransform m_ElementsTransform;
    private Canvas m_ElementCanvas;

    private void Start()
    {
        m_ElementsTransform = m_MainMenuElements.GetComponent<RectTransform>();
        m_ElementCanvas = m_ElementsTransform.transform.parent.GetComponent<Canvas>();

        if (m_ElementCanvas == null)
            Debug.LogError("Can't find canvas, this script will not work!");
    }

    public void MainMenuToOptions()
    {
        m_ElementsTransform.DOAnchorPosY(m_ElementCanvas.GetComponent<RectTransform>().rect.height, 1).SetEase(Ease.OutBack);
    }

    public void OptionsToMainMenu()
    {
        m_ElementsTransform.DOAnchorPosY(0, 1).SetEase(Ease.InOutBack);
    }

    public void StartGame()
    {
        m_FadeTexture.raycastTarget = true; // Prevent double clicking
        m_FadeTexture.DOFade(1, 1.5f).OnComplete(() =>
        {
            // Check if contains save data
            SceneHandler.GetInstance().ChangeSceneAsync(SceneHandler.SceneType.Level1, null);
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
