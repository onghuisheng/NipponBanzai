using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private Rigidbody
        rb_rigidbody;

	protected virtual void Start ()
    {
        HardReset();
    }

    protected virtual void Update()
    {
        SetCollisionSize();
    }

    protected virtual void SetCollisionSize()
    {
        if (!gameObject.GetComponent<BoxCollider>() || !gameObject.GetComponent<MeshFilter>())
            return;

        if (gameObject.GetComponent<BoxCollider>().size != gameObject.GetComponent<MeshFilter>().mesh.bounds.size)
            gameObject.GetComponent<BoxCollider>().size = gameObject.GetComponent<MeshFilter>().mesh.bounds.size;
    }

    protected virtual void SetUpHitBox(string _name, string _tag, string _id, float _damage, Vector3 _size, Vector3 _pos, Quaternion _rot, float _timer = 0.1f, float _iframe_timer = 0.3f)
    {
        GameObject obj = ObjectPool.GetInstance().GetHitboxObjectFromPool();
        HitboxTrigger obj_hitbox = obj.GetComponent<HitboxTrigger>();

        DamageSource dmgsrc = new DamageSource();

        dmgsrc.SetUpDamageSource(_name,
            _tag,
            _id,
            _damage);

        obj_hitbox.SetHitbox(dmgsrc, _size, _timer, _iframe_timer);

        obj_hitbox.SetPosition(_pos);

        obj_hitbox.transform.rotation = _rot;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public void SetPosition(Vector3 _input)
    {
        gameObject.transform.position = _input;
    }

    public Vector3 GetSize()
    {
        return gameObject.transform.localScale;
    }

    public void SetSize(Vector3 _input)
    {
        gameObject.transform.localScale = _input;
    }

    public Vector3 GetVelocity()
    {
        if (rb_rigidbody != null)
        {
            return rb_rigidbody.velocity;
        }
        else
        {
            Debug.Log("RigidBody Not Found In This Object: " + gameObject.name);
            return new Vector3(0, 0, 0);
        }
    }

    public void SetVelocity(Vector3 _input)
    {
        if (rb_rigidbody != null)
        {
            rb_rigidbody.velocity = _input;
        }
        else
        {
            Debug.Log("RigidBody Not Found In This Object: " + gameObject.name);
        }
    }

    public Rigidbody Rb_rigidbody
    {
        get
        {
            return rb_rigidbody;
        }

        set
        {
            rb_rigidbody = value;
        }
    }

    public virtual void HardReset()
    {
        rb_rigidbody = gameObject.GetComponent<Rigidbody>();
    }
}