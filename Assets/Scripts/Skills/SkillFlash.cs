using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFlash : SkillBase
{
    public override void SetUpSkill()
    {
        s_name = "Brilliant Flash";
        s_description = "Applies panic to surrounding enemies";

        type_style = TYPE.DEFENCE;
        f_mana_amount = 10;
        i_id = 0;
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
