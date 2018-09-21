using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateReset : StateMachineBehaviour {

    private EntityBoss
        script_entityBoss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script_entityBoss = animator.GetComponent<EntityBoss>();
    }

    // This will be called once the animator has transitioned out of the state.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script_entityBoss.NextAttackState(EntityBoss.AttackState.NONE);
        script_entityBoss.NextChargeState(EntityBoss.ChargeState.NONE);
        script_entityBoss.SetStateDone(true);
    }
}
