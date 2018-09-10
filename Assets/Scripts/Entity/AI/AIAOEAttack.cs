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
        f_gravitiyConstant;

    private System.Type
        type_target;

    private bool
        b_has_attacked;


    public AIAOEAttack(int _priority, EntityLivingBase _entity, System.Type _type, float _range, float _suctionTime)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "AOE Attack";
        b_is_interruptable = false;
        f_range = _range;
        type_target = _type;
        f_suctionLifeSpan = _suctionTime;
        f_suctionTimer = 0;

        f_aoeRange = 20;
        f_force = 5000f;
        f_upwardForce = 0.0f;
        f_gravitiyConstant = 9.8f;
        f_stateCooldownTimer = 0.0f;
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
        //b_has_attacked = false;
        ent_main.B_isAttacking = true;

        //ent_main.GetAnimator().SetBool("PunchTrigger", false);
        //ent_main.GetAnimator().speed = ent_main.F_defaultAnimationSpeed;

        StartAI();
        return true;
    }


    public override bool ShouldContinueAI()
    {
        if (f_stateCooldownTimer < 12)
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
        if (ent_target != null)
        {
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
                    ent_target.Rb_rigidbody.AddExplosionForce(f_force, ent_main.GetPosition(), f_aoeRange, f_upwardForce, ForceMode.Impulse);
                    b_has_attacked = true;
                }

            }

            //ent_main.RotateTowardsTargetPosition(ent_target.GetPosition());
        }

        return true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

        if (Physics.Linecast(ent_main.GetPosition(), ent_target.GetPosition(), out hit))
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
