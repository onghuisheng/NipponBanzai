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

    private EntityBoss
        script_boss;

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

        script_boss = ent_main.GetComponent<EntityBoss>();
    }

    public override bool StartAI()
    {
        ent_main.B_isVulnerable = true;

        script_boss.As_currentAttState = EntityBoss.AttackState.ARTY;
        Reset();

        return true;
    }

    public override bool EndAI()
    {
        ent_main.B_isAttacking = false;
        ent_main.B_isVulnerable = false;
        Reset();

        ent_main.An_animator.SetBool("Shooting", false);
        script_boss.As_currentAttState = EntityBoss.AttackState.NONE;
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
            Debug.Log("Brek");
            i_shotToFire = Mathf.RoundToInt(f_maxStateTimer / f_shotInterval);
            f_stateCooldownTimer = 0;
            b_has_attacked = false;
            return false;
        }

        f_stateTimer += Time.deltaTime;

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

        //ent_main.GetAnimator().SetInt("AttackState", script_boss.As_currentAttState);
        //ent_main.GetAnimator().speed = ent_main.F_attack_speed;

        ent_main.B_isAttacking = true;

        return true;
    }


    public override bool RunAI()
    {
        if (ent_target != null)
        {
            if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "AttackState", AnimatorControllerParameterType.Int))
            {
                ent_main.An_animator.SetInteger("AttackState", (int)script_boss.As_currentAttState);
            }
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

        Vector3 bulletSpawnPos = ent_main.transform.position;
        bulletSpawnPos.y += 5;

        ab_bullet.SetUpProjectile(5, 20, 1, 10, bulletSpawnPos, ent_target.transform.position, new Vector3(2, 2, 2), ent_main.gameObject, (collider) =>
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                //Apply Knockback
                float range = 6;
                Collider[] colliders = Physics.OverlapSphere(ab_bullet.GetPosition(), range);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb && rb.tag.Equals("Player"))
                    {
                        rb.AddExplosionForce(15000f, ab_bullet.GetPosition(), range * 2, 0, ForceMode.Acceleration);
                    }
                }

                //Spawn Crystal
                Crystal spawnedCrystal = ObjectPool.GetInstance().GetEnviromentObjectFromPool(ObjectPool.ENVIRONMENT.CRYSTAL).GetComponent<Crystal>();
                spawnedCrystal.SetUpCrystal(ent_main.gameObject, 15, ab_bullet.GetPosition(), new Vector3(3, 3, 3), true);
                ab_bullet.gameObject.SetActive(false);
            }
        }, () =>
        {
            //Apply Knockback
            float range = 6;
            Collider[] colliders = Physics.OverlapSphere(ab_bullet.GetPosition(), range);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb && rb.tag.Equals("Player"))
                {
                    rb.AddExplosionForce(15000f, ab_bullet.GetPosition(), range * 2, 0, ForceMode.Acceleration);
                }
            }

            //Spawn Crystal
            Crystal spawnedCrystal = ObjectPool.GetInstance().GetEnviromentObjectFromPool(ObjectPool.ENVIRONMENT.CRYSTAL).GetComponent<Crystal>();
            spawnedCrystal.SetUpCrystal(ent_main.gameObject, 15, ab_bullet.GetPosition(), new Vector3(3, 3, 3), true);
            ab_bullet.gameObject.SetActive(false);
        });

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
