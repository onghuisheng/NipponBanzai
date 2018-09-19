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
        Random.InitState((int)Time.realtimeSinceStartup);

        if (m_spawning_type == SpawningType.OnStart)
            SpawnEnemies();
    }

    protected override void Update()
    {
        base.Update();

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


        if (Input.GetKeyUp(KeyCode.G))
        {

            foreach (var enemy in lst_enemy_collection)
            {
                var gg = new StatusPoison(5, 1, 0.25f);
                enemy.Stc_Status.ApplyStatus(gg);
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
            yield return SpawnAndWait(ObjectPool.GetInstance().GetEntityObjectFromPool(ObjectPool.ENEMY.ENEMY_MELEE).GetComponent<EntityLivingBase>());
        }
        for (int i = 0; i < i_ranged_spawn_count; i++)
        {
            yield return SpawnAndWait(ObjectPool.GetInstance().GetEntityObjectFromPool(ObjectPool.ENEMY.ENEMY_RANGED).GetComponent<EntityLivingBase>());
        }
        for (int i = 0; i < i_miniboss_spawn_count; i++)
        {
            yield return SpawnAndWait(ObjectPool.GetInstance().GetEntityObjectFromPool(ObjectPool.ENEMY.ENEMY_MINIBOSS).GetComponent<EntityLivingBase>());
        }
    }

    // Helper function for StartSpawning coroutine
    WaitForSeconds SpawnAndWait(EntityLivingBase enemy)
    {
        Vector3 spawnPos = RandomInsideBox(enemy, 60);
        enemy.SetPosition(spawnPos);

        // Adjust the Y position to align with the ground
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPos, Vector3.down, out hitInfo, Mathf.Infinity, (1 << LayerMask.NameToLayer("Environment"))))
        { // Only check for environment
            spawnPos.y = hitInfo.point.y;
        }

        enemy.SetPosition(spawnPos);

        lst_enemy_collection.Add(enemy);

        return new WaitForSeconds((b_randomize_spawn_interval) ? Random.Range(v2_spawn_interval_min_max.x, v2_spawn_interval_min_max.y) : 0);
    }

    // Returns a position where spawning an object would not collide with other enemies
    Vector3 RandomInsideBox(EntityLivingBase enemy, int maxAttempt)
    {
        BoxCollider collider = GetComponent<BoxCollider>();

        Vector3 spawnPos = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            transform.position.y,
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            ); // A random point in this trigger zone

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

        //Bounds bound = enemy.GetComponent<Collider>().bounds;
        //enemy.GetComponent<Collider>().enabled = false;
        //bool blocked = Physics.CheckBox(pos + bound.center, bound.extents, enemy.transform.rotation, (1 << LayerMask.NameToLayer("Enemies")));
        //enemy.GetComponent<Collider>().enabled = true;

        //Debug.Log(blocked);

        //return blocked;

        foreach (var it_enemy in lst_enemy_collection)
        {
            Bounds enemyColliderBounds = enemy.GetComponent<Collider>().bounds;
            enemyColliderBounds.center = pos;
            if (it_enemy.GetComponent<Collider>().bounds.Intersects(enemyColliderBounds))
            {
                return true;
            }
        }

        return false;
    }

}
