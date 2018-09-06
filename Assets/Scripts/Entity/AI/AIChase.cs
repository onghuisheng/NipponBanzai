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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}
