using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityLivingBase
{
    protected override void Start ()
    {
        base.Start();
	}

    protected override void Update ()
    {
        base.Update();
	}

    public override void OnAttack()
    {

    }

    public override void OnAttacked(DamageSource _dmgsrc)
    {

    }
}
