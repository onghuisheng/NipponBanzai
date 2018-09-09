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
        v3_size;

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

    #endregion

    public virtual void SetUpProjectile(float _lifetime, float _damage, float _speed, Vector3 _size)
    {
        f_lifetime = _lifetime;
        f_damage = _damage;
        f_speed = _speed;

        v3_size = _size;

    }
}
