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
        ARCH_PROJECTILE = 0,
        STRAIGHT_PROJECTILE,
        LASER_PROJECTILE, 
        SWORD_PROJECTILE,
        HEART_PROJECTILE
    }

    public enum ENVIRONMENT
    {
        CRYSTAL = 0,
        POISON_POOL
    }

    public enum INDICATOR
    {
        RED_MARKER = 0
    }

    [SerializeField]
    private GameObject[] entity_list;
    [SerializeField]
    private GameObject[] projectile_list;
    [SerializeField]
    private GameObject[] enviroment_list;
    [SerializeField]
    private GameObject[] indicator_list;

    [SerializeField]
    private GameObject hitbox;
    [SerializeField]
    private GameObject item_pickup;
    [SerializeField]
    private GameObject go_player;

    private List<GameObject> entity_pool_list = new List<GameObject>();
    private List<GameObject> projectile_pool_list = new List<GameObject>();
    private List<GameObject> hitbox_pool_list = new List<GameObject>();
    private List<GameObject> item_pool_list = new List<GameObject>();
    private List<GameObject> enviroment_pool_list = new List<GameObject>();
    private List<GameObject> indicator_pool_list = new List<GameObject>();
    private GameObject go_player_instance;
    private GameObject go_spawnpoint;

    //SETTING UP THE SPAWNER
    protected override void Awake()
    {
        base.Awake();

        go_spawnpoint = GameObject.FindWithTag("Respawn");

        for (int entity_list_count = 0; entity_list_count < entity_list.Length; ++entity_list_count)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject obj = null;

                if (go_spawnpoint != null)
                    obj = Instantiate(entity_list[entity_list_count], go_spawnpoint.transform.position, Quaternion.identity);
                else
                    obj = Instantiate(entity_list[entity_list_count]);

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

        //ENVIRONMENT
        list = new List<GameObject>();
        foreach (GameObject i in enviroment_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        full_list.Add(list);

        //PROJECTILE
        list = new List<GameObject>();
        foreach (GameObject i in enviroment_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        full_list.Add(list);

        //HITBOX
        list = new List<GameObject>();
        foreach (GameObject i in hitbox_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        full_list.Add(list);

        //ITEM
        list = new List<GameObject>();
        foreach (GameObject i in item_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        full_list.Add(list);

        //INDICATOR
        list = new List<GameObject>();
        foreach (GameObject i in indicator_pool_list)
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

    public List<GameObject> GetAllActiveInSurrounding(Vector3 _pos, float _range, System.Type _type)
    {
        List<GameObject> _list = new List<GameObject>();

        foreach (var list in GetAllEntity())
        {
            foreach (GameObject go in list)
            {
                Entity _entity = go.GetComponent<Entity>();
                if (_entity != null && (_entity.GetType().Equals(_type) || _entity.GetType().IsSubclassOf(_type)))
                {
                    if (Vector3.Distance(_pos, _entity.GetPosition()) < _range)
                        _list.Add(go);
                }
                else
                    break;
            }
        }

        return _list;
    }

    public GameObject GetEntityPlayer()
    {
        if (!go_player_instance.activeSelf)
        {
            go_player_instance.SetActive(true);
            var player = go_player_instance.GetComponent<EntityPlayer>();
            player.SetPosition(new Vector3(0, 1, 0));
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

    public List<GameObject> GetActiveEntityObjects()
    {
        List<GameObject> list = new List<GameObject>();

        list.Add(go_player_instance);

        foreach (GameObject i in entity_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        return list;
    }

    public List<GameObject> GetActiveEnvironmentObjects()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject i in enviroment_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        return list;
    }

    public List<GameObject> GetActiveProjectileObjects()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject i in projectile_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        return list;
    }

    public List<GameObject> GetActiveHitBoxObjects()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject i in hitbox_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        return list;
    }

    public List<GameObject> GetActivePickUpObjects()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject i in item_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        return list;
    }

    public List<GameObject> GetActiveIndicatorObjects()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject i in indicator_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        return list;
    }

    //RETURNS ENTITY OBJECT IF IT FITS THE CRITERIA
    public GameObject GetEntityObjectFromPool(ENEMY _type)
    {
        foreach (GameObject entity_obj in entity_pool_list)
        {
            if (!entity_obj.activeSelf && entity_obj.name.Equals(entity_list[(int)_type].name + "(Clone)"))
            {
                var livingBase = entity_obj.GetComponent<EntityLivingBase>();
                livingBase.HardReset();
                entity_obj.SetActive(true);
                return entity_obj;
            }
        }

        GameObject obj = null;
        
        if (go_spawnpoint != null)
            obj = Instantiate(entity_list[(int)_type], go_spawnpoint.transform.position, Quaternion.identity);
        else
            obj = Instantiate(entity_list[(int)_type]);

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
            if (!enviroment_obj.activeSelf && enviroment_obj.name.Equals(enviroment_list[(int)_type].name + "(Clone)"))
            {
                enviroment_obj.GetComponent<Entity>().HardReset();
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
            if (!projectile_obj.activeSelf && projectile_obj.name.Equals(projectile_list[(int)_type].name + "(Clone)"))
            {
                projectile_obj.GetComponent<Entity>().HardReset();
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
                hitbox_obj.GetComponent<Entity>().HardReset();
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
                item_obj.GetComponent<Entity>().HardReset();
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

    //RETURNS ENTITY OBJECT IF IT FITS THE CRITERIA
    public GameObject GetIndicatorObjectFromPool(INDICATOR _type)
    {
        foreach (GameObject indicator in indicator_pool_list)
        {
            if (!indicator.activeSelf && indicator.name.Equals(indicator_list[(int)_type].name + "(Clone)"))
            {
                indicator.SetActive(true);
                return indicator;
            }
        }

        GameObject obj = Instantiate(indicator_list[(int)_type]);
        obj.SetActive(false);
        indicator_pool_list.Add(obj);
        obj.transform.parent = gameObject.transform;

        return GetIndicatorObjectFromPool(_type);
    }

}
