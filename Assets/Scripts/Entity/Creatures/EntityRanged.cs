using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRanged : EntityEnemy
{

    protected override void Start()
    {
        base.Start();

        ClearAITask();

        St_stats.F_max_health = 50;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_speed = 3;
        St_stats.F_defence = 20.0f;
        St_stats.F_damage = 2.0f;
        St_stats.F_mass = 2.0f;

        B_isHit = false;

        An_animator.Rebind();
        RegisterAITask(new AIAttackRanged(1, this, typeof(EntityPlayer), 10));
        RegisterAITask(new AIChase(1, this, typeof(EntityPlayer), 20, 60));

        GetComponent<Collider>().isTrigger = false;

        St_stats.S_name = "RangedDude";
    }

    protected override void Update()
    {
        if (!IsDead())
        {
            base.Update();
        }
        else
        {
            F_death_timer += Time.deltaTime;

            if (!An_animator.GetBool("Dead"))
            {
                EndAndClearAITask();
                An_animator.SetBool("Dead", true);
                GetComponent<Collider>().isTrigger = true;
            }

            if (F_death_timer > 5.0f)
            {
                gameObject.SetActive(false);

                // GameObject go = ObjectPool.GetInstance().GetItemObjectFromPool();
                // go.GetComponent<EntityPickUps>().SetPosition(GetPosition());
            }
        }
    }

    public override void OnAttack()
    {
        //GameObject obj = ObjectPool.GetInstance().GetHitboxObjectFromPool();
        //HitboxTrigger obj_hitbox = obj.GetComponent<HitboxTrigger>();

        //DamageSource dmgsrc = new DamageSource();

        //dmgsrc.SetUpDamageSource(St_stats.S_name + " " + gameObject.GetInstanceID().ToString(),
        //    gameObject.tag,
        //    gameObject.GetInstanceID().ToString(),
        //    St_stats.F_damage
        //    );

        //obj_hitbox.SetHitbox(dmgsrc, new Vector3(1.5f, 1, 1.5f));

        //obj_hitbox.transform.position = transform.position + (transform.forward * (obj_hitbox.transform.localScale * 0.8f).z);
        //obj_hitbox.transform.position = new Vector3(obj_hitbox.transform.position.x, obj_hitbox.transform.position.y + 1, obj_hitbox.transform.position.z);

        //obj_hitbox.transform.rotation = transform.rotation;

        ArcBulllet ab_bullet = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.ARCH_PROJECTILE).GetComponent<ArcBulllet>();
        ab_bullet.SetUpProjectile(5, 20, 1, 10, transform.position, GameObject.FindWithTag("Player").transform.position, new Vector3(2, 2, 2), null);
    }

    public override void HardReset()
    {
        Start();
    }

    public override void OnAttacked(DamageSource _dmgsrc)
    {
        if (St_stats.F_health <= 0)
            return;

        base.OnAttacked(_dmgsrc);
        St_stats.F_health -= _dmgsrc.GetDamage();
        An_animator.SetTrigger("Injured");
    }

}