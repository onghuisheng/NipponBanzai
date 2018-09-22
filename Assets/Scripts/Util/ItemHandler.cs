using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler
{
	public static Item GetItem(Item.ITEM_TYPE _type)
    {
        switch(_type)
        {
            case Item.ITEM_TYPE.HEALTH_POTION:
                {
                    return new ItemHealthPotion();
                }

            case Item.ITEM_TYPE.MANA_POTION:
                {
                    return new ItemManaPotion();
                }

            case Item.ITEM_TYPE.SOULS:
                {
                    return new ItemSouls();
                }
            default:
                return new ItemHealthPotion();
        }
    }
	
}
