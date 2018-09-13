using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class AIAttackMelee : AIBase
{
    private float
        f_attack_range;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private EntityPlayer ep_player;

    private NavMeshAgent nma_agent;

    private Tweener tween_look_at_player;

    // Use navmesh agent stopping distance for attack range if possible
    public AIAttackMelee(int _priority, EntityLivingBase _entity, System.Type _type, float _attackrange)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Melee Attack";
        b_is_interruptable = false;
        f_attack_range = _attackrange;
        type_target = _type;

        b_has_attacked = false;

    }

    public override bool StartAI()
    {
        ent_target = null;
        nma_agent = ent_main.GetComponent<NavMeshAgent>();
        nma_agent.speed = ent_main.GetStats().F_speed;
        return true;
    }

    public override bool ShouldContinueAI()
    {
        AnimatorStateInfo animatorState = ent_main.An_animator.GetCurrentAnimatorStateInfo(0);

        if (animatorState.IsTag("Attack"))
        {
            ent_main.B_isAttacking = true;
            return true;
        }

        if (ep_player == null)
        {
            ep_player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        }

        if ((ep_player.transform.position - ent_main.transform.position).magnitude <= f_attack_range)
        {
            ent_main.B_isAttacking = true;
            return true;
        }

        return false;
    }

    public override bool RunAI()
    {
        AnimatorStateInfo animatorState = ent_main.An_animator.GetCurrentAnimatorStateInfo(0);

        if (animatorState.IsTag("Attack"))
        {
            b_has_attacked = false;
        }

        if (!animatorState.IsTag("Attack") && !b_has_attacked)
        {
            // Look at the player and then attack, tweak the duration to adjust the rate of turning
            tween_look_at_player = ent_main.transform.DOLookAt(ep_player.transform.position, 0.1f, AxisConstraint.Y).OnComplete(() =>
            {
                if (ent_main.An_animator.HasParameterOfType("AttackType", AnimatorControllerParameterType.Int))
                    ent_main.An_animator.SetInteger("AttackType", Random.Range(1, 4));

                ent_main.An_animator.SetTrigger("Attack");
            });
            b_has_attacked = true;
        }

        return true;
    }

    public override bool EndAI()
    {
        ent_main.B_isAttacking = false;
        b_has_attacked = false;

        if (ent_main.An_animator.HasParameterOfType("AttackType", AnimatorControllerParameterType.Int))
            ent_main.An_animator.SetInteger("AttackType", 0);

        if (tween_look_at_player != null)
            tween_look_at_player.Kill();

        return true;
    }
}