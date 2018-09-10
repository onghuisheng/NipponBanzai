using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class AnimatorExtensions
{

    public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
    {
        var parameters = self.parameters;
        foreach (var currParam in parameters)
        {
            if (currParam.type == type && currParam.name == name)
            {
                return true;
            }
        }
        return false;
    }

}