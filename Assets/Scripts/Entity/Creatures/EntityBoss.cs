using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBoss : EntityEnemy {

    public enum BossState
    {
        FULLHEALTH,
        HALFHEALTH,
        QUARTERHEATLH
    };

    public enum AttackState
    {
        NONE,
        GRAVITY,
        LASER,
        ARTY,
        SPINATTACK
    };

    AttackState currentAttState;

    float
        dissolveRate;

    Material
        bossMat;

    #region Getter/Setter
    public AttackState As_currentAttState
    {
        get
        {
            return currentAttState;
        }

        set
        {
            currentAttState = value;
        }
    }
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!IsDead())
            base.Update();
        else
        {
            F_death_timer += Time.deltaTime;
            An_animator.SetBool("Dead", true);
            dissolveRate += Time.deltaTime * 0.25f;
            bossMat.SetFloat("_DissolveAmount", dissolveRate);

            if (F_death_timer > 5.0f)
            {
                gameObject.SetActive(false);

                //GameObject go = ObjectPool.GetInstance().GetItemObjectFromPool();
                //go.GetComponent<EntityPickUp>().SetPosition(GetPosition());

                //TODO: Spawn Soul For Player TO Collect
            }
        }
    }

    public override void OnAttack()
    {
        GameObject obj = ObjectPool.GetInstance().GetHitboxObjectFromPool();
        HitboxTrigger obj_hitbox = obj.GetComponent<HitboxTrigger>();

        DamageSource dmgsrc = new DamageSource();

        dmgsrc.SetUpDamageSource(St_stats.S_name + " " + gameObject.GetInstanceID().ToString(),
            gameObject.tag,
            gameObject.GetInstanceID().ToString(),
            St_stats.F_damage);

        obj_hitbox.SetHitbox(dmgsrc, new Vector3(1.5f, 1, 1.5f));

        obj_hitbox.transform.position = transform.position + (transform.forward * (obj_hitbox.transform.localScale * 0.8f).z);
        obj_hitbox.transform.position = new Vector3(obj_hitbox.transform.position.x, obj_hitbox.transform.position.y + 1, obj_hitbox.transform.position.z);

        obj_hitbox.transform.rotation = transform.rotation;
    }

    public override void OnAttacked(DamageSource _dmgsrc, float _timer = 0.5f)
    {
        if (!B_isHit)
        {
            S_last_hit = _dmgsrc.GetName();

            if (TagHelper.IsTagCritSpot(_dmgsrc.GetAttackedTag()) && B_isVulnerable)
                St_stats.F_health -= _dmgsrc.GetDamage() * 2; //YEET
            else
                St_stats.F_health -= _dmgsrc.GetDamage();

            Debug.Log("BOSS HP = " + St_stats.F_health);

            ResetOnHit(_timer);
        }
    }

    public override void HardReset()
    {
        base.HardReset();
        ClearAITask();

        St_stats.S_name = "Perstilence";
        St_stats.F_max_health = 5.0f;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_damage = 5.0f;
        St_stats.F_defence = 5.0f;
        St_stats.F_speed = 5.0f;
        St_stats.F_mass = 5.0f;
        St_stats.F_knockback_resistance = 5.0f;

        B_isAttacking = false;
        B_isDodging = false;
        B_isHit = false;

        dissolveRate = 0f;

        var enemiesToSpawn = new List<ObjectPool.ENEMY> { ObjectPool.ENEMY.ENEMY_MINIBOSS };

        bossMat = gameObject.GetComponentInChildren<Renderer>().material;

        //TODO: Set Animation Values


        //TODO: Register AI Task
        //RegisterAITask(new AIArtyState(2, this, typeof(EntityPlayer), 20, 12, 10, 3));
        //RegisterAITask(new AIBossLaser(1, this, typeof(EntityPlayer), 50, 5, 10));
        //RegisterAITask(new AIAOEAttack(3, this, typeof(EntityPlayer), 20, 15,12));
        //RegisterAITask(new AISpawnMobs(0, this, typeof(EntityPlayer), 10, 20, 3.0f, enemiesToSpawn));
        RegisterAITask(new AIChase(2, this, typeof(EntityPlayer), 20.0f, 9999));
        //RegisterAITask(new AIAttackMelee(1, this, typeof(EntityPlayer), 2.0f));
    }

    public void StartAnimAiming()
    {
        An_animator.SetBool("Aiming", true);
    }

    public void EndAnimAiming()
    {
        An_animator.SetBool("Aiming", false);
    }

    public void SetAnimShooting(int _value)
    {
        An_animator.SetBool("Shooting", (_value == 1));
    }
}
