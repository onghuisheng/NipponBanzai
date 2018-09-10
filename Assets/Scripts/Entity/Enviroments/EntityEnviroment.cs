using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEnviroment : Entity {

    private float
        f_lifetime,
        f_lifeElapse;

    private Vector3
        v3_size;

    private bool
        b_isStatic,
        b_willExpire;

    #region Getter/Setter
    public float F_lifetime
    {
        get
        {
            return f_lifetime;
        }

        set
        {
            f_lifetime = value;
        }
    }

    public float F_lifeElapse
    {
        get
        {
            return f_lifeElapse;
        }

        set
        {
            f_lifeElapse = value;
        }
    }

    public Vector3 V3_size
    {
        get
        {
            return v3_size;
        }

        set
        {
            v3_size = value;
        }
    }

    public bool B_isStatic
    {
        get
        {
            return b_isStatic;
        }

        set
        {
            b_isStatic = value;
        }
    }

    public bool B_willExpire
    {
        get
        {
            return b_willExpire;
        }

        set
        {
            b_willExpire = value;
        }
    }

    #endregion

    public virtual void SetUpObjectWLifeTime(float _lifetime, Vector3 _pos, Vector3 _size, bool _isStatic = true)
    {
        SetPosition(_pos);
        v3_size = _size;
        SetSize(v3_size);
        f_lifetime = _lifetime;
        b_willExpire = true;
    }

    public virtual void SetUpObject(Vector3 _pos, Vector3 _size, bool _isStatic = true)
    {
        SetPosition(_pos);
        v3_size = _size;
        SetSize(v3_size);
        b_willExpire = false;
    }

    // Use this for initialization
    protected override void Start () {
		
	}

    // Update is called once per frame
    protected override void Update () {

        base.Update();

        if (b_willExpire)
        {
            f_lifeElapse += Time.deltaTime;

            if (f_lifeElapse > f_lifetime)
            {
                gameObject.SetActive(false);
            }

        }



    }
}
