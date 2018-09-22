using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSwordSummoning : SkillBase
{
    public override void SetUpSkill()
    {
        s_name = "Swords of Revealing Light";
        s_description = "Summons a number of swords based on your mana";

        type_style = TYPE.OFFENCE;
        f_mana_amount = 50;
        i_id = 1;

        f_cooldown = 0;
        f_timer = 0;
    }

    public override void StartSkill(EntityLivingBase _caster, float _manaused)
    {
        go_caster = _caster;
        f_mana_amount_used = _manaused;
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
        f_cooldown = 1;
        f_timer = 0;
    }
}
