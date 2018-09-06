using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackMelee : AIBase
{
    private float
        f_range;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    public AIAttackMelee(int _priority, EntityLivingBase _entity, System.Type _type, float _range)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Melee Attack";
        b_is_interruptable = false;
        f_range = _range;
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

        if (ent_target == null)
        {
            foreach (var list in ObjectPool.GetInstance().GetAllEntity())
            {
                foreach (GameObject l_go in list)
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
        }

        if (ent_target != null && ent_target.IsDead())
        {
            ent_target = null;
        }

        if (ent_target == null)
        {
            return false;
        }

        ent_main.B_isAttacking = true;
        //ent_main.GetAnimator().SetBool("PunchTrigger", true);
        //ent_main.GetAnimator().speed = ent_main.F_attack_speed;

        return true;
    }

    public override bool RunAI()
    {

        if (ent_target != null)
        {
            //if (ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= (ent_main.F_totalAnimationLength * 0.9f) && ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && !b_has_attacked)
            //{
            //    b_has_attacked = true;
            //    ent_main.OnAttack();
            //} 

            //ent_main.RotateTowardsTargetPosition(ent_target.GetPosition());
            ent_main.GetComponent<NavMeshAgent>().destination = ent_target.transform.position;

        }

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