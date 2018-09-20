using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossSpinAttack : AIBase {

    private float
        f_range,
        f_stateTimer,
        f_maxStateTimer,
        f_cooldown,
        f_spinRange,
        f_stateCooldownTimer;

    private System.Type
        type_target;

    private bool
        b_has_attacked;


    private EntityBoss
        script_boss;


    public AIBossSpinAttack(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _stateTime, float _cooldown)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Boss SPIN Attack";
        b_is_interruptable = false;
        f_range = _range;
        type_target = _type;
        f_cooldown = _cooldown;
        f_maxStateTimer = _stateTime;

        //f_stateCooldownTimer = f_cooldown;
        f_spinRange = 0;

        b_has_attacked = false;

        script_boss = ent_main.GetComponent<EntityBoss>();
    }

    public override bool StartAI()
    {
        Debug.Log("SPIN");
        Reset();
        script_boss.NextAttackState(EntityBoss.AttackState.SPINATTACK);
        script_boss.NextChargeState(EntityBoss.ChargeState.STAGE_1);
        return true;
    }

    public override bool EndAI()
    {
        //ent_main.B_isAttacking = false;
        b_has_attacked = false;
        f_stateCooldownTimer = 0;

        Reset();
        script_boss.NextAttackState(EntityBoss.AttackState.NONE);
        script_boss.NextChargeState(EntityBoss.ChargeState.NONE);

        //ent_main.GetAnimator().SetBool("PunchTrigger", false);
        //ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;

        return true;
    }


    public override bool ShouldContinueAI()
    {
        if (f_stateCooldownTimer < f_cooldown)
        {
            f_stateCooldownTimer += Time.deltaTime;
            return false;
        }

        AnimatorStateInfo animatorState = ent_main.An_animator.GetCurrentAnimatorStateInfo(0);

        // Breaking point
        if (f_stateTimer > f_maxStateTimer)
        {
            Debug.Log("ASHFHAKFESLKFELKASJFLJFJFLJAEFK");
            b_has_attacked = false;
            return false;
        }

        f_stateTimer += Time.deltaTime;

        if (ent_target == null)
        {
            foreach (GameObject l_go in ObjectPool.GetInstance().GetActiveEntityObjects())
            {
                if (type_target.Equals(l_go.GetComponent<EntityLivingBase>().GetType()))
                {
                    if (!l_go.GetComponent<EntityLivingBase>().IsDead())
                    {
                        if (ent_target == null)
                        {
                            if (Vector3.Distance(ent_main.GetPosition(), l_go.transform.position) < f_range)
                            {
                                ent_target = l_go.GetComponent<EntityLivingBase>();
                            }
                        }
                        else
                        {
                            if (Vector3.Distance(ent_main.GetPosition(), ent_target.transform.position) > Vector3.Distance(ent_main.GetPosition(), l_go.transform.position))
                            {
                                ent_target = l_go.GetComponent<EntityLivingBase>();
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        if (ent_target != null && ent_target.IsDead())
        {
            ent_target = null;
        }

        if (ent_target == null)
        {
            return false;
        }

        //ent_main.GetAnimator().SetBool("PunchTrigger", true);
        //ent_main.GetAnimator().speed = ent_main.F_attack_speed;

        //ent_main.B_isAttacking = true;

        return true;
    }


    public override bool RunAI()
    {
        Debug.Log("RUNNING");
        if (ent_target != null)
        {
            b_has_attacked = true;

            if (script_boss.Enum_currentChargeState == EntityBoss.ChargeState.STAGE_2)
            {
                f_spinRange += Time.deltaTime;
                float f_spinSize = Mathf.Lerp(0f, 12f, f_spinRange);
                ent_main.OnAOEAttack(f_spinSize);

            }

            //ent_main.RotateTowardsTargetPosition(ent_target.GetPosition());
        }

        return true;
    }

    void Reset()
    {
        f_stateTimer = 0;
        f_spinRange = 0;
        ent_target = null;
    }
}
