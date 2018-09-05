using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityLivingBase : Entity
{
    public struct Stats
    {
        float
            f_health,
            f_max_health,
            f_defence,
            f_damage,
            f_speed, 
            f_mass, 
            f_knockback_resistance;

        string
            s_name;
        #region Getter/Setter
        public float F_health
        {
            get
            {
                return f_health;
            }

            set
            {
                f_health = value;
            }
        }

        public float F_max_health
        {
            get
            {
                return f_max_health;
            }

            set
            {
                f_max_health = value;
            }
        }

        public float F_defence
        {
            get
            {
                return f_defence;
            }

            set
            {
                f_defence = value;
            }
        }

        public float F_damage
        {
            get
            {
                return f_damage;
            }

            set
            {
                f_damage = value;
            }
        }

        public float F_speed
        {
            get
            {
                return f_speed;
            }

            set
            {
                f_speed = value;
            }
        }

        public float F_mass
        {
            get
            {
                return f_mass;
            }

            set
            {
                f_mass = value;
            }
        }

        public float F_knockback_resistance
        {
            get
            {
                return f_knockback_resistance;
            }

            set
            {
                f_knockback_resistance = value;
            }
        }

        public string S_name
        {
            get
            {
                return s_name;
            }

            set
            {
                s_name = value;
            }
        }
        #endregion
    }

    private Stats
        st_stats;

    private bool
       b_isHit,
       b_isAttacking,
       b_isDodging;

    private float
        f_hitTimer,
        f_regenTimer,
        f_death_timer;

    private Animator
        an_animator;

    private string
        s_last_hit;

    #region Getter/Setter
    public bool B_isHit
    {
        get
        {
            return b_isHit;
        }

        set
        {
            b_isHit = value;
        }
    }

    public bool B_isAttacking
    {
        get
        {
            return b_isAttacking;
        }

        set
        {
            b_isAttacking = value;
        }
    }

    public bool B_isDodging
    {
        get
        {
            return b_isDodging;
        }

        set
        {
            b_isDodging = value;
        }
    }

    public string S_last_hit
    {
        get
        {
            return s_last_hit;
        }

        set
        {
            s_last_hit = value;
        }
    }

    public Stats St_stats
    {
        get
        {
            return st_stats;
        }

        set
        {
            st_stats = value;
        }
    }

    public float F_hitTimer
    {
        get
        {
            return f_hitTimer;
        }

        set
        {
            f_hitTimer = value;
        }
    }

    public float F_regenTimer
    {
        get
        {
            return f_regenTimer;
        }

        set
        {
            f_regenTimer = value;
        }
    }

    public float F_death_timer
    {
        get
        {
            return f_death_timer;
        }

        set
        {
            f_death_timer = value;
        }
    }

    protected Animator An_animator
    {
        get
        {
            return an_animator;
        }

        set
        {
            an_animator = value;
        }
    }
    #endregion

    protected override void Start ()
    {
        An_animator = GetComponent<Animator>(); 
	}
	
	protected override void Update ()
    {
        F_regenTimer += Time.deltaTime;


        if (!IsDead())
        {
            if (F_regenTimer > 2.0f)
            {
                F_regenTimer = 0.0f;

                st_stats.F_health += 1;
            }
        }

        if (B_isHit)
        {
            F_regenTimer = 0;
            F_hitTimer -= Time.deltaTime;

            if (F_hitTimer <= 0)
                B_isHit = false;
        }
    }

    public Stats GetStats()
    {
        return St_stats;
    }

    public void SetUpStats(Stats _stats)
    {
        St_stats = _stats;
    }

    public bool IsDead()
    {
        if (st_stats.F_health <= 0)
            return true;
        return false;
    }

    public void ResetOnHit(float _timer = 0.5f)
    {
        F_hitTimer = _timer;
        B_isHit = true;
    }

    public virtual void OnAttack() {}

    public virtual void OnAttacked(DamageSource _dmgsrc) {}
}
