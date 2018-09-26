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
    public Slider m_SFXSlider;
    public Slider m_MusicSlider;
    public List<Image> m_GraphicsLevel; // order matters - index 0 should be low graphics


    private RectTransform m_ElementsTransform;
    private Canvas m_ElementCanvas;

    private void Start()
    {
        m_ElementsTransform = m_MainMenuElements.GetComponent<RectTransform>();
        m_ElementCanvas = m_ElementsTransform.transform.parent.GetComponent<Canvas>();

        if (m_ElementCanvas == null)
            Debug.LogError("Can't find canvas, this script will not work!");

        SaveDataSettings settings = SaveData.LoadSettings();
        m_SFXSlider.value = settings.m_SFXVolume;
        m_MusicSlider.value = settings.m_MusicVolume;
        m_GraphicsLevel[settings.m_GraphicsLevel].gameObject.SetActive(true);
    }

    public void MainMenuToOptions()
    {
        m_ElementsTransform.DOAnchorPosY(m_ElementCanvas.GetComponent<RectTransform>().rect.height, 1).SetEase(Ease.OutBack);
    }

    public void OptionsToMainMenu()
    {
        m_ElementsTransform.DOAnchorPosY(0, 1).SetEase(Ease.InOutBack);
    }

    public void SaveSettings()
    {
        int graphicsLevel = 0;

        for (int i = 0; i < m_GraphicsLevel.Count; ++i)
        {
            if (m_GraphicsLevel[i].gameObject.activeInHierarchy)
            {
                graphicsLevel = i;
                break;
            }
        }

        SaveData.SaveSettings(m_SFXSlider.value, m_MusicSlider.value, graphicsLevel);
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
