using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnMobs : AIBase {

    public AISpawnMobs(int _priority, EntityLivingBase _entity, System.Type _type, float _attackrange)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Boss Melee Attack";
        //b_is_interruptable = false;
        //f_attack_range = _attackrange;
        //type_target = _type;

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
