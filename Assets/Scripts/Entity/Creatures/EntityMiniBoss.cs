using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMiniBoss : EntityEnemy
{

    GameObject go_player;

    AIThrowPoison ai_throw_poison;

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

        ai_throw_poison  = new AIThrowPoison(2, this, typeof(EntityPlayer), 10, 6);

        RegisterAITask(ai_throw_poison);
        RegisterAITask(new AIAttackMelee(1, this, typeof(EntityPlayer), 3));
        RegisterAITask(new AIChase(1, this, typeof(EntityPlayer), 20, 90));

        GetComponent<Collider>().isTrigger = false;

        St_stats.S_name = "MiniBossBro";
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

    public void OnPoisonAttack()
    {
        //    go_player = GameObject.FindWithTag("Player");
        //    StraightBullet ab_bullet = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.STRAIGHT_PROJECTILE).GetComponent<StraightBullet>();
        //    Vector3 dir = go_player.transform.position - GetComponent<Collider>().bounds.center;
        //    ab_bullet.SetUpProjectile(gameObject, dir, 5, St_stats.F_damage, 20, Vector3.one * 0.25f);

        var bullet = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.ARCH_PROJECTILE).GetComponent<ArcBulllet>();
        bullet.SetUpProjectile(5, St_stats.F_damage, 1, 5, transform.position, ai_throw_poison.v3_poison_target, Vector3.one, gameObject, null);
        bullet.GetComponent<Collider>().isTrigger = true;
    }

    public override void OnAttack()
    {
        SetUpHitBox(gameObject.name, gameObject.tag, gameObject.GetInstanceID().ToString(), St_stats.F_damage, Vector3.one, transform.position + (transform.forward * GetComponent<Collider>().bounds.extents.magnitude), transform.rotation);
    }

    public override void HardReset()
    {
        Start();
    }

    public override void OnAttacked(DamageSource _dmgsrc, float _timer = 0.5f)
    {
        if (!B_isHit)
        {
            S_last_hit = _dmgsrc.GetName();
            St_stats.F_health -= _dmgsrc.GetDamage();
            ResetOnHit(_timer);
            An_animator.SetTrigger("Injured");

        }
    }

}