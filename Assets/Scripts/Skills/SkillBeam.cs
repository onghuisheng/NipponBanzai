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

        if (f_timer > 1.5f)
            ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.LASER_PROJECTILE).GetComponent<Laser>()
                .SetUpProjectile(0.5f, go_caster.St_stats.F_damage, 1.0f, 10.0f, new Vector3(go_caster.GetPosition().x, go_caster.GetPosition().y + 2, go_caster.GetPosition().z), new Vector3(go_caster.transform.forward.x * 10, go_caster.GetPosition().y + 12, go_caster.transform.forward.z * 10), Vector3.one * 2, go_caster.gameObject);

        if(f_timer > 100)
            go_caster.An_animator.SetBool("IsSummoning", false);

    }

    public override void EndSkill()
    {
        f_cooldown = 10;
        f_timer = 0;
    }
}
