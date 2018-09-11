using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEnemy : EntityLivingBase {
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (F_death_timer > 5.0f)
        {
           ObjectPool.GetInstance().GetItemObjectFromPool().GetComponent<EntityPickUps>().SetPosition(GetPosition());
        }
    }

    public virtual void HardReset()
    {
    }

}
