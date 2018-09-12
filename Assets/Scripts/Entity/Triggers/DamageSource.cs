using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageSource {

    private string
        s_name,
        s_source_id, 
        s_tag, 
        s_attacked_tag;

    private float
        f_damage;

    public void SetUpDamageSource(string _name, string _tag, string _id, float _damage)
    {
        s_name = _name;
        s_source_id = _id;
        f_damage = _damage;
        s_tag = _tag;
    }

    public string GetName()
    {
        return s_name;
    }

    public string GetSourceID()
    {
        return s_source_id;
    }

    public string GetSourceTag()
    {
        return s_tag;
    }

    public string GetAttackedTag()
    {
        return s_attacked_tag;
    }

    public void SetAttackedTag(string _tag)
    {
        s_attacked_tag = _tag;
    }

    public float GetDamage()
    {
        return f_damage;
    }   
}
