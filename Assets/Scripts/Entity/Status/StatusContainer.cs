using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusContainer
{
    EntityLivingBase m_Victim;

    StatusBase.StatusType m_CurrentStatusBit = StatusBase.StatusType.None;

    List<StatusBase> m_StatusList = new List<StatusBase>();

    #region Getters
    public bool isStunned { get { return (m_CurrentStatusBit & StatusBase.StatusType.Stunned) == StatusBase.StatusType.Stunned; } }
    public bool isPanicking { get { return (m_CurrentStatusBit & StatusBase.StatusType.Panic) == StatusBase.StatusType.Panic; } }
    public bool isPoisoned { get { return (m_CurrentStatusBit & StatusBase.StatusType.Poison) == StatusBase.StatusType.Poison; } }
    #endregion

    public StatusContainer(EntityLivingBase victim) { m_Victim = victim; }

    public void ApplyStatus(StatusBase status)
    {
        m_CurrentStatusBit |= status.statusType;

        bool hasExistingStatus = false;

        foreach (StatusBase remaining in m_StatusList)
        {
            if (remaining.statusType == status.statusType)
            {
                hasExistingStatus = true;
                break;
            }
        }

        status.OnStatusBegin(m_Victim, !hasExistingStatus);
        m_StatusList.Add(status);
    }

    public void UpdateStatuses()
    {
        for (int i = m_StatusList.Count - 1; i >= 0; i--) // Reverse iterate so we can remove elements while iterating
        {
            var status = m_StatusList[i];

            status.timeLeft -= Time.deltaTime;

            if (status.timeLeft <= 0)
            {
                m_StatusList.RemoveAt(i);

                bool hasExistingStatus = false;

                // Clear the status bits if this is the last status in the list
                foreach (StatusBase remaining in m_StatusList)
                {
                    if (remaining.statusType == status.statusType)
                    {
                        hasExistingStatus = true;
                        break;
                    }
                }

                if (hasExistingStatus == false)
                    m_CurrentStatusBit &= ~status.statusType;

                status.OnStatusEnd(m_Victim, !hasExistingStatus);
            }
            else
            {
                status.OnStatusUpdate(m_Victim);
            }
        }
    }

}

public class Status
{



}