using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityMelee : EntityEnemy
{

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
            }
        }
    }

    public override void OnAttack()
    {
        SetUpHitBox(gameObject.name, gameObject.tag, gameObject.GetInstanceID().ToString(), St_stats.F_damage, Vector3.one, transform.position + (transform.forward * GetComponent<Collider>().bounds.extents.magnitude), transform.rotation);

        ap_audioPlayer.PlayClip("EnemyMeleeHit_" + Random.Range(1, 5));

    }

    public override void HardReset()
    {
        base.HardReset();

        ClearAITask();

        St_stats.F_max_health = 50;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_speed = 3;
        St_stats.F_defence = 20.0f;
        St_stats.F_damage = 15;
        St_stats.F_mass = 2.0f;

        B_isHit = false;

        An_animator.Rebind();
        RegisterAITask(new AIPanic(-1, this, typeof(EntityPlayer)));
        RegisterAITask(new AIAttackMelee(2, this, typeof(EntityPlayer), GetComponent<NavMeshAgent>().stoppingDistance));
        RegisterAITask(new AIChase(1, this, typeof(EntityPlayer), 50, 150));
        //RegisterAITask(new AIRoam(3, this, 5.0f));

        GetComponent<Collider>().isTrigger = false;

        SetDrops(Item.ITEM_TYPE.HEALTH_POTION, 1, 70);
        SetDrops(Item.ITEM_TYPE.MANA_POTION, 1, 70);
        SetDrops(Item.ITEM_TYPE.SOULS, Random.Range(0, 20), 70);

        St_stats.S_name = "Minion1";

        DoSpawnAnimation(ParticleHandler.ParticleType.SummoningPortal);
    }

    public override void OnAttacked(DamageSource _dmgsrc, float _timer = 0.5f)
    {
        if (!B_isHit)
        {
            S_last_hit = _dmgsrc.GetName();
            St_stats.F_health -= _dmgsrc.GetDamage();
            ResetOnHit(_timer);
            KnockBack(_dmgsrc);

            if (!An_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && _dmgsrc.IsFlinching())
                An_animator.SetTrigger("Injured");

            if (_dmgsrc.GetSourceID() == "Melee")
                AudioHandler.GetInstance().PlayClip("EnemyImpact_" + Random.Range(1, 4));
        }
    }
}