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
        f_maxBeamCharge,
        f_stateCooldownTimer;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private int
        i_numOfLasers;

    private Laser
        script_laser;

    private EntityBoss
        script_boss;


    public AIBossLaser(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _stateTime, float _cooldown)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Boss Laser Attack";
        b_is_interruptable = false;
        f_range = _range;
        type_target = _type;
        f_cooldown = _cooldown;
        f_maxStateTimer = _stateTime;

        f_stateCooldownTimer = f_cooldown;
        f_beamChargeTimer = 0;
        i_numOfLasers = 4; //For now.
        f_maxBeamCharge = 4.5f;

        b_has_attacked = false;

        script_boss = ent_main.GetComponent<EntityBoss>();
    }

    public override bool StartAI()
    {
        ent_main.B_isVulnerable = true;

        script_boss.NextAttackState(EntityBoss.AttackState.LASER);
        script_boss.NextChargeState(EntityBoss.ChargeState.STAGE_1);
        Reset();

        return true;
    }

    public override bool EndAI()
    {
        Debug.Log("end");
        ent_main.B_isAttacking = false;
        ent_main.B_isVulnerable = false;

        script_boss.NextAttackState(EntityBoss.AttackState.NONE);
        script_boss.NextChargeState(EntityBoss.ChargeState.NONE);

        Reset();

        return true;
    }


    public override bool ShouldContinueAI()
    {
        if (f_stateCooldownTimer < f_cooldown)
        {
            f_stateCooldownTimer += Time.deltaTime;
            return false;
        }

        // Breaking point
        if (f_stateTimer > f_maxStateTimer && script_boss.Enum_currentChargeState == EntityBoss.ChargeState.END)
        {
            if (ent_main.An_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("break");
                f_stateCooldownTimer = 0;
                b_has_attacked = false;
                return false;
            }
        }
        else
        {
            if (script_boss.Enum_currentChargeState >= EntityBoss.ChargeState.STAGE_2)
            {
                f_stateTimer += Time.deltaTime;
            }
        }



        if (ent_target == null)
        {
            foreach (GameObject l_go in ObjectPool.GetInstance().GetActiveEntityObjects())
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
        if (ent_main.B_isAttacking == true && f_beamChargeTimer > f_maxBeamCharge)
        {
            if (b_has_attacked == false && script_boss.Enum_currentChargeState == EntityBoss.ChargeState.STAGE_2)
            {
                //OnTargetBeam();
                for (int i = 0; i < i_numOfLasers; ++i)
                {
                    b_has_attacked = true;
                    AOEBeam(i);
                }

            }

            if (f_beamChargeTimer > f_maxStateTimer)
            {
                Debug.Log("CHAGE");
                script_boss.NextChargeState(EntityBoss.ChargeState.END);
                f_beamChargeTimer = 0;
            }

            //ent_main.RotateTowardsTargetPosition(ent_target.GetPosition());
        }

        f_beamChargeTimer += Time.deltaTime;


        return true;
    }

    private Vector3 RandomCircle(Vector3 _center, float _radius, float _angle) 
     { // create random angle between 0 to 360 degrees
        Vector3 pos;
        pos.x = _center.x + _radius * Mathf.Sin(_angle * Mathf.Deg2Rad);
        //pos.y = _center.y + _radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = _center.y - 10.0f;
        pos.z = _center.z + _radius * Mathf.Cos(_angle * Mathf.Deg2Rad);
        return pos;
    }

    private void AOEBeam(int _index)
    {
        script_laser = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.LASER_PROJECTILE).GetComponent<Laser>();

        var pos = RandomCircle(ent_main.transform.position, 10, _index * 90);
        Vector3 direction = pos - ent_main.transform.position;
        Vector3 corePosition = ent_main.transform.position;
        corePosition.y += 7.5f;
        float lazerLife = f_maxStateTimer - f_stateTimer;
        if (lazerLife > 9.5) lazerLife = 9.5f;
        script_laser.SetUpProjectile(lazerLife, 2, 0.05f, corePosition, direction, new Vector3(2, 2, 2), ent_main.gameObject, true);
        //script_laser.SetUpProjectile(f_maxStateTimer, 2, 0.05f, corePosition, direction, new Vector3(2, 2, 2), ent_main.gameObject, true);
    }

    private void OnTargetBeam()
    {
        script_laser = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.LASER_PROJECTILE).GetComponent<Laser>();
        Vector3 corePosition = ent_main.transform.position;
        corePosition.y += 7.5f;
        script_laser.SetUpProjectile(f_maxStateTimer - f_stateTimer, 2, 0.05f, corePosition, ent_target.transform.position, new Vector3(2, 2, 2), ent_main.gameObject, false);
    }

    void Reset()
    {
        f_stateTimer = 0;
        ent_target = null;
    }
}
