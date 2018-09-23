using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIGameplayAssistant : MonoBehaviour
{

    public Image m_FadeTexture;
    public Image m_PlayerHealthTexture;
    public Image m_PlayerManaTexture;

    public GameObject m_Minimap;
    public GameObject m_MinimapCamera;

    private EntityPlayer m_Player;

    float m_PrevPlayerHP;
    float m_PrevPlayerMP;

    // Use this for initialization
    void Start()
    {
        if (!m_FadeTexture.gameObject.activeInHierarchy && m_FadeTexture.transform.parent != null)
            m_FadeTexture.transform.parent.gameObject.SetActive(true);

        m_FadeTexture.DOFade(0, 1.5f);

        m_Player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        m_PrevPlayerHP = m_Player.St_stats.F_health;
        m_PrevPlayerMP = m_Player.St_stats.F_mana;
    }

    private void LateUpdate()
    {
        if (m_Player.St_stats.F_health != m_PrevPlayerHP)
        {
            float healthPercent = (m_Player.St_stats.F_health / m_Player.St_stats.F_max_health);
            m_PlayerHealthTexture.DOFillAmount(healthPercent, 0.2f).SetEase(Ease.OutExpo); // Subtle tweening of health bar when taking damage
            m_PrevPlayerHP = m_Player.St_stats.F_health;
        }

        if (m_Player.St_stats.F_mana != m_PrevPlayerMP)
        {
            float manaPercent = (m_Player.St_stats.F_mana / m_Player.St_stats.F_max_mana);
            m_PlayerManaTexture.DOFillAmount(manaPercent, 0.2f).SetEase(Ease.OutExpo);
            m_PrevPlayerMP = m_Player.St_stats.F_mana;
        }

        // Minimap Camera
        Vector3 newCamPos = m_MinimapCamera.transform.position;
        newCamPos.x = m_Player.transform.position.x;
        newCamPos.z = m_Player.transform.position.z;
        m_MinimapCamera.transform.position = newCamPos; // follow player's x and z position

        // Rotate the minimap depending on the current camera angle;
        Vector3 dir = m_Player.transform.position - Camera.main.transform.position;
        float cameraAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        m_Minimap.transform.rotation = Quaternion.Euler(0, 0, cameraAngle);

    }

}
