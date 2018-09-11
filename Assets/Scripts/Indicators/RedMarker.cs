using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RedMarker : MonoBehaviour
{

    float f_duration, f_max_size;

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
    public void SetUpIndicator(Vector3 _pos, float _duration, float _max_size, float _finish_delay = 0, System.Action _onComplete = null, EntityLivingBase _firer = null)
    {
        if (p_main_projector == null)
            p_main_projector = transform.GetChild(0).GetComponent<Projector>();

        et_firer = _firer;
        p_main_projector.orthographicSize = 0;
        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360.0f), 0); // Reset rotation
        transform.position = _pos;
        f_duration = _duration;
        f_max_size = _max_size / 2; // divide by 2 as orthographic view is doubled
        act_oncomplete = _onComplete;

        transform.DOScale(_max_size, _duration).SetEase(Ease.OutQuart).OnUpdate(() =>
        {
            p_main_projector.orthographicSize = transform.localScale.x;
        }).OnComplete(() =>
        {
            Invoke("DisableObject", _finish_delay);
        });
    }

    private void Update()
    {
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
