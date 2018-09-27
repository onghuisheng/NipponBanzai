using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : EntityProjectiles
{
    private Vector3
        v3_startPos,
        v3_currEndPos,
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
        b_isRotatable,
        b_filpped,
        b_isDirect; //Set is the laser a direct fire to the target.
    

    //Set up projectile with a end point in mind.
    public void SetUpProjectile(float _lifetime, float _damage, float _speed, Vector3 _start, Vector3 _end, Vector3 _size, GameObject _source, GameObject _target)
    {
        base.SetUpProjectile(_source, _lifetime, _damage, _speed, _size, (v3_endPos - v3_startPos));

        SetPosition(_start);
        v3_startPos = _start;
        v3_endPos = _end;
        v3_currEndPos = _start;
        f_lifeElapse = 0;
        f_incrementor = 0;

        lr_line = GetComponent<LineRenderer>();
        lr_line.SetPosition(0, v3_startPos);
        lr_line.SetPosition(1, v3_currEndPos);
        lr_line.startWidth = 0.2f;
        lr_line.endWidth = 1f;

        go_target = _target;
        b_isDirect = true;
    }

    //Set up projectile with no end point in mind only direction.
    public void SetUpProjectile(float _lifetime, float _damage, float _speed, float _range, Vector3 _start, Vector3 _direction, Vector3 _size, GameObject _source, bool _isRotatable = false)
    {
        base.SetUpProjectile(_source, _lifetime, _damage, _speed, _size, _direction);

        SetPosition(_start);
        v3_startPos = _start;
        v3_endPos = _start + (_direction * _range); //30.0f is the range for now.
        v3_currEndPos = _start;
        f_lifeElapse = 0;
        f_incrementor = 0;
        b_isRotatable = _isRotatable;
        b_isDirect = false;

        f_distance = Vector3.Distance(v3_startPos, v3_endPos);

        lr_line = GetComponent<LineRenderer>();
        lr_line.SetPosition(0, v3_startPos);
        lr_line.SetPosition(1, v3_currEndPos);
        lr_line.startWidth = _size.x;
        lr_line.endWidth = _size.x;
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

        if (b_isRotatable)
            UpdateRotation();

        UpdatePosition();
    }

    public void UpdateRotation()
    {
        Vector3 angle = Quaternion.Euler(90, 0, 0) * Vector3.forward;
        v3_endPos = RotatePointAroundPivot(v3_endPos, v3_startPos, angle);

        if (v3_endPos.y > -50) //Min
        {
            b_filpped = false;
        }
        else if ( v3_endPos.y < -300) //Max
        {
            b_filpped = true;
        }

        if (b_filpped)
        {
            v3_endPos.y += 50.0f * Time.deltaTime;
        }
        else
            v3_endPos.y -= 50.0f * Time.deltaTime;

        //Debug.Log("beam y :" + v3_endPos.y);
    }

    public void UpdatePosition()
    {
            Debug.Log("MOVE");
            f_incrementor += F_speed * Time.deltaTime;

            v3_currEndPos = Vector3.Lerp(v3_currEndPos, v3_endPos, f_incrementor);
            CheckForObjectsInPath();

            lr_line.SetPosition(1, v3_currEndPos);
    }

    void CheckForObjectsInPath()
    {
        RaycastHit hit;

        Vector3 direction = v3_currEndPos - v3_startPos;
        int ignoreEnemiesMask = ~(1 << LayerMask.NameToLayer(Go_owner.tag));
        if (Physics.Linecast(v3_startPos, v3_currEndPos, out hit, ignoreEnemiesMask))
        {
            Debug.Log("Hit" + hit);
            v3_currEndPos = hit.point;

            if (!TagHelper.IsTagBanned(hit.collider.gameObject.tag))
            {
                //Spawn Hitbox to damage player.
                SetUpHitBox(Go_owner.name, Go_owner.tag, Go_owner.GetInstanceID().ToString(), F_damage, transform.localScale, v3_currEndPos, transform.rotation, 0.1f);
            }
        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point;
    }

    public void NewEndPoint(Vector3 _dir, float _range)
    {
        v3_endPos = v3_startPos + (_dir.normalized * _range); //30.0f is the range for now.
    }
}
