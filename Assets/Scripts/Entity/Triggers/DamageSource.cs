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

    private bool
        b_isFlinching;

    private Vector3
        v3_position;

    public void SetUpDamageSource(string _name, string _tag, string _id, float _damage, bool _flinch = true)
    {
        s_name = _name;
        s_source_id = _id;
        f_damage = _damage;
        s_tag = _tag;
        b_isFlinching = _flinch;

        v3_position = Vector3.zero;
    }

    public void SetPosition(Vector3 _pos)
    {
        v3_position = _pos;
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

    public Vector3 GetPosition()
    {
        return v3_position;
    }

    public bool IsFlinching()
    {
        return b_isFlinching;
    }

}
