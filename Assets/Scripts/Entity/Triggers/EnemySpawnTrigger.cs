using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : EntityTrigger
{

    enum SpawningType
    {
        OnTriggerEnter,
        OnTriggerEnterEndless,
        OnStart
    }

    [SerializeField]
    private int i_melee_spawn_count = 0, i_ranged_spawn_count = 0, i_miniboss_spawn_count = 0;

    [SerializeField]
    private SpawningType m_spawning_type = SpawningType.OnTriggerEnter;

    [SerializeField]
    private bool b_randomize_spawn_interval = true;

    [SerializeField]
    [Tooltip("Random interval between spawning of each enemies")]
    private Vector2 v2_spawn_interval_min_max = new Vector2(0, 0);

    bool b_is_spawned = false;

    List<EntityLivingBase> lst_enemy_collection = new List<EntityLivingBase>();
    
    protected override void Start()
    {
        if (m_spawning_type == SpawningType.OnStart)
            SpawnEnemies();
    }

    protected override void Update()
    {
        // Endless wave respawn check
        if (b_is_spawned && m_spawning_type == SpawningType.OnTriggerEnterEndless)
        {
            bool allDead = true;
            foreach (var enemy in lst_enemy_collection)
            {
                if (!enemy.IsDead() && enemy.gameObject.activeSelf)
                {
                    allDead = false;
                    break;
                }
            }

            // Respawn if all enemy died
            if (allDead)
            {
                lst_enemy_collection.Clear();
                StartCoroutine(StartSpawning());
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Q))
        {
            foreach (var enemy in lst_enemy_collection)
            {
                DamageSource dmgSrc = new DamageSource();
                dmgSrc.SetUpDamageSource("God", "God", "God", 10);
                enemy.OnAttacked(dmgSrc);
            }
        }
#endif

    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !b_is_spawned)
        {
            switch (m_spawning_type)
            {
                case SpawningType.OnTriggerEnter:
                    SpawnEnemies();
                    break;
                case SpawningType.OnTriggerEnterEndless:
                    SpawnEnemies();
                    // Tooooodoooo: some ui indication that this is endless wave?
                    break;
            }
        }
    }

    public void SpawnEnemies()
    {
        b_is_spawned = true;
        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        for (int i = 0; i < i_melee_spawn_count; i++)
        {
            var enemy = ObjectPool.GetInstance().GetEntityObjectFromPool(ObjectPool.ENEMY.ENEMY_MELEE).GetComponent<EntityLivingBase>();
            enemy.SetPosition(RandomInsideBox(enemy));
            lst_enemy_collection.Add(enemy);

            yield return new WaitForSeconds((b_randomize_spawn_interval) ? Random.Range(v2_spawn_interval_min_max.x, v2_spawn_interval_min_max.y) : 0);
        }
        for (int i = 0; i < i_ranged_spawn_count; i++)
        {
            var enemy = ObjectPool.GetInstance().GetEntityObjectFromPool(ObjectPool.ENEMY.ENEMY_RANGED).GetComponent<EntityLivingBase>();
            enemy.SetPosition(RandomInsideBox(enemy));
            lst_enemy_collection.Add(enemy);

            yield return new WaitForSeconds((b_randomize_spawn_interval) ? Random.Range(v2_spawn_interval_min_max.x, v2_spawn_interval_min_max.y) : 0);
        }
        for (int i = 0; i < i_miniboss_spawn_count; i++)
        {
            var enemy = ObjectPool.GetInstance().GetEntityObjectFromPool(ObjectPool.ENEMY.ENEMY_MINIBOSS).GetComponent<EntityLivingBase>();
            enemy.SetPosition(RandomInsideBox(enemy));
            lst_enemy_collection.Add(enemy);

            yield return new WaitForSeconds((b_randomize_spawn_interval) ? Random.Range(v2_spawn_interval_min_max.x, v2_spawn_interval_min_max.y) : 0);
        }
    }

    Vector3 RandomInsideBox(EntityLivingBase enemy)
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 value = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            transform.position.y,
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

        if (CheckForCollision(enemy, value))
        {
            return RandomInsideBox(enemy);
        }
        else
        {
            return value;
        }
    }

    // Checks if we're going to collide with anything while spawning
    bool CheckForCollision(EntityLivingBase enemy, Vector3 destination)
    {
        foreach (var cat in lst_enemy_collection)
        {
            Bounds enemyColliderBounds = enemy.GetComponent<Collider>().bounds;
            enemyColliderBounds.center = destination;
            if (cat.GetComponent<Collider>().bounds.Intersects(enemyColliderBounds))
            {
                return true;
            }
        }

        return false;
    }

}
