using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBossMeleeAttack : AIBase
{ 
    private float
        f_attack_range;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private EntityPlayer ep_player;

    private NavMeshAgent nma_agent;

    public AIBossMeleeAttack(int _priority, EntityLivingBase _entity, System.Type _type, float _attackrange)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Boss Melee Attack";
        b_is_interruptable = false;
        f_attack_range = _attackrange;
        type_target = _type;

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
        throw new System.NotImplementedException();
    }


    public override bool RunAI()
    {
        throw new System.NotImplementedException();
    }


    public override bool EndAI()
    {
        ent_main.B_isAttacking = false;
        b_has_attacked = false;
        return true;
    }




}
