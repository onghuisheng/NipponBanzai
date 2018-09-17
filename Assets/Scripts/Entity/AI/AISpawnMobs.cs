using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnMobs : AIBase
{

    private float
        f_spawnRadius,
        f_cooldown,
        f_nextCooldownTime,
        f_summonAnimationDelay,
        f_attack_range;

    private bool
        b_IsSummoning;

    private System.Type
        type_target;

    List<ObjectPool.ENEMY> list_enemiesToSpawn;

    public AISpawnMobs(int _priority, EntityLivingBase _entity, System.Type _type, float _spawnRadius, float _cooldown, float _summonAnimationDelay, List<ObjectPool.ENEMY> _enemiesToSpawn, float _attackRange = 0)
    {
        i_priority = _priority;
        ent_main = _entity;
        s_ID = "Combat";
        s_display_name = "Spawn Mobs";
        b_is_interruptable = false;
        f_spawnRadius = _spawnRadius;
        b_IsSummoning = false;
        type_target = _type;
        f_cooldown = _cooldown;
        f_summonAnimationDelay = _summonAnimationDelay;
        f_attack_range = _attackRange;
        list_enemiesToSpawn = _enemiesToSpawn;
    }

    public override bool StartAI()
    {
        b_IsSummoning = false;
        f_nextCooldownTime = 0;
        return true;
    }

    public override bool ShouldContinueAI()
    {
        if (ent_target == null)
        {
            ent_target = GameObject.FindWithTag("Player").GetComponent<EntityPlayer>();
        }

        if (Time.time < f_nextCooldownTime && !b_IsSummoning || ent_main.An_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            return false;
        }
        else
        {
            // If we do not have a range to check, just spawn mobs immediately
            if (f_attack_range == 0)
                return true;

            int ignoreEnemiesMask = ~(1 << LayerMask.NameToLayer("Enemies"));

            RaycastHit hitInfo;

            // Cast a ray towards the player while ignoring all objects in the Enemies layer
            Vector3 enemyCenter = ent_main.GetComponent<Collider>().bounds.center;
            Vector3 playerCenter = ent_target.GetComponent<Collider>().bounds.center;
            if (Physics.Raycast(enemyCenter, playerCenter - enemyCenter, out hitInfo, f_attack_range, ignoreEnemiesMask))
            {
                if (hitInfo.collider.tag == "Player")
                    return true;
            }
        }

        return false;
    }


    public override bool RunAI()
    {
        if (b_IsSummoning)
            return true;

        b_IsSummoning = true;
        ent_main.B_isAttacking = true;

        ent_main.An_animator.SetTrigger("Summoning");

        ent_main.StartCoroutine(SpawnEnemy());

        f_nextCooldownTime = Time.time + f_cooldown;

        return true;
    }


    public override bool EndAI()
    {
        ent_main.B_isAttacking = false;
        return true;
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(f_summonAnimationDelay);

        foreach (var enemyType in list_enemiesToSpawn)
        {
            EntityEnemy enemy = ObjectPool.GetInstance().GetEntityObjectFromPool(enemyType).GetComponent<EntityEnemy>();
            enemy.transform.position = RandomInsideBox(enemy, 60);
        }
        b_IsSummoning = false;
    }

    Vector3 RandomInsideBox(EntityLivingBase enemy, int maxAttempt)
    {
        Vector3 bossCenter = ent_main.GetPosition();

        Vector3 spawnPos = new Vector3(
            Random.Range(bossCenter.x - f_spawnRadius, bossCenter.x + f_spawnRadius),
            ent_main.transform.position.y,
            Random.Range(bossCenter.z - f_spawnRadius, bossCenter.z + f_spawnRadius)
            ); // A random point around ent_main

        // Stop looping if we hit max attempts to prevent stackoverflow
        if (maxAttempt <= 0)
        {
            Debug.LogWarning("Can't find empty space to spawn");
            return spawnPos;
        }

        if (CheckForCollision(enemy, spawnPos)) // Check if we'll collide with any enemies that are already spawned in this trigger
        {
            return RandomInsideBox(enemy, --maxAttempt); // recurse until we get a valid position
        }
        else
        {
            return spawnPos;
        }

    }

    // Checks if we're going to collide with anything while spawning
    bool CheckForCollision(EntityLivingBase enemy, Vector3 pos)
    {
        Bounds bound = enemy.GetComponent<Collider>().bounds;
        enemy.GetComponent<Collider>().enabled = false;
        bool blocked = Physics.CheckBox(pos + bound.center, bound.extents, enemy.transform.rotation, (1 << LayerMask.NameToLayer("Enemies")));
        enemy.GetComponent<Collider>().enabled = true;
        return blocked;
    }

}
