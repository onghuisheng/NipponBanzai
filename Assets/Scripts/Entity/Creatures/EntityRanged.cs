using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRanged : EntityEnemy
{

    GameObject go_player;

    [SerializeField]
    Transform tf_beak;

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

                foreach (var type in GetInventory().GetInventoryContainer())
                {
                    for (int i = 0; i < type.Value; ++i)
                    {
                        ObjectPool.GetInstance().GetItemObjectFromPool().GetComponent<EntityPickUps>().SetUpPickUp(new Vector3(GetPosition().x + Random.Range(-1.0f, 1.0f), GetPosition().y + 0.5f, GetPosition().z + Random.Range(-1.0f, 1.0f)), 30, ItemHandler.GetItem(type.Key));
                    }
                }

                for (int i = 0; i < GetInventory().GetSouls(); ++i)
                {
                    ObjectPool.GetInstance().GetItemObjectFromPool().GetComponent<EntityPickUps>().SetUpPickUp(new Vector3(GetPosition().x + Random.Range(-1.0f, 1.0f), GetPosition().y + Random.Range(0.5f, 1.0f), GetPosition().z + Random.Range(-1.0f, 1.0f)), 30, ItemHandler.GetItem(Item.ITEM_TYPE.SOULS));
                }
            }
        }
    }

    public override void OnAttack()
    {
        if (go_player == null)
            go_player = GameObject.FindWithTag("Player");

        StraightBullet ab_bullet = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.STRAIGHT_PROJECTILE).GetComponent<StraightBullet>();
        Vector3 dir = go_player.GetComponent<Collider>().bounds.center - tf_beak.position;
        ab_bullet.SetUpProjectile(gameObject, tf_beak.position, dir, 5, St_stats.F_damage, 20, Vector3.one * 0.25f);

        ap_audioPlayer.PlayClip("EnemyRangedFire_" + Random.Range(1, 3));
    }

    public override void HardReset()
    {
        base.HardReset();

        ClearAITask();

        St_stats.F_max_health = 50;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_speed = 3;
        St_stats.F_defence = 20.0f;
        St_stats.F_damage = 5;
        St_stats.F_mass = 2.0f;

        B_isHit = false;

        An_animator.Rebind();
        RegisterAITask(new AIPanic(-1, this, typeof(EntityPlayer)));
        RegisterAITask(new AIAttackRanged(2, this, typeof(EntityPlayer), 15));
        RegisterAITask(new AIChase(1, this, typeof(EntityPlayer), 50, 150));

        GetComponent<Collider>().isTrigger = false;

        SetDrops(Item.ITEM_TYPE.HEALTH_POTION, 1, 70);
        SetDrops(Item.ITEM_TYPE.MANA_POTION, 1, 70);
        GetInventory().AdjustSoulsAmount(Random.Range(0, 10));

        St_stats.S_name = "Minion2";

        DoSpawnAnimation(ParticleHandler.ParticleType.SummoningPortal);
    }

    public override void OnAttacked(DamageSource _dmgsrc, float _timer = 0.5f)
    {
        if (!B_isHit)
        {
            S_last_hit = _dmgsrc.GetName();
            St_stats.F_health -= _dmgsrc.GetDamage();
            ResetOnHit(_timer);

            if (!An_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && _dmgsrc.IsFlinching())
                An_animator.SetTrigger("Injured");

            if (_dmgsrc.GetSourceID() == "Melee")
                AudioHandler.GetInstance().PlayClip("EnemyImpact_" + Random.Range(1, 4));
        }
    }

}