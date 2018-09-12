using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPoisonPool : EntityEnviroment
{

    public void SetUpPoisonPoolWLifeTime(float _damageTick, float _lifetime, Vector3 _pos, Vector3 _size, bool _isStatic = true)
    {
        base.SetUpObjectWLifeTime(_lifetime, _pos, _size, _isStatic);

        SetUpHitBox(gameObject.name, gameObject.tag, gameObject.GetInstanceID().ToString(), _damageTick, _size, _pos, Quaternion.Euler(0, Random.Range(0, 360), 0), _lifetime, 0.1f);
    }

}
