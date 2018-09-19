using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[assembly: InternalsVisibleTo("EntityLivingBase")]
public class StatusContainer
{

    Status.StatusType m_CurrentStatusBit = Status.StatusType.None;

    List<Status> m_StatusList = new List<Status>();

    #region Getters
    public bool isStunned { get { return (m_CurrentStatusBit & Status.StatusType.Stunned) == Status.StatusType.Stunned; } }
    public bool isPanicking { get { return (m_CurrentStatusBit & Status.StatusType.Panic) == Status.StatusType.Panic; } }
    public bool isPoisoned { get { return (m_CurrentStatusBit & Status.StatusType.Poison) == Status.StatusType.Poison; } }
    #endregion

    public void ApplyStatus(Status status)
    {
        m_CurrentStatusBit |= status.statusType;
        status.OnStatusBegin();
        m_StatusList.Add(status);
    }

    internal void UpdateStatuses(EntityLivingBase entity)
    {
        for (int i = m_StatusList.Count - 1; i >= 0; i--) // Reverse iterate so we can remove elements while iterating
        {
            var status = m_StatusList[i];

            status.timeLeft -= Time.deltaTime;

            if (status.timeLeft <= 0)
            {
                status.OnStatusEnd();
                m_StatusList.RemoveAt(i);
                continue;
            }
            else
            {
                status.OnStatusUpdate(entity);
            }
        }
    }

}
