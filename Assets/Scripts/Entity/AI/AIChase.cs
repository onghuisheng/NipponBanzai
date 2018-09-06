using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : AIBase
{
    private Vector3
        v3_target_position;

    private System.Type
        type_target;

    private float
        f_range;

    public AIChase(int _priority, EntityLivingBase _entity, System.Type _type, float _range)
    {
        f_range = _range;
        i_priority = _priority;
        ent_main = _entity;
        type_target = _type;
        s_ID = "Movement";
        s_display_name = "Chase Target - " + type_target;
        b_is_interruptable = true;
    }

    public override bool StartAI()
    {
        ent_target = null;
        return true;
    }

    public override bool ShouldContinueAI()
    {
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

        if ((ent_main.GetPosition().x > ent_target.GetPosition().x - 1 && ent_main.GetPosition().x < ent_target.GetPosition().x + 1 && ent_main.GetPosition().z > ent_target.GetPosition().z - 1 && ent_main.GetPosition().z < ent_target.GetPosition().z + 1)
            || !(ent_main.GetPosition().x > ent_target.GetPosition().x - f_range && ent_main.GetPosition().x < ent_target.GetPosition().x + f_range && ent_main.GetPosition().z > ent_target.GetPosition().z - f_range && ent_main.GetPosition().z < ent_target.GetPosition().z + f_range)
            || ent_main.B_isAttacking)
        {
            EndAI();
            return false;
        }

        //ent_main.GetAnimator().SetBool("Walk Forward", true);
        //ent_main.GetAnimator().speed = ent_main.F_speed;

        return true;
    }

    public override bool RunAI()
    {
        if (ent_target != null)
        {
            v3_target_position = new Vector3(ent_target.GetPosition().x,
                ent_main.GetPosition().y,
                ent_target.GetPosition().z);

            //ent_main.MoveTowardsPosition(v3_target_position);
            //ent_main.RotateTowardsTargetPosition(v3_target_position);
        }

        return true;
    }

    public override bool EndAI()
    {
        StartAI();
        return true;
    }
}
