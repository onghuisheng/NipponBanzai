using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectBobber : MonoBehaviour
{

    public Vector3 m_RotationSpeed;

    public bool m_IsFloating;

    public float m_FloatYOffset;

    public float m_FloatDurationToReachOffset = 1;

    public Vector2 m_FloatStartupDelayRange;

    public Ease m_FloatEasing = Ease.InOutQuad;

    public bool m_RotateChildInstead;

    public int m_RotateChildIndex;

    private void Start()
    {
        if (m_RotationSpeed != Vector3.zero)
        {
            Transform target = null;

            if (m_RotateChildInstead)
                target = transform.GetChild(m_RotateChildIndex);
            else
                target = transform;


            target.DOLocalRotate(m_RotationSpeed, 1).SetLoops(-1, LoopType.Incremental).SetRelative().SetEase(Ease.Linear);
        }

        if (m_IsFloating && m_FloatYOffset != 0)
        {
            float delay = 0;

            if (m_FloatStartupDelayRange != Vector2.zero)
                delay = Random.Range(m_FloatStartupDelayRange.x, m_FloatStartupDelayRange.y);

            transform.DOLocalMoveY(transform.localPosition.y + m_FloatYOffset, m_FloatDurationToReachOffset).SetLoops(-1, LoopType.Yoyo).SetEase(m_FloatEasing).SetDelay(delay);
            // transform.Translate(0, -m_FloatYOffset, 0, Space.Self);
        }
    }

}
