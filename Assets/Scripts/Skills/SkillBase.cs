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
        f_cooldown;

    protected int
        i_id;

    protected EntityLivingBase
        go_caster;

    protected TYPE
        type_style;

    public int I_id
    {
        get
        {
            return i_id;
        }
    }

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
