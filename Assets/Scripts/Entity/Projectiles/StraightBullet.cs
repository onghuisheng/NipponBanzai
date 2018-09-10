using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : EntityProjectiles
{

    private GameObject go_firer = null;
    private Vector3 v3_direction;
    private float f_lifeElapse;

    public void SetUpProjectile(GameObject _firer, Vector3 _direction, float _lifetime, float _damage, float _speed, Vector3 _size)
    {
        base.SetUpProjectile(_lifetime, _damage, _speed, _size);
        transform.position = _firer.transform.position;
        v3_direction = _direction.normalized;
        go_firer = _firer;
        f_lifeElapse = 0;
        transform.rotation = Quaternion.Euler(0, 0, -90); // Imagine a capsule lying on the floor
        transform.Rotate(Mathf.Atan2(v3_direction.z, v3_direction.x) * Mathf.Rad2Deg, 0, 0); // Face the capsule to the direction
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        f_lifeElapse += Time.deltaTime;

        if (go_firer == null || f_lifeElapse >= F_lifetime)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            transform.position += (v3_direction * Time.deltaTime * F_speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (go_firer.tag != other.tag && other.tag != gameObject.tag)
        {
            SetUpHitBox(go_firer.name, go_firer.tag, go_firer.GetInstanceID().ToString(), F_damage, transform.localScale, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }

}