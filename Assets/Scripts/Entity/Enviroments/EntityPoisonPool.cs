using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPoisonPool : EntityEnviroment {

    public override void SetUpObjectWLifeTime(float _lifetime, Vector3 _pos, Vector3 _size, bool _isStatic = true)
    {
        base.SetUpObjectWLifeTime(_lifetime, _pos, _size, _isStatic);

        // SetUpHitBox(gameObject.name, gameObject.tag, gameObject.GetInstanceID().ToString(), );
    }
    
}
