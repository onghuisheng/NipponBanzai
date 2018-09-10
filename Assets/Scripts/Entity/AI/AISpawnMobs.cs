using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnMobs : AIBase {

    private float
     f_range,
     f_stateTimer,
     f_maxStateTimer,
     f_cooldown,
     f_stateCooldownTimer;

    private System.Type
        type_target;

    private bool
        b_has_attacked;


    public AISpawnMobs(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _stateTime, float _cooldown)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Boss Spawn";
        b_is_interruptable = false;
        f_range = _range;
        type_target = _type;
        f_cooldown = _cooldown;
        f_maxStateTimer = _stateTime;

        f_stateCooldownTimer = 0;

        b_has_attacked = false;
    }

    public override bool StartAI()
    {
        ent_target = null;
        return true;
    }

    public override bool ShouldContinueAI()
    {
        throw new System.NotImplementedException();
    }


    public override bool RunAI()
    {
        throw new System.NotImplementedException();
    }


    public override bool EndAI()
    {
        return true;
    }
}
