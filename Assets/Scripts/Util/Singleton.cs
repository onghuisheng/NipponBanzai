using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : SingletonInterface where T : MonoBehaviour
{

    private static T s_instance = null;
    private static GameObject s_instanceholder;
    
    protected override void Awake()
    {
        base.Awake();
        if (s_instance == null)
        {
            s_instance = this as T;
            s_instanceholder = gameObject;
            s_instanceholder.name = typeof(T).ToString().ToUpper() + " SINGLETON";

            if (!m_dontBringOverOnLoad)
                s_instance.transform.parent = s_parentDontDestroyOnLoadObject.transform;
            else
                s_instance.transform.parent = s_parentObject.transform;
        }
    }

    public static T GetInstance()
    {
        if (null == s_instance)
        {
            s_instanceholder = new GameObject();
            s_instanceholder.AddComponent<T>();
        }

        return s_instance;
    }
    
    protected virtual void Update()
    {
        if (!m_dontBringOverOnLoad)
        {
            transform.SetParent(s_parentDontDestroyOnLoadObject.transform);
        }
        else
        {
            transform.SetParent(s_parentObject.transform);
        }
    }
    

}
