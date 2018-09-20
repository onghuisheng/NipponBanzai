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

        f_cooldown = 0;
        f_timer = 0;
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
        f_timer += Time.deltaTime;

        if (f_timer > 10)
            go_caster.An_animator.SetBool("IsSummoning", false);
    }

    public override void EndSkill()
    {
        f_cooldown = 10;
        f_timer = 0;
    }
}
