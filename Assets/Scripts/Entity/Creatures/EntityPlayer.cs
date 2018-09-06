using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityLivingBase
{
    public enum State
    {
        IDLE,
        MOVING,
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

    delegate void m_checkfunction();
    Dictionary<State, m_checkfunction> m_checkfuntions = new Dictionary<State, m_checkfunction>();

    protected override void Start ()
    {
        base.Start();

        if (m_checkfuntions == null)
            m_checkfuntions = new Dictionary<State, m_checkfunction>();

        m_checkfuntions.Add(State.IDLE, IdleCheckFunction);
        m_checkfuntions.Add(State.MOVING, MovingCheckFunction);
        m_checkfuntions.Add(State.DODGE, DodgeCheckFunction);
        m_checkfuntions.Add(State.ATTACK, AttackCheckFunction);
        m_checkfuntions.Add(State.SUMMONING, SummoningCheckFunction);
        m_checkfuntions.Add(State.DEAD, DeadCheckFunction);

        Stats temp_stats = new Stats();


        temp_stats.F_speed = 5;
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

        if (Input.GetMouseButton(0))
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
        player_dir = DIRECTION.FRONT;
    }

    private void MovingCheckFunction()
    {
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            player_state = State.IDLE;
        }  

        i_combo = 0;
    }

    private void DodgeCheckFunction()
    {
       
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
    }

    private void SummoningCheckFunction()
    {
       
    }

    private void DeadCheckFunction()
    {
     
    }

    public State GetPlayerState()
    {
        return player_state;
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

        if (Input.GetMouseButton(1))
        {
            player_target_state = TARGET_STATE.AIMING;
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
