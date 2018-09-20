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

    internal override void OnStatusBegin()
    {
        Debug.Log("OnPoisonStart");
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

    internal override void OnStatusEnd()
    {
        Debug.Log("OnPoisonEnd");
    }

}

public class StatusPanic : StatusBase
{

    public StatusPanic(float duration) : base(StatusType.Panic, duration)
    {
    }

    internal override void OnStatusBegin()
    {
    }

    internal override void OnStatusEnd()
    {
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

    internal override void OnStatusBegin()
    {
    }

    internal override void OnStatusEnd()
    {
    }

    internal override void OnStatusUpdate(EntityLivingBase entity)
    {
    }

}