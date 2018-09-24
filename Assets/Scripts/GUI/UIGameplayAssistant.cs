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

    public Text m_HealthPotionAmount;
    public Text m_ManaPotionAmount;

    public GameObject m_SkillButtonPrefab;
    public GameObject m_SkillHolder;
    public List<Sprite> m_SkillIcons;

    public GameObject m_Minimap;
    public GameObject m_MinimapCamera;

    private EntityPlayer m_Player;

    float m_PrevPlayerHP;
    float m_PrevPlayerMP;

    bool m_IsInitialized = false;
    float m_SkillsIconGap; // Length of the gap between the skill icons

    // Use this for initialization
    void Start()
    {
        if (!m_FadeTexture.gameObject.activeInHierarchy && m_FadeTexture.transform.parent != null)
            m_FadeTexture.transform.parent.gameObject.SetActive(true);

        m_FadeTexture.DOFade(0, 1.5f);
    }

    void PopulateSkillIcons()
    {
        foreach (SkillBase skill in m_Player.GetInventory().GetAllSkills())
        {
            // skill.S_Name
            GameObject go = Instantiate(m_SkillButtonPrefab, m_SkillHolder.transform);
            go.transform.Find("Icon").GetComponent<Image>().sprite = FindSpriteWithName(skill.S_Name);
            go.name = skill.S_Name;
        }

        var hLayout = m_SkillHolder.GetComponent<HorizontalLayoutGroup>();
        m_SkillsIconGap = hLayout.padding.left + hLayout.spacing + m_SkillButtonPrefab.GetComponent<RectTransform>().rect.width / 2;

    }

    Sprite FindSpriteWithName(string name)
    {
        foreach (Sprite sprite in m_SkillIcons)
        {
            if (sprite.name == name)
                return sprite;
        }
        return null;
    }

    private void LateUpdate()
    {
        if (!m_IsInitialized)
        {
            m_Player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
            m_PrevPlayerHP = m_Player.St_stats.F_health;
            m_PrevPlayerMP = m_Player.St_stats.F_mana;

            PopulateSkillIcons();
            m_IsInitialized = true;
        }

        // Subtle tweening of health/mana bar when its values are changed
        if (m_Player.St_stats.F_health != m_PrevPlayerHP)
        {
            float healthPercent = (m_Player.St_stats.F_health / m_Player.St_stats.F_max_health);
            m_PlayerHealthTexture.DOFillAmount(healthPercent, 0.2f).SetEase(Ease.OutExpo);
            m_PrevPlayerHP = m_Player.St_stats.F_health;
        }

        if (m_Player.St_stats.F_mana != m_PrevPlayerMP)
        {
            float manaPercent = (m_Player.St_stats.F_mana / m_Player.St_stats.F_max_mana);
            m_PlayerManaTexture.DOFillAmount(manaPercent, 0.2f).SetEase(Ease.OutExpo);
            m_PrevPlayerMP = m_Player.St_stats.F_mana;
        }

        // Assign potion amount text
        var playerInventory = m_Player.GetInventory().GetInventoryContainer();
        int hpPots = 0, manaPots = 0;

        foreach (var item in playerInventory)
        {
            if (item.Key == Item.ITEM_TYPE.HEALTH_POTION)
                hpPots = item.Value;
            if (item.Key == Item.ITEM_TYPE.MANA_POTION)
                manaPots = item.Value;
        }

        m_HealthPotionAmount.text = hpPots.ToString();
        m_ManaPotionAmount.text = manaPots.ToString();

        // Minimap Camera
        Vector3 newCamPos = m_MinimapCamera.transform.position;
        newCamPos.x = m_Player.transform.position.x;
        newCamPos.z = m_Player.transform.position.z;
        m_MinimapCamera.transform.position = newCamPos; // follow player's x and z position

        // Rotate the minimap depending on the current camera angle;        
        m_Minimap.transform.rotation = Quaternion.Euler(0, 0, TPCamera.f_CurrentAngle);

        // Skill icons movement
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            RectTransform holderTransform = m_SkillHolder.GetComponent<RectTransform>();
            holderTransform.DOAnchorPosX(m_Player.GetInventory().GetCurrSkillIndex() * -m_SkillsIconGap, 0.5f).SetEase(Ease.OutBack);
        }

    }

}
