using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityLivingBase
{
    public enum State
    {
        IDLE,
        MOVING,
        DASHING,
        ATTACK,
        SUMMONING,
        DEAD
    }

    public enum TARGET_STATE
    {
        AIMING,
        NOT_AIMING
    }

    public enum DIRECTION
    {
        FRONT = 1,
        BACK,
        RIGHT,
        LEFT
    }

    private State
        player_state;

    private DIRECTION
        player_dir;

    private TARGET_STATE
        player_target_state;

    private int
        i_combo;

    private float
        f_shooting_interval,
        f_shooting_max_interval,

        f_charged_amount,
        f_charged_max_amount,

        f_charged_increase_amount;

    private bool
        b_is_charging_shot;

    private List<GameObject>
        list_joints;

    //private List<Transform>
    //    list_last_joint_transform;

    delegate void m_checkfunction();
    Dictionary<State, m_checkfunction> m_checkfuntions = new Dictionary<State, m_checkfunction>();

    protected override void Start()
    {
        base.Start();

        if (m_checkfuntions == null)
            m_checkfuntions = new Dictionary<State, m_checkfunction>();

        list_joints = new List<GameObject>();
        // list_last_joint_transform = new List<Transform>();

        m_checkfuntions.Add(State.IDLE, IdleCheckFunction);
        m_checkfuntions.Add(State.MOVING, MovingCheckFunction);
        m_checkfuntions.Add(State.DASHING, DashingCheckFunction);
        m_checkfuntions.Add(State.ATTACK, AttackCheckFunction);
        m_checkfuntions.Add(State.SUMMONING, SummoningCheckFunction);
        m_checkfuntions.Add(State.DEAD, DeadCheckFunction);

        St_stats = new Stats();
        St_stats.F_maxspeed = St_stats.F_speed = 5;
        St_stats.F_max_health = St_stats.F_health = 100;
        St_stats.F_damage = 1;
        St_stats.F_mass = 1;

        f_shooting_max_interval = f_shooting_interval = 0.1f;
        f_charged_amount = 1.0f;
        f_charged_max_amount = 2;
        f_charged_increase_amount = 0.5f;
        b_is_charging_shot = false;
        i_combo = 1;
        
        foreach (Transform _trans in gameObject.GetComponentsInChildren<Transform>())
        {
            if (TagHelper.IsTagJoint(_trans.gameObject.tag))
                list_joints.Add(_trans.gameObject);
        }
    }

    private void IdleCheckFunction()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            player_state = State.MOVING;
            return;
        }

        if (DoubleTapCheck.GetInstance().IsDoubleTapTriggered() &&
             (DoubleTapCheck.GetInstance().GetDoubleTapKey() == KeyCode.W
             || DoubleTapCheck.GetInstance().GetDoubleTapKey() == KeyCode.A
             || DoubleTapCheck.GetInstance().GetDoubleTapKey() == KeyCode.S
             || DoubleTapCheck.GetInstance().GetDoubleTapKey() == KeyCode.D))
        {
            player_state = State.DASHING;

            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            player_state = State.ATTACK;

            return;
        }

        St_stats.F_speed = St_stats.F_maxspeed;
        i_combo = 1;
        player_dir = DIRECTION.FRONT;
    }

    private void MovingCheckFunction()
    {
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            player_state = State.IDLE;
        }

        if (DoubleTapCheck.GetInstance().IsDoubleTapTriggered() &&
            (DoubleTapCheck.GetInstance().GetDoubleTapKey() == KeyCode.W
            || DoubleTapCheck.GetInstance().GetDoubleTapKey() == KeyCode.A
            || DoubleTapCheck.GetInstance().GetDoubleTapKey() == KeyCode.S
            || DoubleTapCheck.GetInstance().GetDoubleTapKey() == KeyCode.D))
        {
            player_state = State.DASHING;

            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            player_state = State.ATTACK;

            return;
        }

        i_combo = 1;
    }

    private void DashingCheckFunction()
    {
        if (player_target_state != TARGET_STATE.AIMING)
            St_stats.F_speed = St_stats.F_maxspeed * 2;

        player_state = State.MOVING;
    }

    private void AttackCheckFunction()
    {
        if (An_animator.GetBool("IsAttacking"))
        {
            if (player_target_state == TARGET_STATE.AIMING && !An_animator.GetBool("IsMelee"))
            {
                if (DoubleTapCheck.GetInstance().IsDoubleClickTriggered() && DoubleTapCheck.GetInstance().GetDoubleTapMouseKey() == KeyCode.Mouse0 && !b_is_charging_shot)
                {
                    b_is_charging_shot = true;
                }

                if (b_is_charging_shot)
                {
                    if (f_charged_amount < f_charged_max_amount)
                        f_charged_amount += f_charged_increase_amount * Time.deltaTime;
                    else if (f_charged_amount > f_charged_max_amount)
                        f_charged_amount = f_charged_max_amount;

                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * 25;

                        StraightBullet sb = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.STRAIGHT_PROJECTILE).GetComponent<StraightBullet>();
                        sb.SetUpProjectile(gameObject, target - gameObject.transform.position, 5, St_stats.F_damage * f_charged_amount, 40, new Vector3(f_charged_amount * 0.25f, f_charged_amount * 0.25f, f_charged_amount * 0.25f));

                        if (b_is_charging_shot)
                            b_is_charging_shot = false;

                        if (f_charged_amount != 1)
                            f_charged_amount = 1;

                        f_shooting_interval = 0;

                        An_animator.SetBool("IsAttacking", false);
                    }
                }
                else
                {

                    if (f_shooting_interval >= f_shooting_max_interval)
                    {
                        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * 25;

                        StraightBullet sb = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.STRAIGHT_PROJECTILE).GetComponent<StraightBullet>();
                        sb.SetUpProjectile(gameObject, target - gameObject.transform.position, 5, St_stats.F_damage * f_charged_amount, 40, new Vector3(f_charged_amount * 0.25f, f_charged_amount * 0.25f, f_charged_amount * 0.25f));

                        f_shooting_interval = 0;
                    }
                }
            }
            else
            {
                if (b_is_charging_shot)
                    b_is_charging_shot = false;

                if (f_charged_amount != 1)
                    f_charged_amount = 1;

                An_animator.SetBool("IsMelee", true);
            }
        }

        if (!b_is_charging_shot && !An_animator.GetBool("IsAttacking"))
        {
            player_state = State.IDLE;
        }
    }

    private void SummoningCheckFunction()
    {
        player_state = State.IDLE;
    }

    private void DeadCheckFunction()
    {
        if (!IsDead())
            player_state = State.IDLE;
    }

    public State GetPlayerState()
    {
        return player_state;
    }

    public TARGET_STATE GetPlayerTargetState()
    {
        return player_target_state;
    }

    public DIRECTION GetPlayerDir()
    {
        return player_dir;
    }

    public void SetPlayerState(State _player_new_state)
    {
        player_state = _player_new_state;
    }

    protected override void Update()
    {
        base.Update();

        An_animator.SetFloat("MoveSpeed", GetStats().F_speed / GetStats().F_maxspeed);
        An_animator.SetInteger("Combo", i_combo);

        m_checkfuntions[player_state]();

        if (player_state == State.DASHING)
            B_isDodging = true;
        else
            B_isDodging = false;

        if (IsDead())
        {
            player_state = State.DEAD;
            player_target_state = TARGET_STATE.NOT_AIMING;
        }

        if (!IsDead())
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                //if(list_last_joint_transform.Count == 0)
                //{
                //    foreach(GameObject go in list_joints)
                //    {
                //        list_last_joint_transform.Add(go.transform);
                //    }
                //}

                player_target_state = TARGET_STATE.AIMING;
                if (St_stats.F_speed != St_stats.F_maxspeed)
                    St_stats.F_speed = St_stats.F_maxspeed;

                if (!b_is_charging_shot)
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                        An_animator.SetBool("IsShooting", true);
                    else
                        An_animator.SetBool("IsShooting", false);
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        An_animator.SetBool("IsShooting", true);
                    }
                    else
                    {
                        An_animator.SetBool("IsShooting", false);
                    }
                }

            }
            else
            {
                player_target_state = TARGET_STATE.NOT_AIMING;
                An_animator.SetBool("IsShooting", false);
            }

            if (f_shooting_interval < f_shooting_max_interval)
                f_shooting_interval += Time.deltaTime;
        }

        switch (player_target_state)
        {
            case TARGET_STATE.AIMING:
                An_animator.SetBool("IsAiming", true);
                break;
            case TARGET_STATE.NOT_AIMING:
                An_animator.SetBool("IsAiming", false);
                break;

            default:
                An_animator.SetBool("IsAiming", false);
                break;
        }

        switch (player_state)
        {
            case State.ATTACK:
                An_animator.SetBool("IsDead", false);
                An_animator.SetBool("IsMoving", false);
                An_animator.SetBool("IsAttacking", true);
                break;

            case State.DASHING:
                An_animator.SetBool("IsDead", false);
                break;

            case State.DEAD:
                An_animator.SetBool("IsMoving", false);
                An_animator.SetBool("IsDead", true);
                An_animator.SetBool("IsAttacking", false);
                break;

            case State.IDLE:
                An_animator.SetBool("IsMoving", false);
                An_animator.SetBool("IsDead", false);
                break;

            case State.MOVING:
                An_animator.SetBool("IsDead", false);

                An_animator.SetBool("IsMoving", true);
                break;

            case State.SUMMONING:
                An_animator.SetBool("IsMoving", false);
                An_animator.SetBool("IsDead", false);
                break;

            default:
                An_animator.SetBool("IsMoving", false);
                An_animator.SetBool("IsDead", false);
                An_animator.SetBool("IsAttacking", false);
                break;
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        // Debug.Log("Player Health: " + St_stats.F_health + "/" + St_stats.F_max_health);
        // Debug.Log(player_state);
        if (player_target_state == TARGET_STATE.AIMING)
        {
            foreach (GameObject go in list_joints)
            {
                go.transform.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * 100);
                go.transform.localEulerAngles = new Vector3(go.transform.localRotation.x + 45,
                     go.transform.localRotation.y,
                     go.transform.localRotation.z + 45);
            }
        }
    }

    public override void OnAttack()
    {
        SetUpHitBox(gameObject.name, gameObject.tag, gameObject.GetInstanceID().ToString(), St_stats.F_damage, Vector3.one, transform.position + (transform.forward * GetComponent<Collider>().bounds.extents.magnitude), transform.rotation);
    }

    public override void OnAttacked(DamageSource _dmgsrc, float _timer = 0.5f)
    {
        if (!IsDead() && !B_isDodging && !B_isHit)
        {
            S_last_hit = _dmgsrc.GetName();
            St_stats.F_health -= _dmgsrc.GetDamage();
            An_animator.SetTrigger("IsHit");
            ResetOnHit(_timer);
        }
    }

    public void EndAttackAnimation()
    {
        An_animator.SetBool("IsAttacking", false);
        An_animator.SetBool("IsMelee", false);
        ++i_combo;

        if (i_combo > 3)
            i_combo = 1;
    }
}
