using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIArtyState : AIBase
{

    private float
        f_range,
        f_stateTimer,
        f_maxStateTimer,
        f_shotInterval,
        f_stateCooldownTimer,
        f_cooldown,
        f_aimTimer;

    private int
        i_shotToFire;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private RedMarker
        marker_targetCircle;

    private ArcBulllet
        ab_bullet;

    public AIArtyState(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _stateTime, float _cooldown, float _shotInterval)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Arty Attack";
        b_is_interruptable = false;
        f_range = _range;
        type_target = _type;
        f_maxStateTimer = _stateTime;
        f_cooldown = _cooldown;
        f_shotInterval = _shotInterval;
        i_shotToFire = Mathf.RoundToInt(f_maxStateTimer / f_shotInterval);

        f_aimTimer = 0;
        f_stateCooldownTimer = 0;

        b_has_attacked = false;
    }

    public override bool StartAI()
    {
        ent_main.B_isVulnerable = true;

        Reset();

        return true;
    }

    public override bool EndAI()
    {
        ent_main.B_isAttacking = false;
        ent_main.B_isVulnerable = false;
        Reset();
        //ent_main.GetAnimator().SetBool("PunchTrigger", false);
        //ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;
        return true;
    }


    public override bool ShouldContinueAI()
    {
        if (f_stateCooldownTimer < f_cooldown)
        {
            f_stateCooldownTimer += Time.deltaTime;
            return false;
        }

        // Breaking point for the arty state.
        if (f_stateTimer > f_maxStateTimer && i_shotToFire < 1)
        {
            i_shotToFire = Mathf.RoundToInt(f_maxStateTimer / f_shotInterval);
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
        if (!b_has_attacked)
        {
            b_has_attacked = true;

            marker_targetCircle = ObjectPool.GetInstance().GetIndicatorObjectFromPool(ObjectPool.INDICATOR.RED_MARKER).GetComponent<RedMarker>();
            marker_targetCircle.SetUpIndicator(ent_target.transform.position, f_shotInterval, 3.0f, 1, () =>
            {
                Fire();
            })
            .SetToFollow(ent_target.transform)
            .SetToRotate(new Vector3(0, 90, 0));
        }

        if (f_aimTimer < f_shotInterval)
        {
            //Lerp the Position for the targetCirce to the player.
            f_aimTimer += Time.deltaTime;
            //Vector3 currentPos = Vector3.Lerp(ent_main.transform.position, ent_target.transform.position, f_aimTimer);
            //currentPos.y = 0.5f;
            //go_targetCircle.transform.position = currentPos;
        }

    }

    void Fire()
    {
        //Temp Spawn of rock to the position of the circle
        b_has_attacked = false;
        ab_bullet = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.ARCH_PROJECTILE).GetComponent<ArcBulllet>();
        ab_bullet.SetUpProjectile(5, 20, 1, 10, ent_main.transform.position, ent_target.transform.position, new Vector3(2, 2, 2), ent_main.gameObject, null);
        f_aimTimer = 0;
        i_shotToFire--;

    }

    void Reset()
    {
        f_stateTimer = 0;
        f_aimTimer = 0;
        ent_target = null;
    }
}
