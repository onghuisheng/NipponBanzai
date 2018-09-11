using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthPotion : Item
{

    public override void SetUpItem()
    {
        inf_info = new Info();

        inf_info.It_type = ITEM_TYPE.HEALTH_POTION;
        inf_info.S_name = "Health Potion";
        inf_info.S_desc = "Recovers a portion of the user's health";
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
