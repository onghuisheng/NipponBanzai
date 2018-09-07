using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        return true;
    }

    public override bool ShouldContinueAI()
    {
        //if (b_has_attacked && ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > ent_main.F_totalAnimationLength)
        //{
        //    return false;
        //}

        if (ep_player == null)
        {
            ep_player = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        }

        if ((ep_player.transform.position - ent_main.transform.position).magnitude <= f_attack_range)
        {
            ent_main.B_isAttacking = true;
            return true;
        }

        //ent_main.GetAnimator().SetBool("PunchTrigger", true);
        //ent_main.GetAnimator().speed = ent_main.F_attack_speed;

        return false;
    }

    public override bool RunAI()
    {

        //if (ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= (ent_main.F_totalAnimationLength * 0.9f) && ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && !b_has_attacked)
        //{
        //    b_has_attacked = true;
        //    ent_main.OnAttack();
        //} 

        //ent_main.RotateTowardsTargetPosition(ent_target.GetPosition());



        Debug.Log("Enemy melee");

        return true;
    }

    public override bool EndAI()
    {
        ent_main.B_isAttacking = false;
        b_has_attacked = false;

        //ent_main.GetAnimator().SetBool("PunchTrigger", false);
        //ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;

        StartAI();
        return true;
    }
}