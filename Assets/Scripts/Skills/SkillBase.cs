using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase
{
    public enum TYPE
    {
        OFFENCE,
        DEFENCE
    }

    protected string
        s_name,
        s_description;

    protected float
        f_mana_amount,
        f_mana_amount_used,
        f_timer,
        f_cooldown,
        f_maxcooldown;

    protected int
        i_id;

    protected EntityLivingBase
        go_caster;

    protected TYPE
        type_style;

    protected System.Action act_OnSkillPressed, act_OnSkillReleased, act_OnSkillBegin, act_OnSkillEnd;

    public int I_id {
        get {
            return i_id;
        }
    }

    public float F_MaxCooldown {
        get {
            return f_maxcooldown;
        }
    }

    public string S_Name {
        get {
            return s_name;
        }
    }

    public void RegisterOnSkillPressed(System.Action action) { act_OnSkillPressed += action; }
    public void OnSkillPressed() { if (act_OnSkillPressed != null) act_OnSkillPressed.Invoke(); }
    public void RegisterOnSkillReleased(System.Action action) { act_OnSkillReleased += action; }
    public void OnSkillReleased() { if (act_OnSkillReleased != null) act_OnSkillReleased.Invoke(); }
    public void RegisterOnSkillBegin(System.Action action) { act_OnSkillBegin += action; }
    public void OnSkillBegin() { if (act_OnSkillBegin != null) act_OnSkillBegin.Invoke(); }
    public void RegisterOnSkillEnd(System.Action action) { act_OnSkillEnd += action; }
    public void OnSkillEnd() { if (act_OnSkillEnd != null) act_OnSkillEnd.Invoke(); }

    public abstract void SetUpSkill();

    public abstract void StartSkill(EntityLivingBase _caster, float _manaused);

    public abstract void RunSkill();        //Run intented effects (flashing, etc)
    public virtual void UpdateSkill()     //Update skills variables (timer, etc)
    {
        if (f_cooldown > 0)
            f_cooldown -= Time.deltaTime;
    }

    public abstract void EndSkill();        //Reset Variable

    public bool IsUnderCooldown()
    {
        return f_cooldown <= 0;
    }

    public float GetManaAmount()
    {
        return f_mana_amount;
    }
}
