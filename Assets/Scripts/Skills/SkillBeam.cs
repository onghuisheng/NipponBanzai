using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBeam : SkillBase
{


    public override void SetUpSkill()
    {
        s_name = "Light Beam";
        s_description = "Shoots a concentrated beam of light in front of caster";

        type_style = TYPE.OFFENCE;
        f_mana_amount = 50;
        i_id = 2;
    }

    public override void StartSkill(EntityLivingBase _caster)
    {
        go_caster = _caster;
    }

    public override void UpdateSkill()
    {
        base.UpdateSkill();
    }

    public override void RunSkill()
    {
    }

    public override void EndSkill()
    {
    }
}
