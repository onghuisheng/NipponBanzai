using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : Singleton<ParticleHandler>
{

    public enum ParticleType
    {
        PoisonCloud = 0
    }

    [SerializeField]
    private List<GameObject> m_ParticlePrefabs;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public GameObject SpawnParticle(ParticleType type, Transform parent, Vector3 localPos, Vector3 localRotation, float duration)
    {
        if (m_ParticlePrefabs[(int)type].name != type.ToString())
        {
            Debug.LogError("Error: Tried to spawn a particle of different name");
            return null;
        }

        GameObject go = Instantiate(m_ParticlePrefabs[(int)type], parent);
        go.transform.localPosition = localPos;
        go.transform.localRotation = Quaternion.Euler(localRotation);

        if (duration != 0)
            Destroy(go, duration);

        return go;
    }

}
