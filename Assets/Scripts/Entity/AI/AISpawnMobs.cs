using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnMobs : AIBase
{

    private float
     f_spawnRadius,
     f_cooldown,
     f_nextCooldownTime;

    private System.Type
        type_target;

    List<ObjectPool.ENEMY> list_enemiesToSpawn;
    

    public AISpawnMobs(int _priority, EntityLivingBase _entity, System.Type _type, float _spawnRadius, float _cooldown, List<ObjectPool.ENEMY> _enemiesToSpawn)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Spawn Mobs";
        b_is_interruptable = false;
        f_spawnRadius = _spawnRadius;
        type_target = _type;
        f_cooldown = _cooldown;
        list_enemiesToSpawn = _enemiesToSpawn;
    }

    public override bool StartAI()
    {
        ent_target = null;
        return true;
    }

    public override bool ShouldContinueAI()
    {
        if (Time.time < f_nextCooldownTime)
        {
            return false;
        }
        else
        {
            


            return true;
        }
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
