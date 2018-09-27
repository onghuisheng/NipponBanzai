using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : EntityEnviroment {

    public void SetUpCrystal(GameObject _owner, float _damage, Vector3 _pos, Vector3 _offset, Vector3 _size, bool _isDestructible, bool _isStatic = true)
    {
        base.SetUpObject(_pos, _size, _isDestructible, _isStatic);

        //ParticleSystem ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        //ps.Clear();
        //ps.Simulate(5);
        //ps.Play();
        _pos += _offset;
        //Vector3 boundSize = ps.GetComponent<Renderer>().bounds.size * 0.25f;
        SetUpHitBox(_owner.name, _owner.tag, _owner.GetInstanceID().ToString(), _damage, _size * 10, _pos, _owner.transform.rotation);
    }
}
