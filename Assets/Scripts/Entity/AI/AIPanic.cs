using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AIPanic : AIBase
{
    private System.Type
        type_target;

    private EntityPlayer ep_player;

    private NavMeshAgent nma_agent;

    private float f_next_panic_time, f_panic_interval, f_prev_angular_speed;

    private Vector3 v3_current_destination;

    public AIPanic(int _priority, EntityLivingBase _entity, System.Type _type)
    {
        i_priority = _priority;
        ent_main = _entity;
        type_target = _type;
        s_ID = "Combat";
        s_display_name = "Panic";
        f_panic_interval = 1;
        b_is_interruptable = false;
    }

    public override bool StartAI()
    {
        nma_agent = ent_main.GetComponent<NavMeshAgent>();
        nma_agent.speed = ent_main.GetStats().F_speed * 3;

        f_prev_angular_speed = nma_agent.angularSpeed;
        nma_agent.angularSpeed = 720;

        ent_main.An_animator.SetBool("Panicking", true);

        ent_main.transform.DOKill();            

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

        if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "Moving", AnimatorControllerParameterType.Bool))
            ent_main.An_animator.SetBool("Moving", true);

        if (Time.time < f_next_panic_time)
            return false;
        else
        {
            Vector3 enemyCenter = ent_main.GetComponent<Collider>().bounds.center;
            float angle = UnityEngine.Random.Range(0, 360);
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 5;
            Vector3 target = enemyCenter + dir;

            v3_current_destination = target;
            nma_agent.destination = target;

            f_next_panic_time = Time.time + f_panic_interval;

        }

        return true;
    }

    public override bool EndAI()
    {
        // nma_agent.SetDestination(nma_agent.transform.position);
        if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "Moving", AnimatorControllerParameterType.Bool))
            ent_main.An_animator.SetBool("Moving", false);

        nma_agent.destination = ent_main.transform.position;
        nma_agent.speed = ent_main.GetStats().F_speed;
        nma_agent.angularSpeed = f_prev_angular_speed;

        ent_main.An_animator.SetBool("Panicking", false);

        return true;
    }
}
