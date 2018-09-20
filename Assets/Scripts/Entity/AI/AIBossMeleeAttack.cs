using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AIBossMeleeAttack : AIBase
{
    private float
        f_attack_range,
        f_maxStateTimer,
        f_cooldown,
        f_stateCooldownTimer;

    private int
        i_randomChance;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private EntityPlayer ep_player;

    private NavMeshAgent nma_agent;

    private EntityBoss
        script_boss;

    private Tweener tween_look_at_player;

    public AIBossMeleeAttack(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _stateTime, float _cooldown)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Boss Melee Attack";
        b_is_interruptable = false;
        f_attack_range = _range;
        type_target = _type;
        f_cooldown = _cooldown;
        f_maxStateTimer = _stateTime;

        //f_stateCooldownTimer = f_cooldown;

        b_has_attacked = false;
        i_randomChance = 5;

        script_boss = ent_main.GetComponent<EntityBoss>();
    }

    public override bool StartAI()
    {
        Reset();
        nma_agent = ent_main.GetComponent<NavMeshAgent>();
        nma_agent.speed = ent_main.GetStats().F_speed * 0.5f;

        script_boss.NextAttackState((EntityBoss.AttackState)Random.Range((int)EntityBoss.AttackState.H_MEELE, (int)EntityBoss.AttackState.V_MEELE + 1));
        script_boss.NextChargeState(EntityBoss.ChargeState.STAGE_1);

        return true;
    }

    public override bool ShouldContinueAI()
    {
        if (f_stateCooldownTimer < f_cooldown)
        {
            f_stateCooldownTimer += Time.deltaTime;
            return false;
        }

        //// USING RNG TO PROCEED FOR ATTACK
        //if (Random.Range(0,10) > i_randomChance)
        //{
        //    return false;
        //}

        AnimatorStateInfo animatorState = ent_main.An_animator.GetCurrentAnimatorStateInfo(0);

        if (animatorState.IsTag("Attack"))
        {
            //ent_main.B_isAttacking = true;
            return true;
        }

        if (ep_player == null)
        {
            ep_player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        }

        //if ((ep_player.transform.position - ent_main.transform.position).magnitude <= f_attack_range)
        //{
        //    //ent_main.B_isAttacking = true;
        //    return true;
        //}

        return true;
    }


    public override bool RunAI()
    {
        AnimatorStateInfo animatorState = ent_main.An_animator.GetCurrentAnimatorStateInfo(0);

        //if (animatorState.IsTag("Attack"))
        //{
        //    b_has_attacked = false;
        //}

        //if (!animatorState.IsTag("Attack") && !b_has_attacked)
        if (!b_has_attacked)
        {
            // Look at the player and then attack, tweak the duration to adjust the rate of turning
            tween_look_at_player = ent_main.transform.DOLookAt(ep_player.transform.position, 0.1f, AxisConstraint.Y).OnComplete(() =>
            {
                //if (ent_main.An_animator.HasParameterOfType("AttackType", AnimatorControllerParameterType.Int))
                //    ent_main.An_animator.SetInteger("AttackType", Random.Range(1, 4));

                ent_main.An_animator.SetTrigger("Attack");
            });
            b_has_attacked = true;
        }

        return true;
    }


    public override bool EndAI()
    {
        //ent_main.B_isAttacking = false;
        Debug.Log("end");
        Reset();
        f_stateCooldownTimer = 0;
        script_boss.NextAttackState(EntityBoss.AttackState.NONE);
        script_boss.NextChargeState(EntityBoss.ChargeState.NONE);
        return true;
    }

    void Reset()
    {
        b_has_attacked = false;
        ent_target = null;
    }


}
