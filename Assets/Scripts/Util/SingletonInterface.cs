using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonInterface : MonoBehaviour
{

    public static GameObject s_parentObject;
    public static GameObject s_parentDontDestroyOnLoadObject;
    public bool m_dontBringOverOnLoad = false;

    protected virtual void Awake()
    {
        if (null == s_parentObject)
        {
            s_parentObject = new GameObject();
            s_parentObject.name = "SINGLETONHOLDER";
        }

        if (null == s_parentDontDestroyOnLoadObject)
        {
            s_parentDontDestroyOnLoadObject = new GameObject();
            s_parentDontDestroyOnLoadObject.name = "DDOLSINGLETONHOLDER";

            if (!m_dontBringOverOnLoad)
                DontDestroyOnLoad(s_parentDontDestroyOnLoadObject);
        }
    }

}
