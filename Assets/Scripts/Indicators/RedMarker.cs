using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RedMarker : MonoBehaviour
{
    
    Transform
        tf_followee;

    Vector3
        v3_rotation_axis;

    Projector
        p_main_projector;

    Tweener
        tween_expander;

    EntityLivingBase
        et_firer;

    System.Action
        act_oncomplete;

    /// <summary>
    /// Sets up this indicator for use, also resets previously assigned values
    /// </summary>
    /// <param name="_pos">Position to spawn at</param>
    /// <param name="_duration">Duration that this marker will take to reach _max_size</param>
    /// <param name="_max_size">How much this indicator should expand - unit is in meters </param>
    /// <param name="_finish_delay">How long this indicator should be alive after reaching _max_size</param>
    /// <param name="_onComplete">Function to run when this indicator finishes and the object disappears</param>
    /// <param name="_firer">If assigned, this indicator will be removed upon the _firer dying or getting disabled</param>
    public RedMarker SetUpIndicator(Vector3 _pos, float _duration, float _max_size, float _finish_delay = 0, System.Action _onComplete = null, EntityLivingBase _firer = null)
    {
        if (p_main_projector == null)
            p_main_projector = transform.GetChild(0).GetComponent<Projector>();

        et_firer = _firer;
        p_main_projector.orthographicSize = 0;
        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360.0f), 0); // Reset rotation
        transform.position = _pos;
        _max_size /= 2; // divide by 2 as orthographic view is doubled
        act_oncomplete = _onComplete;

        tf_followee = null;
        v3_rotation_axis = Vector3.zero;

        transform.DOScale(_max_size, _duration).SetEase(Ease.OutQuart).OnUpdate(() =>
        {
            p_main_projector.orthographicSize = transform.localScale.x;
        }).OnComplete(() =>
        {
            Invoke("DisableObject", _finish_delay);
        });

        return this;
    }

    public RedMarker SetToFollow(Transform _transform)
    {
        tf_followee = _transform;
        return this;
    }

    public RedMarker SetToRotate(Vector3 _axis)
    {
        v3_rotation_axis = _axis;
        return this;
    }

    private void Update()
    {
        if (tf_followee)
            transform.position = tf_followee.position;

        transform.Rotate(v3_rotation_axis * Time.deltaTime);

        if (et_firer != null && (et_firer.IsDead() || !et_firer.gameObject.activeInHierarchy))
            DisableObject();
    }

    void DisableObject()
    {
        transform.DOKill();

        gameObject.SetActive(false);

        if (act_oncomplete != null)
            act_oncomplete.Invoke();
    }

}
