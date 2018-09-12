using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagHelper
{
	private static string[] s_tags = { "HitBox", "Non-Hitable" };
    private static string[] s_crit_tags = { "CritBox" };

    public static bool IsTagBanned(string _tag)
    {
        foreach(string s in s_tags)
        {
            if (_tag.Equals(s))
                return true;
        }

        return false;
    }

    public static bool IsTagCritSpot(string _tag)
    {
        foreach (string s in s_crit_tags)
        {
            if (_tag.Equals(s))
                return true;
        }

        return false;
    }
}
