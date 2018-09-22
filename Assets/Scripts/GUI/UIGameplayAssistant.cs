using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIGameplayAssistant : MonoBehaviour {

    public Image m_FadeTexture;

    // Use this for initialization
    void Start () {
        m_FadeTexture.DOFade(0, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
