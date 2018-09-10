using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBoss : EntityEnemy {

    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected override void Update()
    {
        if (!IsDead())
            base.Update();
        else
        {
            F_death_timer += Time.deltaTime;
            //An_animator.SetBool("DeadTrigger", true);

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

    public override void OnAttacked(DamageSource _dmgsrc)
    {

    }

    void Init()
    {
        ClearAITask();

        St_stats.S_name = "Perstilence";
        St_stats.F_max_health = 50.0f;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_damage = 5.0f;
        St_stats.F_defence = 5.0f;
        St_stats.F_speed = 5.0f;
        St_stats.F_mass = 5.0f;
        St_stats.F_knockback_resistance = 5.0f;

        B_isAttacking = false;
        B_isDodging = false;
        B_isHit = false;

        //TODO: Set Animation Values


        //TODO: Register AI Task
        //RegisterAITask(new AIArtyState(3, this, typeof(EntityPlayer), 20, 12, 3));
        RegisterAITask(new AIAOEAttack(1, this, typeof(EntityPlayer), 20, 5));
        //RegisterAITask(new AIRoam(3, this, 5.0f));
        //RegisterAITask(new AIChase(2, this, typeof(EntityPlayer), 20.0f, 9999));
        //RegisterAITask(new AIAttackMelee(1, this, typeof(EntityPlayer), 2.0f));
    }
}
