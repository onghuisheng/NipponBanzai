using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRoam : AIBase
{ 
    private Vector3
        v3_spawn_position,
        v3_target_position;

    private float
        f_timer,
        f_roaming_space;

    public AIRoam(int _priority, EntityLivingBase _entity, float _roaming_space)
    {
        i_priority = _priority;
        ent_main = _entity;
        v3_spawn_position = new Vector3(0, 0, 0);
        s_ID = "Movement";
        s_display_name = "Roaming";
        b_is_interruptable = true;
        f_timer = 0.0f;
        f_roaming_space = _roaming_space;
    }

    public override bool StartAI()
    {
        if (v3_spawn_position == new Vector3(0, 0, 0))
            v3_spawn_position = ent_main.GetPosition();

        v3_target_position = new Vector3(
            v3_spawn_position.x + Random.Range(-f_roaming_space, f_roaming_space),
            v3_spawn_position.y,
            v3_spawn_position.z + Random.Range(-f_roaming_space, f_roaming_space));

        f_timer = 0.0f;


        return true;
    }

    public override bool ShouldContinueAI()
    {
        if ((ent_main.GetPosition().x > v3_target_position.x - 2 && ent_main.GetPosition().x < v3_target_position.x + 2 && ent_main.GetPosition().z > v3_target_position.z - 2 && ent_main.GetPosition().z < v3_target_position.z + 2)
            || f_timer > f_roaming_space * 2
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
        v3_target_position.y = ent_main.GetPosition().y;

        f_timer += Time.deltaTime;
        //ent_main.MoveTowardsPosition(v3_target_position);
        //ent_main.RotateTowardsTargetPosition(v3_target_position);

        return true;
    }

    public override bool EndAI()
    {
        //ent_main.GetAnimator().SetBool("Walk Forward", false);
        //ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;

        StartAI();
        return true;
    }
}
