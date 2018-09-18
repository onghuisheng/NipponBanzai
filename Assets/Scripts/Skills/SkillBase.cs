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

    private string
        s_name,
        s_description, 
        s_caster_tag;

    private float
        f_mana_amount;

    private TYPE
        type_style;   

    public virtual void SetUpSkill(string _caster_tag = "")
    {
        s_caster_tag = _caster_tag;
    }

    public abstract void RunSkill();

    public abstract void EndSkill();
}
