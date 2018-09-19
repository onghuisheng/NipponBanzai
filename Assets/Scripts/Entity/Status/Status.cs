using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[assembly: InternalsVisibleTo("StatusContainer")]
public abstract class Status
{
    [Flags]
    public enum StatusType
    {
        None = 0,
        Stunned = 1 << 0,
        Panic = 1 << 1,
        Poison = 1 << 2,
    }

    StatusType m_StatusType;
    public StatusType statusType {
        get { return m_StatusType; }
    }

    float m_TimeLeft;
    public float timeLeft {
        get { return m_TimeLeft; }
        internal set { m_TimeLeft = value; }
    }

    protected Status(StatusType statusType, float duration)
    {
        m_StatusType = statusType;
        m_TimeLeft = duration;
    }

    /// <summary>
    /// Called only once when this Status is added
    /// </summary>
    internal abstract void OnStatusBegin();

    /// <summary>
    /// Called once per Update()
    /// </summary>
    internal abstract void OnStatusUpdate(EntityLivingBase entity);

    /// <summary>
    /// Called when duration has ended and this is to get deleted (Note: There might still be other instances of this Status that are still active)
    /// </summary>
    internal abstract void OnStatusEnd();

}