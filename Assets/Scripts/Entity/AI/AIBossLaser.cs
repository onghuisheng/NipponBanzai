using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossLaser : AIBase {

    private float
        f_range,
        f_stateTimer,
        f_maxStateTimer,
        f_cooldown,
        f_beamChargeTimer,
        f_stateCooldownTimer;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private int
        i_numOfLasers;

    private Laser
        script_laser;


    public AIBossLaser(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _stateTime, float _cooldown)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Boss SPIN Attack";
        b_is_interruptable = false;
        f_range = _range;
        type_target = _type;
        f_cooldown = _cooldown;
        f_maxStateTimer = _stateTime;

        f_stateCooldownTimer = 0;
        f_beamChargeTimer = 0;
        i_numOfLasers = 4; //For now.

        b_has_attacked = false;
    }

    public override bool StartAI()
    {
        f_stateTimer = 0;

        ent_target = null;
        return true;
    }

    public override bool EndAI()
    {
        ent_main.B_isAttacking = false;


        //ent_main.GetAnimator().SetBool("PunchTrigger", false);
        //ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;

        StartAI();
        return true;
    }


    public override bool ShouldContinueAI()
    {
        if (f_stateCooldownTimer < f_maxStateTimer)
        {
            f_stateCooldownTimer += Time.deltaTime;
            return false;
        }

        // Breaking point
        if (f_stateTimer > f_maxStateTimer)
        {
            f_stateCooldownTimer = 0;
            b_has_attacked = false;
            return false;
        }

        f_stateTimer += Time.deltaTime;

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

        ent_main.B_isAttacking = true;

        return true;
    }


    public override bool RunAI()
    {
        Debug.Log("RUNNING");

        f_beamChargeTimer += Time.deltaTime;
        if (ent_target != null && f_beamChargeTimer > 5)
        {
            //if (ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= (ent_main.F_totalAnimationLength * 0.9f) && ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && !b_has_attacked)
            //{
            //    b_has_attacked = true;
            //    ent_main.OnAttack();
            //}

            //OnTargetBeam();
            for (int i = 0; i < i_numOfLasers; ++i)
            {

                AOEBeam(i);


            }
            

            f_beamChargeTimer = 0;

            //ent_main.RotateTowardsTargetPosition(ent_target.GetPosition());
        }

        return true;
    }

    private Vector3 RandomCircle(Vector3 _center, float _radius, float _angle) 
     { // create random angle between 0 to 360 degrees
        Vector3 pos;
        pos.x = _center.x + _radius * Mathf.Sin(_angle * Mathf.Deg2Rad);
        //pos.y = _center.y + _radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = _center.y;
        pos.z = _center.z + _radius * Mathf.Cos(_angle * Mathf.Deg2Rad);
        return pos;
    }

    private void AOEBeam(int _index)
    {
        script_laser = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.LASER_PROJECTILE).GetComponent<Laser>();

        var pos = RandomCircle(ent_main.transform.position, 10, _index * 90);
        Vector3 direction = pos - ent_main.transform.position;
        script_laser.SetUpProjectile(3, 20, 0.05f, ent_main.transform.position, direction, new Vector3(2, 2, 2), ent_main.gameObject);
    }

    private void OnTargetBeam()
    {
        script_laser = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.LASER_PROJECTILE).GetComponent<Laser>();

        script_laser.SetUpProjectile(3, 20, 0.05f, ent_main.transform.position, ent_target.transform.position, new Vector3(2, 2, 2), ent_main.gameObject);
    }
}
