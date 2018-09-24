using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSouls : Item
{
    public override void SetUpItem()
    {
        inf_info = new Info();

        inf_info.It_type = ITEM_TYPE.SOULS;
        inf_info.S_name = "Souls";
        inf_info.S_desc = "Souls of the enemy you killed";
    }

    public override bool OnUse(EntityLivingBase _ent)
    {
       
        return true;
    }

    public override void OnGround(EntityPickUps _go)
    {
        _go.DoFloating();
        _go.DoSucking(5, 5f);
        _go.DoSpawnParticle(ParticleHandler.ParticleType.Souls, Vector3.zero, Vector3.one, new Vector3(-90, 0, 0));
    }

    public override void OnTaken(EntityPickUps _go, EntityLivingBase _taker)
    {
        _taker.GetInventory().AdjustSoulsAmount(1);
    }

    public override void OnExpire(EntityPickUps _go)
    {

    }
}

