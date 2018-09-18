using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : Singleton<ParticleHandler>
{

    public enum ParticleType
    {
        PoisonCloud = 0,
        PoisonMouthDrip,
        SummoningPortal,
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
        
    public GameObject SpawnParticle(ParticleType type, Transform parent, Vector3 localPos, Vector3 localScale, Vector3 localRotation, float duration)
    {
        if (m_ParticlePrefabs[(int)type].name != type.ToString())
        {
            Debug.LogError("Error: Tried to spawn a particle of different name");
            return null;
        }

        GameObject go = Instantiate(m_ParticlePrefabs[(int)type], (parent == null) ? localPos : parent.position + localPos, Quaternion.Euler(localRotation), parent);
        go.transform.localScale = localScale;

        if (duration != 0)
            Destroy(go, duration);

        return go;
    }

}
