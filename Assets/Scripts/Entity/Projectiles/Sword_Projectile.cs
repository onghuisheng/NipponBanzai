using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Projectile : EntityProjectiles
{
    private float f_lifeElapse;
    private EntityLivingBase
        go_target;

    public void SetUpProjectile(GameObject _firer, Vector3 _position, Vector3 _direction, float _lifetime, float _damage, float _speed, Vector3 _size)
    {
        base.SetUpProjectile(_firer, _lifetime, _damage, _speed, _size, _direction.normalized);
        transform.position = _position;
        f_lifeElapse = 0;
        transform.rotation = Quaternion.Euler(0, 0, -90); // Imagine a capsule lying on the floor
        transform.Rotate(Mathf.Atan2(V3_dir.z, V3_dir.x) * Mathf.Rad2Deg, 0, 0); // Face the capsule to the direction

        go_target = null;
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
            go_target = null;
            return;
        }
        else
        {
            if (go_target != null)
            {                
                transform.LookAt(go_target.gameObject.transform);
                transform.position += (transform.forward * Time.deltaTime * F_speed);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + 90, transform.localEulerAngles.y, transform.localEulerAngles.z);

                if (go_target.IsDead())
                    go_target = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != gameObject.tag && Go_owner.tag != other.tag && !TagHelper.IsTagBanned(other.tag))
        {
            SetUpHitBox(Go_owner.name, Go_owner.tag, Go_owner.GetInstanceID().ToString(), F_damage, transform.localScale, transform.position, transform.rotation, 0.1f);
            gameObject.SetActive(false);
            go_target = null;
        }
    }

    public void SetTarget(EntityLivingBase _target)
    {
        go_target = _target;
    }

    public bool HasTarget()
    {
        return go_target != null;
    }
}