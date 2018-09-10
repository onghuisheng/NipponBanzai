using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityProjectiles : Entity
{
    private float
        f_lifetime,
        f_damage,
        f_speed;

    private Vector3
        v3_size, 
        v3_dir;

    private GameObject
        go_owner; 


    #region Getter/Setter
    public float F_lifetime
    {
        get
        {
            return f_lifetime;
        }

        set
        {
            f_lifetime = value;
        }
    }

    public float F_damage
    {
        get
        {
            return f_damage;
        }

        set
        {
            f_damage = value;
        }
    }

    public float F_speed
    {
        get
        {
            return f_speed;
        }

        set
        {
            f_speed = value;
        }
    }

    public Vector3 V3_size
    {
        get
        {
            return v3_size;
        }

        set
        {
            v3_size = value;
        }
    }

    public Vector3 V3_dir
    {
        get
        {
            return v3_dir;
        }

        set
        {
            v3_dir = value;
        }
    }

    public GameObject Go_owner
    {
        get
        {
            return go_owner;
        }

        set
        {
            go_owner = value;
        }
    }

    #endregion

    public virtual void SetUpProjectile(GameObject _owner, float _lifetime, float _damage, float _speed, Vector3 _size, Vector3 _dir)
    {
        F_lifetime = _lifetime;
        F_damage = _damage;
        F_speed = _speed;

        V3_size = _size;
        V3_dir = _dir;
        Go_owner = _owner;
        SetSize(_size);
    }
}
