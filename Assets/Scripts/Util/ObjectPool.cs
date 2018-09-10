using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{

    public enum ENEMY
    {
        ENEMY_MELEE = 0,
        ENEMY_RANGED,
        ENEMY_MINIBOSS
    }

    public enum PROJECTILE
    {
        TARGET_ZONE = 0,
        ARCH_PROJECTILE,
        STRAIGHT_PROJECTILE
    }

    public enum ENVIRONMENT
    {
        CRYSTAL = 0
    }

    public GameObject[] entity_list;
    public GameObject[] projectile_list;
    public GameObject[] enviroment_list;

    public GameObject hitbox;
    public GameObject item_pickup;
    public GameObject go_player;

    private List<GameObject> entity_pool_list = new List<GameObject>();
    private List<GameObject> projectile_pool_list = new List<GameObject>();
    private List<GameObject> hitbox_pool_list = new List<GameObject>();
    private List<GameObject> item_pool_list = new List<GameObject>();
    private List<GameObject> enviroment_pool_list = new List<GameObject>();
    private GameObject go_player_instance;

    //SETTING UP THE SPAWNER
    protected override void Awake()
    {
        base.Awake();

        for (int entity_list_count = 0; entity_list_count < entity_list.Length; ++entity_list_count)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject obj = Instantiate(entity_list[entity_list_count]);
                obj.SetActive(false);
                entity_pool_list.Add(obj);

                obj.transform.parent = gameObject.transform;
            }
        }

        for (int projectile_list_count = 0; projectile_list_count < projectile_list.Length; ++projectile_list_count)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject obj = Instantiate(projectile_list[projectile_list_count]);
                obj.SetActive(false);
                projectile_pool_list.Add(obj);

                obj.transform.parent = gameObject.transform;
            }
        }

        for (int enviroment_list_count = 0; enviroment_list_count < enviroment_list.Length; ++enviroment_list_count)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject obj = Instantiate(enviroment_list[enviroment_list_count]);
                obj.SetActive(false);
                enviroment_pool_list.Add(obj);

                obj.transform.parent = gameObject.transform;
            }
        }


        for (int i = 0; i < 10; ++i)
        {
            GameObject obj = Instantiate(hitbox);

            obj.SetActive(false);
            hitbox_pool_list.Add(obj);

            obj.transform.parent = gameObject.transform;

        }

        for (int i = 0; i < 10; ++i)
        {
            GameObject obj = Instantiate(item_pickup);

            obj.SetActive(false);
            item_pool_list.Add(obj);

            obj.transform.parent = gameObject.transform;

        }

        if (go_player_instance == null)
        {
            go_player_instance = Instantiate(go_player);
            go_player_instance.SetActive(false);
        }
    }

    //RESETS ALL ENEMY
    public void ResetSpawnerManager()
    {
        List<List<GameObject>> list = GetAllEntity();

        foreach (List<GameObject> i in list)
        {
            foreach (GameObject obj in i)
            {
                obj.SetActive(false);
            }
        }
    }

    //RETURNS SIZE OF ENTITY TYPES
    public int GetSizeOfEntity()
    {
        return entity_list.Length;
    }

    //RETURNS ALL ACTIVE ENTITES
    public List<List<GameObject>> GetAllEntity()
    {
        List<List<GameObject>> full_list = new List<List<GameObject>>();
        List<GameObject> list = new List<GameObject>();

        //MINIONS
        foreach (GameObject i in entity_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        full_list.Add(list);

        list = new List<GameObject>();
        list.Add(go_player_instance);

        full_list.Add(list);

        return full_list;
    }

    public GameObject GetEntityPlayer()
    {
        if (!go_player_instance.activeSelf)
        {
            go_player_instance.SetActive(true);

            go_player_instance.GetComponent<EntityPlayer>().SetPosition(new Vector3(0, 1, 0));
        }
        return go_player_instance;
    }

    //RETURNS TRUE IF ALL ENTITES ARE DEAD
    public bool IsAllEntityDead()
    {
        foreach (GameObject i in entity_pool_list)
        {
            if (i.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    //RETURNS ENTITY OBJECT IF IT FITS THE CRITERIA
    public GameObject GetEntityObjectFromPool(ENEMY _type)
    {
        foreach (GameObject entity_obj in entity_pool_list)
        {
            if (!entity_obj.activeSelf && entity_obj.CompareTag(entity_list[(int)_type].tag))
            {
                entity_obj.SetActive(true);
                return entity_obj;
            }
        }

        GameObject obj = Instantiate(entity_list[(int)_type]);
        obj.SetActive(false);
        entity_pool_list.Add(obj);
        obj.transform.parent = gameObject.transform;

        return GetEntityObjectFromPool(_type);
    }

    //RETURNS ENVIROMENT OBJECT IF IT FITS THE CRITERIA
    public GameObject GetEnviromentObjectFromPool(ENVIRONMENT _type)
    {
        foreach (GameObject enviroment_obj in enviroment_pool_list)
        {
            if (!enviroment_obj.activeSelf && enviroment_obj.CompareTag(enviroment_list[(int)_type].tag))
            {
                enviroment_obj.SetActive(true);
                return enviroment_obj;
            }
        }

        GameObject obj = Instantiate(enviroment_list[(int)_type]);
        obj.SetActive(false);
        enviroment_pool_list.Add(obj);
        obj.transform.parent = gameObject.transform;

        return GetEnviromentObjectFromPool(_type);
    }

    //RETURNS PROJECTILE OBJECT IF IT FITS THE CRITERIA
    public GameObject GetProjectileObjectFromPool(PROJECTILE _type)
    {
        foreach (GameObject projectile_obj in projectile_pool_list)
        {
            if (!projectile_obj.activeSelf && (projectile_obj.name.Equals(projectile_list[(int)_type].tag)))
            {
                projectile_obj.SetActive(true);
                return projectile_obj;
            }
        }

        GameObject obj = Instantiate(projectile_list[(int)_type]);
        obj.SetActive(false);
        projectile_pool_list.Add(obj);
        obj.transform.parent = gameObject.transform;

        return GetProjectileObjectFromPool(_type);
    }


    //RETURNS ENTITY OBJECT IF IT FITS THE CRITERIA
    public GameObject GetHitboxObjectFromPool()
    {
        foreach (GameObject hitbox_obj in hitbox_pool_list)
        {
            if (!hitbox_obj.activeSelf)
            {
                hitbox_obj.SetActive(true);
                return hitbox_obj;
            }
        }

        GameObject obj = Instantiate(hitbox);

        obj.SetActive(false);
        hitbox_pool_list.Add(obj);
        obj.transform.parent = gameObject.transform;

        return GetHitboxObjectFromPool();
    }

    //RETURNS ENTITY OBJECT IF IT FITS THE CRITERIA
    public GameObject GetItemObjectFromPool()
    {
        foreach (GameObject item_obj in item_pool_list)
        {
            if (!item_obj.activeSelf)
            {
                item_obj.SetActive(true);
                return item_obj;
            }
        }

        GameObject obj = Instantiate(item_pickup);

        obj.SetActive(false);
        item_pool_list.Add(obj);
        obj.transform.parent = gameObject.transform;

        return GetItemObjectFromPool();
    }
}
