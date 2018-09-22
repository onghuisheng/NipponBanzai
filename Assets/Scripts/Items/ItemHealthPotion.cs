using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthPotion : Item
{
    private static float
        f_heal_amount;

    public override void SetUpItem()
    {
        inf_info = new Info();

        f_heal_amount = 20;

        inf_info.It_type = ITEM_TYPE.HEALTH_POTION;
        inf_info.S_name = "Health Potion";
        inf_info.S_desc = "Recovers a portion of the user's health";
    }

    public override bool OnUse(EntityLivingBase _ent)
    {
        if (_ent.St_stats.F_health >= _ent.St_stats.F_max_health)
            return false;

        _ent.F_regen_amount += f_heal_amount;
        return true;
    }

    public override void OnGround(EntityPickUps _go)
    {
        _go.DoFloating();
    }

    public override void OnTaken(EntityPickUps _go, EntityLivingBase _taker)
    {
        _taker.GetInventory().AddItemToInventory(_go.CurrentItem().GetInfo().It_type, 1);
    }

    public override void OnExpire(EntityPickUps _go)
    {

    }
}
