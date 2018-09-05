using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityProjectiles : Entity
{
    private float
        f_lifetime,
        f_damage,
        f_range;

    private Vector3
        v3_size;


    public virtual void SetUpProjectile(float _lifetime, float _damage, float _range, Vector3 _size)
    {
        f_lifetime = _lifetime;
        f_damage = _damage;
        f_range = _range;

        v3_size = _size;
    }
}
