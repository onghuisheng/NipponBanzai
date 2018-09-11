using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : EntityProjectiles
{

    private float f_lifeElapse;

    public void SetUpProjectile(GameObject _firer, Vector3 _direction, float _lifetime, float _damage, float _speed, Vector3 _size)
    {
        base.SetUpProjectile(_firer, _lifetime, _damage, _speed, _size, _direction.normalized);
        transform.position = _firer.GetComponent<Collider>().bounds.center;
        f_lifeElapse = 0;
        transform.rotation = Quaternion.Euler(0, 0, -90); // Imagine a capsule lying on the floor
        transform.Rotate(Mathf.Atan2(V3_dir.z, V3_dir.x) * Mathf.Rad2Deg, 0, 0); // Face the capsule to the direction
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        f_lifeElapse += Time.deltaTime;

        if (Go_owner == null || f_lifeElapse >= F_lifetime)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            transform.position += (V3_dir * Time.deltaTime * F_speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != gameObject.tag && Go_owner.tag != other.tag && !TagHelper.IsTagBanned(other.tag))
        {
            SetUpHitBox(Go_owner.name, Go_owner.tag, Go_owner.GetInstanceID().ToString(), F_damage, transform.localScale, transform.position, transform.rotation, 0.1f);
            gameObject.SetActive(false);
        }
    }

}