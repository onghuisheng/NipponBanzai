using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthPotion : Item
{
    private float
        f_heal_amount;

    public ItemHealthPotion()
    {
        SetUpItem();
    }

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

        Debug.Log("Before: " + _ent.St_stats.F_health + " gg " + f_heal_amount);
        _ent.St_stats.F_health += f_heal_amount;
        Debug.Log("Afterr: " + _ent.St_stats.F_health);
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
