using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagHelper
{
	private static string[] s_tags = { "HitBox", "Non-Hitable" };

    public static bool IsTagBanned(string _tag)
    {
        foreach(string s in s_tags)
        {
            if (_tag.Equals(s))
                return true;
        }

        return false;
    }
}
