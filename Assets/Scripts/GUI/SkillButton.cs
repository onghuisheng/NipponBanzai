using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillButton : MonoBehaviour
{

    EntityPlayer m_Player;

    SkillBase m_AssignedSkill;

    Image m_SkillIcon;
    Image m_SkillCooldown;
    Image m_SkillPressed;
    Image m_SkillFrame;

    private void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        m_SkillIcon = transform.Find("Icon").GetComponent<Image>();
        m_SkillCooldown = transform.Find("Cooldown").GetComponent<Image>();
        m_SkillPressed = transform.Find("Pressed").GetComponent<Image>();
        m_SkillFrame = transform.Find("Frame").GetComponent<Image>();
    }

    public void RegisterSkill(SkillBase skill)
    {
        m_AssignedSkill = skill;
    }

    public void OnSkillPressed()
    {
        m_SkillPressed.gameObject.SetActive(true);
    }

    public void OnSkillReleased()
    {
        m_SkillPressed.gameObject.SetActive(false);
    }

    public void OnSkillBegin()
    {
        m_SkillCooldown.gameObject.SetActive(true);
        m_SkillCooldown.fillAmount = 1;

        Color color = m_SkillCooldown.color;
        color.a = 0;
        m_SkillCooldown.color = color;

        m_SkillCooldown.DOFade(0.8f, 0.25f).OnComplete(() =>
        {
            m_SkillCooldown.DOFillAmount(0, m_AssignedSkill.F_MaxCooldown).SetEase(Ease.Linear);
        });

        Debug.Log("Pressed: " + m_AssignedSkill.S_Name);
    }

}
