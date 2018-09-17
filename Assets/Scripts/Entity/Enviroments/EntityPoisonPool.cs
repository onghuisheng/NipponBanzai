using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPoisonPool : EntityEnviroment
{

    protected override void Start()
    {
        base.Start();
    }

    public void SetUpPoisonPoolWLifeTime(GameObject _owner, float _damageTick, float _lifetime, Vector3 _pos, bool _isStatic = true)
    {
        base.SetUpObjectWLifeTime(_lifetime, _pos, Vector3.one, _isStatic);

        ParticleSystem ps = ParticleHandler.GetInstance().SpawnParticle(ParticleHandler.ParticleType.PoisonCloud, null, _pos, Vector3.one, Quaternion.identity.eulerAngles, _lifetime).GetComponent<ParticleSystem>();

        Vector3 boundSize = ps.GetComponent<Renderer>().bounds.size * 0.25f;
        SetUpHitBox(_owner.name, _owner.tag, _owner.GetInstanceID().ToString(), _damageTick, boundSize, _pos, Quaternion.Euler(0, Random.Range(0, 360), 0), _lifetime, 0.1f);
    }

}
