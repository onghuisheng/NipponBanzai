using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusPoison : StatusBase
{

    float m_TickDamage, m_TickRate, m_NextTickTime;

    /// <param name="tickRate">How many times to tick per second</param>
    public StatusPoison(float duration, float damagePerTick, float tickRate) : base(StatusType.Poison, duration)
    {
        m_TickDamage = damagePerTick;
        m_TickRate = tickRate;
    }

    internal override void OnStatusBegin(EntityLivingBase entity, bool isFirst)
    {
        if (isFirst)
        {
            GameObject particle = ParticleHandler.GetInstance().SpawnParticle(ParticleHandler.ParticleType.PoisonMouthDrip, entity.transform, new Vector3(0, entity.GetComponent<Collider>().bounds.size.y, 0), Vector3.one, Vector3.zero, 0);
            particle.name = "PoisonBoi";
            var particleSystem = particle.GetComponent<ParticleSystem>().main;
            particleSystem.loop = true;
        }
    }

    internal override void OnStatusEnd(EntityLivingBase entity, bool isLast)
    {
        if (isLast)
        {
            GameObject.Destroy(entity.transform.Find("PoisonBoi").gameObject);
        }
    }

    internal override void OnStatusUpdate(EntityLivingBase entity)
    {
        if (Time.time > m_NextTickTime)
        {
            m_NextTickTime = Time.time + m_TickRate;

            var dmgSrc = new DamageSource();
            dmgSrc.SetUpDamageSource("Poison", "God", "", m_TickDamage, false);

            entity.OnAttacked(dmgSrc, 0);
        }
    }

}

public class StatusPanic : StatusBase
{

    GameObject m_PanicParticle;

    public StatusPanic(float duration) : base(StatusType.Panic, duration)
    {
    }

    internal override void OnStatusBegin(EntityLivingBase entity, bool isFirst)
    {
        if (isFirst)
        {
            GameObject particle = ParticleHandler.GetInstance().SpawnParticle(ParticleHandler.ParticleType.Heart_Burst, entity.transform, new Vector3(0, entity.GetComponent<Collider>().bounds.size.y, 0), Vector3.one, Vector3.zero, 0);
            particle.name = "HeartyBoi";
            var particleSystem = particle.GetComponent<ParticleSystem>().main;
            particleSystem.loop = true;
        }
    }

    internal override void OnStatusEnd(EntityLivingBase entity, bool isLast)
    {
        if (isLast)
        {
            GameObject.Destroy(entity.transform.Find("HeartyBoi").gameObject);
            Debug.Log("Poison: " +entity.Stc_Status.isPoisoned);
            Debug.Log("Panic: " + entity.Stc_Status.isPanicking);
        }
    }

    internal override void OnStatusUpdate(EntityLivingBase entity)
    {
    }

}

public class StatusStunned : StatusBase
{

    public StatusStunned(float duration) : base(StatusType.Stunned, duration)
    {
    }

    internal override void OnStatusBegin(EntityLivingBase entity, bool isFirst)
    {
    }

    internal override void OnStatusEnd(EntityLivingBase entity, bool isLast)
    {
    }

    internal override void OnStatusUpdate(EntityLivingBase entity)
    {
    }

}