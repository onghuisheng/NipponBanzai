using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChase : AIBase
{
    private Vector3
        v3_target_position;

    private System.Type
        type_target;

    private float
        f_aggro_range, f_aggro_break_range;

    private bool
        b_is_chasing;

    private EntityPlayer ep_player;

    private NavMeshAgent nma_agent;

    public AIChase(int _priority, EntityLivingBase _entity, System.Type _type, float _aggroRange, float _aggroBreakRange)
    {
        f_aggro_range = _aggroRange;
        f_aggro_break_range = _aggroBreakRange;
        i_priority = _priority;
        ent_main = _entity;
        type_target = _type;
        s_ID = "Movement";
        s_display_name = "Chase Target - " + type_target;
        b_is_interruptable = true;
        b_is_chasing = false;
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

        if (nma_agent != null && !nma_agent.enabled)
            return false;

        if (ep_player == null)
        {
            ep_player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        }

        // Dont chase if we're attacking
        var animatorState = ent_main.An_animator.GetCurrentAnimatorStateInfo(0);
        if (ent_main.Stc_Status.isPanicking || ent_main.Stc_Status.isStunned || ent_main.B_isAttacking || animatorState.IsTag("Attack") || animatorState.IsName("Poison Attack"))
            return false;

        // Chase player if within aggro range and if theres nothing blocking it
        if ((ep_player.transform.position - ent_main.transform.position).magnitude <= f_aggro_range)
        {
            RaycastHit hitInfo;

            int ignoreEnemiesMask = ~(1 << LayerMask.NameToLayer("Enemy"));

            // Cast a ray towards the player, ignoring all objects in the Enemies layer
            Vector3 enemyCenter = ent_main.GetComponent<Collider>().bounds.center;
            Vector3 playerCenter = ep_player.GetComponent<Collider>().bounds.center;
            if (!b_is_chasing && Physics.Raycast(enemyCenter, playerCenter - enemyCenter, out hitInfo, f_aggro_range, ignoreEnemiesMask))
            {
                if (hitInfo.collider.tag != "Player")
                {
                    return false;
                }
                else
                {
                    b_is_chasing = true;
                    return true;
                }
            }
            else
            {
                b_is_chasing = true;
                return true;
            }
        }

        // Break if player got out of aggro range
        if ((ep_player.transform.position - ent_main.transform.position).magnitude >= f_aggro_break_range)
        {
            return false;
        }

        if (ep_player.IsDead())
        {
            return false;
        }

        return false;
    }

    public override bool RunAI()
    {
        if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "MoveSpeed", AnimatorControllerParameterType.Float))
            ent_main.An_animator.SetFloat("MoveSpeed", ent_main.St_stats.F_speed);
        if (nma_agent != null && nma_agent.enabled)
            nma_agent.destination = ep_player.transform.position;

        if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "Moving", AnimatorControllerParameterType.Bool))
            ent_main.An_animator.SetBool("Moving", true);

        return true;
    }

    public override bool EndAI()
    {
        if(nma_agent != null && nma_agent.enabled)
            nma_agent.SetDestination(nma_agent.transform.position);
        if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "Moving", AnimatorControllerParameterType.Bool))
            ent_main.An_animator.SetBool("Moving", false);

        return true;
    }
}
