using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : SingletonInterface where T : MonoBehaviour
{

    private static T s_instance = null;
    private static GameObject s_instanceholder;
    private static bool s_destroyOnLoad = false;
    //[SerializeField]
    public bool m_destroyOnLoad = false;

    public static bool StaticDestroyOnLoad
    {
        set
        {
            s_destroyOnLoad = value;
            (s_instance as Singleton<T>).m_destroyOnLoad = value;
            if (null != s_instanceholder)
            {
                if (!value)
                {
                    s_instance.transform.parent = s_parentDontDestroyOnLoadObject.transform;
                }
                else
                {
                    s_instance.transform.parent = s_parentObject.transform;
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (s_instance == null)
        {
            s_instance = this as T;
            s_instanceholder = this.gameObject;
            s_instanceholder.name = typeof(T).ToString().ToUpper() + " SINGELTON";
            //s_instanceholder.tag = typeof(T).ToString().ToUpper() + " SINGELTON";
            (s_instance as Singleton<T>).m_destroyOnLoad = s_destroyOnLoad;

            if (!s_destroyOnLoad)
                s_instance.transform.parent = s_parentDontDestroyOnLoadObject.transform;
            else
                s_instance.transform.parent = s_parentObject.transform;
        }
    }

    public static T GetInstance(bool _destroyOnLoad = false)
    {
        s_destroyOnLoad = _destroyOnLoad;
        if (null == s_instance)
        {
            s_instanceholder = new GameObject();
            s_instanceholder.AddComponent<T>();
        }

        return s_instance;
    }

    public static GameObject GetObject(bool _destroyOnLoad = false)
    {
        if (null == s_instanceholder)
        {
            GetInstance(_destroyOnLoad);
        }
        return s_instanceholder;
    }

    private void DestroySelf()
    {
        if (Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }

    protected virtual void Update()
    {
        if (!m_destroyOnLoad)
        {
            transform.SetParent(s_parentDontDestroyOnLoadObject.transform);
        }
        else
        {
            transform.SetParent(s_parentObject.transform);
        }
    }
}
