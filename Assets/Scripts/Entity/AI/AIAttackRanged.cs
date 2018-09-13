using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class AIAttackRanged : AIBase
{
    private float
        f_attack_range, f_turn_Rate;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private EntityPlayer ep_player;

    private NavMeshAgent nma_agent;

    private Tweener tween_look_at_player;


    public AIAttackRanged(int _priority, EntityLivingBase _entity, System.Type _type, float _attackrange)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Ranged Attack";
        b_is_interruptable = false;
        f_attack_range = _attackrange;
        type_target = _type;
        f_turn_Rate = 0.5f;

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

        if (ep_player == null)
        {
            ep_player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        }

        int ignoreEnemiesMask = ~(1 << LayerMask.NameToLayer("Enemies"));

        RaycastHit hitInfo;

        // Cast a ray towards the player, ignoring all objects in the Enemies layer
        Vector3 enemyCenter = ent_main.GetComponent<Collider>().bounds.center;
        Vector3 playerCenter = ep_player.GetComponent<Collider>().bounds.center;
        if (Physics.Raycast(enemyCenter, playerCenter - enemyCenter, out hitInfo, f_attack_range, ignoreEnemiesMask))
        {
            if (hitInfo.collider.tag != "Player")
            {
                return false;
            }
        }

        AnimatorStateInfo animatorState = ent_main.An_animator.GetCurrentAnimatorStateInfo(0);
            
        if (animatorState.IsTag("Attack"))
        {
            ent_main.B_isAttacking = true;
            return true;
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
            ent_main.transform.DOLookAt(ep_player.transform.position, f_turn_Rate, AxisConstraint.Y);
        }

        if (!animatorState.IsTag("Attack") && !b_has_attacked)
        {
            // Look at the player and then attack, tweak the duration to adjust the rate of turning
            tween_look_at_player = ent_main.transform.DOLookAt(ep_player.transform.position, f_turn_Rate, AxisConstraint.Y).OnComplete(() =>
            {
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

        if (tween_look_at_player != null)
            tween_look_at_player.Kill();

        return true;
    }
}