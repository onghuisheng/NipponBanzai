using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    private int
        i_souls;

    private Dictionary<Item.ITEM_TYPE, int>
        dic_storage;

    private List<SkillBase>
        list_skills_inventory;

    private int
        f_curr_skill;

	public void SetUpInventory()
    {
        dic_storage = new Dictionary<Item.ITEM_TYPE, int>();
        list_skills_inventory = new List<SkillBase>();
        i_souls = 0;
        f_curr_skill = 0;
    }	

    public Dictionary<Item.ITEM_TYPE, int> GetInventoryContainer()
    {
        if(dic_storage != null)
            return dic_storage;
        return null;
    }

    public int GetSouls()
    {
        return i_souls;
    }

    public void AdjustSoulsAmount(int _amount)      //parameter - _amount(int): to increase/decrease value of current souls
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

    public void UseItemInInventory(EntityLivingBase _owner, Item.ITEM_TYPE _itemid)
    {
        if(dic_storage.ContainsKey(_itemid))
        {
            if (dic_storage[_itemid] > 0)
            {
                if (ItemHandler.GetItem(_itemid).OnUse(_owner))
                    --dic_storage[_itemid];
            }
        }
    }

    public SkillBase GetNextSkill(bool _getRight)
    {
        if (list_skills_inventory.Count <= 0)
            return null;

        if(f_curr_skill > list_skills_inventory.Count - 1)
        {
            f_curr_skill = list_skills_inventory.Count - 1;
        }

        if (_getRight)
            ++f_curr_skill;
        else
            --f_curr_skill;

        if (f_curr_skill > list_skills_inventory.Count - 1)
            f_curr_skill = 0;
        else if (f_curr_skill < 0)
            f_curr_skill = list_skills_inventory.Count - 1;


        return list_skills_inventory[f_curr_skill];
    }

    public SkillBase GetCurrSkill()
    {
        if (f_curr_skill < 0 || f_curr_skill > list_skills_inventory.Count - 1)
            return null;

        return list_skills_inventory[f_curr_skill];
    }

    public List<SkillBase> GetAllSkills()
    {
        return list_skills_inventory;
    }
}
