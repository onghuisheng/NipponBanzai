using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAOEAttack : AIBase {

    private float
        f_range,
        f_aoeRange,
        f_suctionTimer,
        f_suctionLifeSpan,
        f_force,
        f_upwardForce,
        f_stateCooldownTimer,
        f_cooldown,
        f_gravitiyConstant;

    private System.Type
        type_target;

    private bool
        b_has_attacked;

    private EntityBoss
        script_boss;


    public AIAOEAttack(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _suctionTime, float _cooldown)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "AOE Attack";
        b_is_interruptable = false;
        f_range = _range;
        type_target = _type;
        f_suctionLifeSpan = _suctionTime;
        f_cooldown = _cooldown;

        //Force Values
        f_aoeRange = 20;
        f_force = 5000f;
        f_upwardForce = 0.0f;
        f_gravitiyConstant = 9.8f;

        //Timers
        f_stateCooldownTimer = 0.0f;
        f_suctionTimer = 0.0f;
        b_has_attacked = false;

        script_boss = ent_main.GetComponent<EntityBoss>();
    }

    public override bool StartAI()
    {
        ent_target = null;
        script_boss.Enum_currentAttState = EntityBoss.AttackState.GRAVITY;
        return true;
    }

    public override bool EndAI()
    {
        //ent_main.B_isAttacking = false;
        //b_has_attacked = false;
        ent_main.B_isAttacking = true;

        //ent_main.GetAnimator().SetBool("PunchTrigger", false);
        //ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;

        StartAI();
        script_boss.Enum_currentAttState = EntityBoss.AttackState.NONE;
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
        if (b_has_attacked)
        {
            f_stateCooldownTimer = 0;
            b_has_attacked = false;
            f_suctionTimer = 0;
            return false;
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
        if (ent_target != null)
        {
            if (AnimatorExtensions.HasParameterOfType(ent_main.An_animator, "AttackState", AnimatorControllerParameterType.Int))
            {
                ent_main.An_animator.SetInteger("AttackState", (int)script_boss.Enum_currentAttState);
            }
            //if (ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= (ent_main.F_totalAnimationLength * 0.9f) && ent_main.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && !b_has_attacked)
            //{
            //    b_has_attacked = true;
            //    ent_main.OnAttack();
            //}
            f_suctionTimer += Time.deltaTime;

            if (f_suctionTimer < f_suctionLifeSpan)
            {
                Debug.Log("SUCKING");

                if (!IsPlayerInCover())
                {
                    ent_target.Rb_rigidbody.AddForce(GAcceleration(ent_main.GetPosition(), ent_main.Rb_rigidbody.mass, ent_target.Rb_rigidbody));
                }  
            }
            else
            {
                if (!b_has_attacked)
                {
                    Debug.Log("KABOOM");
                    if (!IsPlayerInCover())
                    {
                        ent_target.Rb_rigidbody.AddExplosionForce(f_force, ent_main.GetPosition(), f_aoeRange, f_upwardForce, ForceMode.Acceleration);
                    }

                    b_has_attacked = true;

                    var crystalList = ObjectPool.GetInstance().GetActiveEnvironmentObjects();

                    foreach(GameObject i in crystalList)
                    {
                        EntityEnviroment envir = i.GetComponent<EntityEnviroment>();

                        if (envir.B_isDestructible)
                        {
                            i.SetActive(false);
                        }
                    }
                }

            }

            //ent_main.RotateTowardsTargetPosition(ent_target.GetPosition());
        }

        return true;
    }

    public Vector3 GAcceleration(Vector3 position, float mass, Rigidbody r)
    {
        Vector3 direction = position - r.position;

        //Realist GF with increasing force when getting closer to each other.
        //float gravityForce = f_gravitiyConstant * ((mass * r.mass * 1000) / direction.sqrMagnitude);
        //Simple GF with linear force.
        float gravityForce = f_gravitiyConstant * (mass * r.mass * 1000);
        gravityForce /= r.mass;
        //Debug.Log("gravityForce: " + gravityForce);

        return direction.normalized * gravityForce * Time.fixedDeltaTime;
    }

    bool IsPlayerInCover()
    {
        RaycastHit hit;

        Vector3 direction = ent_target.GetPosition() - ent_main.GetPosition();

        int ignoreEnemiesMask = ~(1 << LayerMask.NameToLayer("Enemies"));

        if (Physics.Linecast(ent_main.GetPosition(), ent_target.GetPosition(), out hit, ignoreEnemiesMask))
        {
            Debug.DrawLine(ent_main.GetPosition(), ent_target.GetPosition(), Color.yellow);
            Debug.Log("Did Hit : " + hit.collider.gameObject);

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return false;
            }

            return true;
        }

        return false;
    }
}
