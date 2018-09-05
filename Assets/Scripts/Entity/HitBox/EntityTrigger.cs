using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityTrigger : Entity
{
    protected virtual void OnTriggerEnter(Collider other) { }

    protected void SetCollisionSize()
    {
        if(gameObject.GetComponent<BoxCollider>().size != gameObject.GetComponent<MeshFilter>().mesh.bounds.size)
         gameObject.GetComponent<BoxCollider>().size = gameObject.GetComponent<MeshFilter>().mesh.bounds.size;
    }
}
