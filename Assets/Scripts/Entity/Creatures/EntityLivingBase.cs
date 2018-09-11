using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityLivingBase : Entity
{
    public class Stats
    {
        float
            f_health,
            f_max_health,
            f_mana,
            f_max_mana,
            f_defence,
            f_damage,
            f_speed,
            f_maxspeed,
            f_mass,
            f_knockback_resistance;

        string
            s_name;
        #region Getter/Setter
        public float F_health {
            get {
                return f_health;
            }

            set {
                f_health = value;
            }
        }

        public float F_max_health {
            get {
                return f_max_health;
            }

            set {
                f_max_health = value;
            }
        }

        public float F_defence {
            get {
                return f_defence;
            }

            set {
                f_defence = value;
            }
        }

        public float F_damage {
            get {
                return f_damage;
            }

            set {
                f_damage = value;
            }
        }

        public float F_speed {
            get {
                return f_speed;
            }

            set {
                f_speed = value;
            }
        }

        public float F_mass {
            get {
                return f_mass;
            }

            set {
                f_mass = value;
            }
        }

        public float F_knockback_resistance {
            get {
                return f_knockback_resistance;
            }

            set {
                f_knockback_resistance = value;
            }
        }

        public string S_name {
            get {
                return s_name;
            }

            set {
                s_name = value;
            }
        }

        public float F_maxspeed
        {
            get
            {
                return f_maxspeed;
            }

            set
            {
                f_maxspeed = value;
            }
        }

        public float F_max_mana
        {
            get
            {
                return F_max_mana;
            }

            set
            {
                F_max_mana = value;
            }
        }

        public float F_mana
        {
            get
            {
                return f_mana;
            }

            set
            {
                f_mana = value;
            }
        }
        #endregion
    }

    private Stats
        st_stats = new Stats();

    private bool
       b_isHit,
       b_isAttacking,
       b_isDodging;

    private float
        f_hit_timer,
        f_regen_timer,
        f_regen_amount,
        f_death_timer,
        f_AI_task_change_timer;

    private Animator
        an_animator;

    private string
        s_last_hit;

    private Dictionary<string, SortedDictionary<int, List<AIBase>>>
       dic_AI_list = new Dictionary<string, SortedDictionary<int, List<AIBase>>>();

    private Dictionary<string, AIBase>
       dic_running_AI_list;

    #region Getter/Setter
    public bool B_isHit {
        get {
            return b_isHit;
        }

        set {
            b_isHit = value;
        }
    }

    public bool B_isAttacking {
        get {
            return b_isAttacking;
        }

        set {
            b_isAttacking = value;
        }
    }

    public bool B_isDodging {
        get {
            return b_isDodging;
        }

        set {
            b_isDodging = value;
        }
    }

    public string S_last_hit {
        get {
            return s_last_hit;
        }

        set {
            s_last_hit = value;
        }
    }

    public Stats St_stats {
        get {
            return st_stats;
        }

        set {
            st_stats = value;
        }
    }

    public float F_hit_timer {
        get {
            return f_hit_timer;
        }

        set {
            f_hit_timer = value;
        }
    }

    public float F_regen_timer {
        get {
            return f_regen_timer;
        }

        set {
            f_regen_timer = value;
        }
    }

    public float F_death_timer {
        get {
            return f_death_timer;
        }

        set {
            f_death_timer = value;
        }
    }

    public float F_AI_task_change_timer {
        get {
            return f_AI_task_change_timer;
        }

        set {
            f_AI_task_change_timer = value;
        }
    }

    public Animator An_animator {
        get {
            return an_animator;
        }

        set {
            an_animator = value;
        }
    }

    public float F_regen_amount
    {
        get
        {
            return f_regen_amount;
        }

        set
        {
            f_regen_amount = value;
        }
    }
    #endregion

    private void Awake()
    {
        dic_AI_list = new Dictionary<string, SortedDictionary<int, List<AIBase>>>();
        dic_running_AI_list = new Dictionary<string, AIBase>();
    }

    protected override void Start ()
    {
        base.Start();
        F_death_timer = 0.0f;
        F_AI_task_change_timer = 0.0f;
        F_regen_amount = 0;

        An_animator = GetComponent<Animator>();
        Rb_rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();

        F_AI_task_change_timer += Time.deltaTime;
        F_regen_timer += Time.deltaTime;

        if (dic_AI_list.Count > 0)
        {

            //   if (f_AI_task_change_timer > 1.0f)
            //   {
            foreach (var dic1 in dic_AI_list)
            {
                foreach (var dic2 in dic1.Value)
                {
                    bool done = false;
                    foreach (AIBase ai in dic2.Value)
                    {
                        if (dic_running_AI_list.ContainsKey(ai.GetID()) && dic_running_AI_list[ai.GetID()] != null)
                        {
                            if (!dic_running_AI_list[ai.GetID()].ShouldContinueAI())
                            {
                                dic_running_AI_list[ai.GetID()].EndAI();
                                dic_running_AI_list[ai.GetID()] = null;
                            }

                            if (dic_running_AI_list[ai.GetID()] == null || (dic_running_AI_list[ai.GetID()].GetPriority() > ai.GetPriority() && dic_running_AI_list[ai.GetID()].GetIsInteruptable()))
                            {
                                if (ai.ShouldContinueAI())
                                {
                                    if (dic_running_AI_list[ai.GetID()] != null)
                                        dic_running_AI_list[ai.GetID()].EndAI();

                                    dic_running_AI_list[ai.GetID()] = ai;
                                    dic_running_AI_list[ai.GetID()].StartAI();

                                    done = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (ai.ShouldContinueAI())
                            {
                                if (!dic_running_AI_list.ContainsKey(ai.GetID()))
                                    dic_running_AI_list.Add(ai.GetID(), ai);
                                else
                                    dic_running_AI_list[ai.GetID()] = ai;

                                dic_running_AI_list[ai.GetID()].StartAI();

                                done = true;
                                break;
                            }
                        }
                    }

                    if (done)
                        break;
                }
            }

            f_AI_task_change_timer = 0.0f;
            //  }
        }

        if (dic_running_AI_list.Count > 0)
        {
            // Debug.Log("Running available AI AI");

            foreach (var dic in dic_running_AI_list)
            {
                if (dic.Value != null)
                {
                    dic.Value.RunAI();
                    //Debug.Log("Size of AI Task: " + dic_running_AI_list.Count);
                    //Debug.Log("Running AI of: " + dic.Value.GetID() + " - " + dic.Value.GetDisplayName() +  " With priority: " + dic.Value.GetPriority());
                }
            }
        }


        if (!IsDead())
        {
            if (F_regen_amount > 0)
            {
                if (F_regen_timer > 0.5f)
                {
                    F_regen_timer = 0.0f;

                    F_regen_amount -= 1;
                    st_stats.F_health += 1;
                }
            }
        }

        if (B_isHit)
        {
            F_regen_timer = 0;
            F_hit_timer -= Time.deltaTime;

            if (F_hit_timer <= 0)
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
        F_hit_timer = _timer;
        B_isHit = true;
    }

    public void RegisterAITask(AIBase _ai)
    {
        if (dic_AI_list == null)
            dic_AI_list = new Dictionary<string, SortedDictionary<int, List<AIBase>>>();

        if (!dic_AI_list.ContainsKey(_ai.GetID()))
        {
            dic_AI_list.Add(_ai.GetID(), new SortedDictionary<int, List<AIBase>>());
        }

        if (!dic_AI_list[_ai.GetID()].ContainsKey(_ai.GetPriority()))
        {
            dic_AI_list[_ai.GetID()].Add(_ai.GetPriority(), new List<AIBase>());
        }

        dic_AI_list[_ai.GetID()][_ai.GetPriority()].Add(_ai);
    }

    public void EndAndClearAITask()
    {
        foreach (var dic1 in dic_AI_list)
        {
            foreach (var dic2 in dic1.Value)
            {
                foreach (AIBase ai in dic2.Value)
                {
                    if (dic_running_AI_list.ContainsKey(ai.GetID()) && dic_running_AI_list[ai.GetID()] != null)
                    {
                        dic_running_AI_list[ai.GetID()].EndAI();
                    }
                }
            }
        }

        dic_AI_list.Clear();
    }

    public void ClearAITask()
    {
        dic_AI_list.Clear();
    }

    public virtual void OnAttack() { }

    public virtual void OnAttacked(DamageSource _dmgsrc, float _timer = 0.3f) { }
}
