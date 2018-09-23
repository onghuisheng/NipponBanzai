using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIGameplayAssistant : MonoBehaviour {

    public Image m_FadeTexture;

    // Use this for initialization
    void Start () {
        if (!m_FadeTexture.gameObject.activeInHierarchy && m_FadeTexture.transform.parent != null)
            m_FadeTexture.transform.parent.gameObject.SetActive(true);

        m_FadeTexture.DOFade(0, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
