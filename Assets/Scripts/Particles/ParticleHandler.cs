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
        Charging,
        Heart_Burst,
        Charging_Beam, 
        Souls,
        BossCharging, 
        Blind, 
        Mana,
        Heal
    }

    [SerializeField]
    List<GameObject> m_ParticlePrefabs;
    
    public GameObject SpawnParticle(ParticleType type, Transform parent, Vector3 localPos, Vector3 localScale, Vector3 localRotation, float duration, System.Action<float> onParticleUpdate = null, System.Action onParticleDestroy = null)
    {
        if (m_ParticlePrefabs[(int)type].name != type.ToString())
        {
            Debug.LogError("Error: Tried to spawn a particle of different name than the enum");
            return null;
        }

        GameObject go = Instantiate(m_ParticlePrefabs[(int)type], (parent == null) ? localPos : parent.position + localPos, Quaternion.Euler(localRotation), parent);
        go.transform.localScale = localScale;

        if (duration != 0)
        {
            Destroy(go, duration);
            StartCoroutine(UpdateParticle(go, duration, onParticleUpdate, onParticleDestroy));
        }

        return go;
    }

    private IEnumerator UpdateParticle(GameObject particle, float delay, System.Action<float> onParticleUpdate, System.Action onParticleDestroy)
    {
        float currentDuration = 0;

        while (currentDuration < delay)
        {
            currentDuration += Time.deltaTime;

            if (onParticleUpdate != null)
                onParticleUpdate.Invoke(currentDuration);

            yield return null;
        }

        Destroy(particle);

        if (onParticleDestroy != null)
            onParticleDestroy.Invoke();
    }

}
