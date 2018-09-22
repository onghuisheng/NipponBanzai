using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public enum ITEM_TYPE
    {
        HEALTH_POTION = 0,
        MANA_POTION,
        SOULS,

        TOTAL
    }

    public class Info
    {
        private ITEM_TYPE
        it_type;

        private string
            s_name,
            s_desc;

        public ITEM_TYPE It_type
        {
            get
            {
                return it_type;
            }

            set
            {
                it_type = value;
            }
        }

        public string S_name
        {
            get
            {
                return s_name;
            }

            set
            {
                s_name = value;
            }
        }

        public string S_desc
        {
            get
            {
                return s_desc;
            }

            set
            {
                s_desc = value;
            }
        }
    }

    protected Info
        inf_info;

    public Info GetInfo()
    {
        return inf_info;
    }

    public abstract void SetUpItem();

    public abstract bool OnUse(EntityLivingBase _ent);

    public abstract void OnGround(EntityPickUps _go);

    public abstract void OnTaken(EntityPickUps _go, EntityLivingBase _taker);

    public abstract void OnExpire(EntityPickUps _go);
}
