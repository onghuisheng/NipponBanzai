using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : EntityProjectiles
{
    private Vector3
        v3_startPos,
        v3_endPos;

    private float
        f_lifeElapse,
        f_distance,
        f_incrementor;

    private LineRenderer
        lr_line;

    private GameObject
        go_target;

    private bool
        b_isDirect; //Set is the laser a direct fire to the target.

    //Set up projectile with a end point in mind.
    public void SetUpProjectile(float _lifetime, float _damage, float _speed, Vector3 _start, Vector3 _end, Vector3 _size, GameObject _source, GameObject _target)
    {
        base.SetUpProjectile(_source, _lifetime, _damage, _speed, _size, (v3_endPos - v3_startPos));

        SetPosition(_start);
        v3_startPos = _start;
        v3_endPos = _end;
        f_lifeElapse = 0;
        f_incrementor = 0;

        go_target = _target;
        b_isDirect = true;
    }

    //Set up projectile with no end point in mind only direction.
    public void SetUpProjectile(float _lifetime, float _damage, float _speed, Vector3 _start, Vector3 _direction, Vector3 _size, GameObject _source)
    {
        base.SetUpProjectile(_source, _lifetime, _damage, _speed, _size, _direction);

        SetPosition(_start);
        v3_startPos = _start;
        v3_endPos = _direction * 30.0f; //30.0f is the range for now.
        f_lifeElapse = 0;
        f_incrementor = 0;
        b_isDirect = false;

        f_distance = Vector3.Distance(v3_startPos, v3_endPos);
    }
    // Use this for initialization
    protected override void Start()
    {
        lr_line = GetComponent<LineRenderer>();
        lr_line.startWidth = 0.2f;
        lr_line.endWidth = 1f;
    }

    // Update is called once per frame
    protected override void Update()
    {

        f_lifeElapse += Time.deltaTime;

        if (f_lifeElapse > F_lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        if (b_isDirect)
        {
            v3_endPos = go_target.transform.position;
        }



        CheckForObjectsInPath();

        UpdatePosition(v3_startPos, v3_endPos);
    }

    public void UpdatePosition(Vector3 _start, Vector3 _end)
    {
        if (f_incrementor < f_distance)
        {
            f_incrementor += F_speed * Time.deltaTime;

            float x = Mathf.Lerp(0, f_distance, f_incrementor);
            Vector3 finalPoint = x * Vector3.Normalize(_end - _start) + _start;

            lr_line.SetPosition(0, _start);
            lr_line.SetPosition(1, finalPoint);
        }
    }

    void CheckForObjectsInPath()
    {
        RaycastHit hit;

        Vector3 direction = v3_endPos - v3_startPos;

        if (Physics.Linecast(v3_startPos, v3_endPos, out hit))
        {
            v3_endPos = hit.point;

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Spawn Hitbox to damage player.
            }
        }
    }
}
