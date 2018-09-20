using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPanic : AIBase
{
    private System.Type
        type_target;

    private EntityPlayer ep_player;

    private NavMeshAgent nma_agent;


    public AIPanic(int _priority, EntityLivingBase _entity, System.Type _type)
    {
        i_priority = _priority;
        ent_main = _entity;
        type_target = _type;
        s_ID = "Combat";
        s_display_name = "Panic";
        b_is_interruptable = false;
    }

    public override bool StartAI()
    {
        return true;
    }

    public override bool ShouldContinueAI()
    {

        if (ep_player == null)
        {
            ep_player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        }

        if (ent_main.Stc_Status.isPanicking)
        {
            return true;
        }

        return false;
    }

    public override bool RunAI()
    {
        if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "MoveSpeed", AnimatorControllerParameterType.Float))
            ent_main.An_animator.SetFloat("MoveSpeed", ent_main.St_stats.F_speed);

        nma_agent.destination = ep_player.transform.position;

        if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "Moving", AnimatorControllerParameterType.Bool))
            ent_main.An_animator.SetBool("Moving", true);

        return true;
    }

    public override bool EndAI()
    {
        nma_agent.SetDestination(nma_agent.transform.position);
        if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "Moving", AnimatorControllerParameterType.Bool))
            ent_main.An_animator.SetBool("Moving", false);

        return true;
    }
}
