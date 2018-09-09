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
        DODGE,
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
        f_player_speed;

    delegate void m_checkfunction();
    Dictionary<State, m_checkfunction> m_checkfuntions = new Dictionary<State, m_checkfunction>();

    public float F_player_speed
    {
        get
        {
            return f_player_speed;
        }

        set
        {
            f_player_speed = value;
        }
    }

    protected override void Start ()
    {
        base.Start();

        if (m_checkfuntions == null)
            m_checkfuntions = new Dictionary<State, m_checkfunction>();

        m_checkfuntions.Add(State.IDLE, IdleCheckFunction);
        m_checkfuntions.Add(State.MOVING, MovingCheckFunction);
        m_checkfuntions.Add(State.DASHING, DashingCheckFunction);
        m_checkfuntions.Add(State.DODGE, DodgeCheckFunction);
        m_checkfuntions.Add(State.ATTACK, AttackCheckFunction);
        m_checkfuntions.Add(State.SUMMONING, SummoningCheckFunction);
        m_checkfuntions.Add(State.DEAD, DeadCheckFunction);

        Stats temp_stats = new Stats();


        temp_stats.F_speed = 20;
        temp_stats.F_maxspeed = temp_stats.F_speed;
        temp_stats.F_health = 5;

        St_stats = temp_stats;
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

        if (Input.GetKey(KeyCode.Space))
        {
            player_state = State.DODGE;
           
            return;
        }

        St_stats.F_speed = St_stats.F_maxspeed;
        i_combo = 0;
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

        if (Input.GetKey(KeyCode.Space))
        {
            player_state = State.DODGE;

            return;
        }

        i_combo = 0;
    }

    private void DashingCheckFunction()
    {
        if(player_target_state != TARGET_STATE.AIMING)
            St_stats.F_speed = St_stats.F_maxspeed * 2;

        player_state = State.MOVING;
    }

    private void DodgeCheckFunction()
    {
        player_state = State.IDLE;
    }

    private void AttackCheckFunction()
    {  
       if(player_target_state == TARGET_STATE.AIMING)
        {

        }
       else
        {
            ++i_combo;

            if (i_combo > 3)
                i_combo = 1;
        }

        player_state = State.IDLE;
    }

    private void SummoningCheckFunction()
    {
        player_state = State.IDLE;
    }

    private void DeadCheckFunction()
    {
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

    protected override void Update ()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Mouse1))
        {
            player_target_state = TARGET_STATE.AIMING;
            if(St_stats.F_speed != St_stats.F_maxspeed)
                St_stats.F_speed = St_stats.F_maxspeed;
        }
        else
        {
            player_target_state = TARGET_STATE.NOT_AIMING;
        }

        m_checkfuntions[player_state]();

        Debug.Log(player_state);
    }  

    public override void OnAttack()
    {

    }

    public override void OnAttacked(DamageSource _dmgsrc)
    {

    }
}
