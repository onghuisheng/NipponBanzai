using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private Rigidbody
        rb_rigidbody;

	protected virtual void Start ()
    {
        rb_rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    protected virtual void Update() {}

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

        Debug.Log("RigidBody Not Found In This Object: " + gameObject.name);
    }
}