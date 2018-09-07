﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIArtyState : AIBase {

    private float
        f_range,
        f_stateTimer,
        f_maxStateTimer,
        f_shotInterval,
        f_aimTimer;

    private int
        i_shotToFire;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private GameObject
        go_targetCircle,
        go_crystal;

    public AIArtyState(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _stateTime, float _shotInterval)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Arty Attack";
        b_is_interruptable = false;
        f_range = _range;
        type_target = _type;
        f_maxStateTimer = _stateTime;
        f_shotInterval = _shotInterval;
        i_shotToFire = Mathf.RoundToInt(f_maxStateTimer / f_shotInterval);

        f_aimTimer = 0;


        b_has_attacked = false;
    }

    public override bool StartAI()
    {
        ent_target = null;
        return true;
    }

    public override bool EndAI()
    {
        //ent_main.B_isAttacking = false;
        b_has_attacked = false;

        //ent_main.GetAnimator().SetBool("PunchTrigger", false);
        //ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;

        StartAI();
        return true;
    }


    public override bool ShouldContinueAI()
    {
        f_stateTimer += Time.deltaTime;

        // Breaking point for the arty state.
        if (f_stateTimer > f_maxStateTimer && i_shotToFire < 1)
        {
            return false;
        }

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

        //ent_main.GetAnimator().SetBool("PunchTrigger", true);
        //ent_main.GetAnimator().speed = ent_main.F_attack_speed;
        return true;
    }


    public override bool RunAI()
    {
        Debug.Log("RUNNING");
        if (ent_target != null)
        {
            //if (ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= (ent_main.F_totalAnimationLength * 0.9f) && ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && !b_has_attacked)
            //{
            //    b_has_attacked = true;
            //    ent_main.OnAttack();
            //}
            if (i_shotToFire > 0)
            {
                AimTarget();
            }


            //ent_main.RotateTowardsTargetPosition(ent_target.GetPosition());
        }

        return true;
    }

    void AimTarget()
    {
        //Loads up the gameobject to be use for this shot.
        if (!go_targetCircle || !go_targetCircle.activeSelf) 
        {
            go_targetCircle = ObjectPool.GetInstance().GetEntityObjectFromPool(4);
            go_crystal = ObjectPool.GetInstance().GetEntityObjectFromPool(3);
        }

        if (f_aimTimer > 3)
        {
            Fire();
        }
        else
        {
            //Set Position for the targetCirce to the player.
            f_aimTimer += Time.deltaTime;
            go_targetCircle.transform.position = new Vector3(ent_target.transform.position.x, 0.5f, ent_target.transform.position.z);
            Debug.Log("AIMING:" + f_aimTimer);
        }
    }

    void Fire()
    {
        //Temp Spawn of rock to the position of the circle
        if (go_crystal)
        {
            Debug.Log("FIRE");
            go_crystal.transform.position = ent_target.transform.position;
            go_targetCircle.SetActive(false);
            f_aimTimer = 0;

        }
    }
}
