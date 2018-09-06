using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMelee : EntityEnemy
{

    private void Awake()
    {
        ClearAITask();

        St_stats.F_max_health = 50;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_speed = 0.5f;
        St_stats.F_defence = 20.0f;
        St_stats.F_damage = 2.0f;
        St_stats.F_mass = 2.0f;

        B_isHit = false;

        // RegisterAITask(new AIIdle(2, this));
        RegisterAITask(new AIAttackMelee(1, this, typeof(EntityPlayer), 2.0f));
        RegisterAITask(new AIRoam(3, this, 5.0f));

        //if (an_animator = GetComponentInChildren<Animator>())
        //{
        //}
        //else
        //{
        //    Debug.LogError("ERROR: There is no animator for character.");
        //    Destroy(this);
        //}

        St_stats.S_name = "MeleeDude";
    }

    protected override void Update()
    {
        if (!IsDead())
            base.Update();
        else
        {
            F_death_timer += Time.deltaTime;
            an_animator.SetBool("DeadTrigger", true);

            if (F_death_timer > 5.0f)
            {
                gameObject.SetActive(false);

                GameObject go = ObjectPool.GetInstance().GetItemObjectFromPool();
                go.GetComponent<EntityPickUps>().SetPosition(GetPosition());
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
            St_stats.F_damage
            );

        obj_hitbox.SetHitbox(dmgsrc, new Vector3(1.5f, 1, 1.5f));

        obj_hitbox.transform.position = transform.position + (transform.forward * (obj_hitbox.transform.localScale * 0.8f).z);
        obj_hitbox.transform.position = new Vector3(obj_hitbox.transform.position.x, obj_hitbox.transform.position.y + 1, obj_hitbox.transform.position.z);

        obj_hitbox.transform.rotation = transform.rotation;
    }

    public override void HardReset()
    {
        St_stats.F_max_health = 50;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_speed = 0.5f;
        St_stats.F_defence = 20.0f;
        St_stats.F_damage = 2.0f;
        St_stats.F_mass = 2.0f;

        B_isHit = false;

        // RegisterAITask(new AIIdle(2, this));
        RegisterAITask(new AIAttackMelee(1, this, typeof(EntityPlayer), 2.0f));
        RegisterAITask(new AIRoam(3, this, 5.0f));

        if (an_animator = GetComponentInChildren<Animator>())
        {
        }
        else
        {
            Debug.LogError("ERROR: There is no animator for character.");
            Destroy(this);
        }

        St_stats.S_name = "MeleeDude";
    }
    
}