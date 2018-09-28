using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EntityMiniBoss : EntityEnemy
{

    GameObject go_player;

    AIThrowPoison ai_throw_poison;

    [SerializeField]
    Transform tf_poison_start_position;

    protected override void Start()
    {
        base.Start();
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

                foreach (var type in GetInventory().GetInventoryContainer())
                {
                    for (int i = 0; i < type.Value; ++i)
                    {
                        ObjectPool.GetInstance().GetItemObjectFromPool().GetComponent<EntityPickUps>().SetUpPickUp(new Vector3(GetPosition().x + Random.Range(-1.0f, 1.0f), GetPosition().y + 0.5f, GetPosition().z + Random.Range(-1.0f, 1.0f)), 30, ItemHandler.GetItem(type.Key));
                    }
                }
            }
        }
    }

    public void OnPoisonAttack()
    {
        var bullet = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.ARCH_PROJECTILE).GetComponent<ArcBulllet>();
        bullet.GetComponent<Renderer>().enabled = false;
        GameObject bulletTrail = ParticleHandler.GetInstance().SpawnParticle(ParticleHandler.ParticleType.PoisonMouthDrip, bullet.transform, Vector3.zero, Vector3.one * 2, Vector3.zero, 0);

        Vector3 bulletSpawnPos = tf_poison_start_position.position;

        bullet.SetUpProjectile(5, St_stats.F_damage, 1.5f, 1.5f, bulletSpawnPos, ai_throw_poison.v3_poison_target, Vector3.one * 0.25f, gameObject, (collider) =>
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                EntityPoisonPool pool = ObjectPool.GetInstance().GetEnviromentObjectFromPool(ObjectPool.ENVIRONMENT.POISON_POOL).GetComponent<EntityPoisonPool>();
                pool.SetUpPoisonPoolWLifeTime(gameObject, St_stats.F_damage / 2, 10, bullet.GetPosition());
                Destroy(bulletTrail);
                bullet.gameObject.SetActive(false);
            }
        }, () =>
        {
            EntityPoisonPool pool = ObjectPool.GetInstance().GetEnviromentObjectFromPool(ObjectPool.ENVIRONMENT.POISON_POOL).GetComponent<EntityPoisonPool>();
            pool.SetUpPoisonPoolWLifeTime(gameObject, St_stats.F_damage / 2, 10, bullet.GetPosition());
            Destroy(bulletTrail);
            bullet.gameObject.SetActive(false);
        });

        ap_audioPlayer.PlayClip("MiniBossPoison");
    }

    public override void OnAttack()
    {
        SetUpHitBox(gameObject.name, gameObject.tag, gameObject.GetInstanceID().ToString(), St_stats.F_damage, Vector3.one, transform.position + (transform.forward * GetComponent<Collider>().bounds.extents.magnitude), transform.rotation);
        ap_audioPlayer.PlayClip("MiniBossMelee");
    }

    public override void HardReset()
    {
        base.HardReset();

        ClearAITask();

        St_stats.F_max_health = 50;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_speed = 3;
        St_stats.F_defence = 20.0f;
        St_stats.F_damage = 30;
        St_stats.F_mass = 2.0f;

        B_isHit = false;

        An_animator.Rebind();

        var enemiesToSpawn = new List<ObjectPool.ENEMY> { ObjectPool.ENEMY.ENEMY_MELEE, ObjectPool.ENEMY.ENEMY_MELEE, };

        RegisterAITask(new AIPanic(-1, this, typeof(EntityPlayer)));
        RegisterAITask(new AISpawnMobs(0, this, typeof(EntityPlayer), 10, 10, 2.6f, enemiesToSpawn, 10));
        RegisterAITask(ai_throw_poison = new AIThrowPoison(1, this, typeof(EntityPlayer), 15, 6, () =>
        {
            // Spawn poison particle on mouth
            ParticleHandler.GetInstance().SpawnParticle(ParticleHandler.ParticleType.PoisonMouthDrip, tf_poison_start_position, Vector3.zero, Vector3.one, -tf_poison_start_position.rotation.eulerAngles, 2);
        }));
        RegisterAITask(new AIAttackMelee(2, this, typeof(EntityPlayer), GetComponent<NavMeshAgent>().stoppingDistance));
        RegisterAITask(new AIChase(1, this, typeof(EntityPlayer), 50, 150));

        GetComponent<Collider>().isTrigger = false;

        SetDrops(Item.ITEM_TYPE.HEALTH_POTION, Random.Range(1, 2), 70);
        SetDrops(Item.ITEM_TYPE.MANA_POTION, Random.Range(1, 2), 70);
        SetDrops(Item.ITEM_TYPE.SOULS, Random.Range(0, 20), 70);

        St_stats.S_name = "MiniBoss";

        DoSpawnAnimation(ParticleHandler.ParticleType.SummoningPortal);
    }

    public void PlayCrowNoise()
    {
        ap_audioPlayer.PlayClip("MiniBossSpawnMobs");
    }

    public override void OnAttacked(DamageSource _dmgsrc, float _timer = 0.5f)
    {
        if (!B_isHit)
        {
            S_last_hit = _dmgsrc.GetName();
            St_stats.F_health -= _dmgsrc.GetDamage();
            ResetOnHit(_timer);

            if (!An_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !An_animator.GetCurrentAnimatorStateInfo(0).IsTag("Summon") && _dmgsrc.IsFlinching())
                An_animator.SetTrigger("Injured");

            if (_dmgsrc.GetSourceID() == "Melee")
                AudioHandler.GetInstance().PlayClip("EnemyImpact_" + Random.Range(1, 4));
        }
    }

}