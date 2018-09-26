using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBeam : SkillBase
{
    private Laser
        ent_laser;

    public override void SetUpSkill()
    {
        s_name = "Light Beam";
        s_description = "Shoots a concentrated beam of light in front of caster";

        type_style = TYPE.OFFENCE;
        f_mana_amount = 50;
        i_id = 2;

        f_cooldown = 0;
        f_maxcooldown = 10;
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
            if (ent_laser == null)
            {
                ent_laser = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.LASER_PROJECTILE).GetComponent<Laser>();
                ent_laser.SetUpProjectile(100000, go_caster.St_stats.F_damage, 10.0f, 100, new Vector3(go_caster.GetPosition().x, go_caster.GetPosition().y + 2, go_caster.GetPosition().z), new Vector3(go_caster.transform.forward.x, go_caster.GetPosition().y, go_caster.transform.forward.z), Vector3.one * (f_mana_amount_used * 0.1f), go_caster.gameObject);         
            }
            else
            {
                ent_laser.NewEndPoint(go_caster.transform.forward, 100);
                
            }
        }

        if (f_timer > 20)
        {
            ent_laser.gameObject.SetActive(false);
            ent_laser = null;
            go_caster.An_animator.SetBool("IsSummoning", false);
        }

    }

    public override void EndSkill()
    {
        f_cooldown = f_maxcooldown;
        f_timer = 0;

        if (ent_laser != null)
        {
            ent_laser.gameObject.SetActive(false);
            ent_laser = null;
        }
        go_caster.An_animator.SetBool("IsSummoning", false);

        OnSkillEnd();
    }
}
