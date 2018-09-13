using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityTrigger : Entity
{
    protected virtual void OnTriggerEnter(Collider other) { }

    protected virtual void OnTriggerStay(Collider other) { }

}
