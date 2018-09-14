using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    private int
        i_souls;

    private Dictionary<Item.ITEM_TYPE, int>
        dic_storage;

	public void SetUpInventory()
    {
        dic_storage = new Dictionary<Item.ITEM_TYPE, int>();
        i_souls = 0;
    }	

    public Dictionary<Item.ITEM_TYPE, int> GetInventory()
    {
        if(dic_storage != null)
            return dic_storage;
        return null;
    }

    public int GetSouls()
    {
        return i_souls;
    }

    public void AddSouls(int _amount)
    {
        i_souls += _amount;
        Mathf.Clamp(i_souls, 0, int.MaxValue);
    }

    public void AddItemToInventory(Item.ITEM_TYPE _item_to_store, int _amount)
    {
        if(!dic_storage.ContainsKey(_item_to_store))
        {
            dic_storage.Add(_item_to_store, _amount);
            Mathf.Clamp(dic_storage[_item_to_store], 0, int.MaxValue);
        }
        else
        {
            dic_storage[_item_to_store] += _amount;
            Mathf.Clamp(dic_storage[_item_to_store], 0, int.MaxValue);
        }
    }
}
