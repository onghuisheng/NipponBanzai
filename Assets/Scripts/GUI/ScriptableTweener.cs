using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[CreateAssetMenu(fileName = "TweenData", menuName = "Tween Data")]
public class ScriptableTweener : ScriptableObject
{

    public bool m_UsingRectTransform = false;

    public bool m_TweenPosition = false;
    public Vector3 m_PositionTarget;
    public Ease m_PositionEaseType = Ease.Linear;

    public bool m_TweenScale = false;
    public Vector3 m_ScaleTarget = Vector3.one;
    public Ease m_ScaleEaseType = Ease.Linear;

    public bool m_TweenRotation = false;
    public Vector3 m_RotationTarget;
    public Ease m_RotationEaseType = Ease.Linear;

}
