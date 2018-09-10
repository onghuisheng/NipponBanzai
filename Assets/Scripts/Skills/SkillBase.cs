using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public enum TYPE
    {
        OFFENCE,
        DEFENCE
    }

    private string
        s_name,
        s_description;

    private float
        f_mana_amount;

    private TYPE
        type_style;

    public virtual void SetUpSkill(string _name, string _description, float _mana_amount, TYPE _type)
    {
        s_name = _name;
        s_description = _description;
        f_mana_amount = _mana_amount;
        type_style = _type;
    }

	public virtual void RunSkill ()
    {
		
	}
}
