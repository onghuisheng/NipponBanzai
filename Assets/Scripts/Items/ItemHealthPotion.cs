using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthPotion : Item
{
    private float
        f_heal_amount;

    public override void SetUpItem()
    {
        inf_info = new Info();

        f_heal_amount = 20;

        inf_info.It_type = ITEM_TYPE.HEALTH_POTION;
        inf_info.S_name = "Health Potion";
        inf_info.S_desc = "Recovers a portion of the user's health";
    }

    public override void OnUse(EntityLivingBase _ent)
    {
        _ent.F_regen_amount += f_heal_amount;
    }

    public override void OnGround(EntityPickUps _go)
    {
        _go.DoFloating();
    }

    public override void OnTaken(EntityPickUps _go)
    {

    }

    public override void OnExpire(EntityPickUps _go)
    {

    }
}
