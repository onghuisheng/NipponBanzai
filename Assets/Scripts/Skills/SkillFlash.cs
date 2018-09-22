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

        if (f_timer > 1.5f)
        {
            go_caster.An_animator.SetBool("IsSummoning", false);

            StatusBase _effect = new StatusPanic(30);

            foreach (GameObject go in ObjectPool.GetInstance().GetAllActiveInSurrounding(go_caster.GetPosition(), 15 * (((f_mana_amount_used / f_mana_amount) * 100) / 100), typeof(EntityEnemy)))
            {
                go.GetComponent<EntityEnemy>().Stc_Status.ApplyStatus(_effect);
            }
        }
    }

    public override void EndSkill()
    {
        f_cooldown = 1;
        f_timer = 0;
    }
}
