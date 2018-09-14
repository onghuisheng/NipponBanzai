using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPickUps : Entity
{
    private float
       f_lifetime,
       f_max_lifetime,

       f_floating_offset,
       f_floating_speed,

       f_rotation_speed;

    private Item
        i_id;

    private Vector3
        v3_original_pos;

    [SerializeField]
    private List<Mesh>
        list_mesh;

    [SerializeField]
    private List<Texture>
       list_texture;

    private MeshFilter
        mf_meshfilter;

    private MeshRenderer
        mr_meshrenderer;

    public virtual void SetUpPickUp(Vector3 _pos, float _lifetime, Item _id)
    {
        if (mf_meshfilter == null)
        {
            mf_meshfilter = GetComponent<MeshFilter>();
            mr_meshrenderer = GetComponent<MeshRenderer>();
        }

        f_max_lifetime = f_lifetime = _lifetime;
        i_id = _id;
        _id.SetUpItem();

        f_floating_offset = 0.5f;
        f_floating_speed = 0.1f;
        f_rotation_speed = 3;

        v3_original_pos = _pos;
        SetPosition(v3_original_pos);

        if (list_mesh.Capacity > (int)_id.GetInfo().It_type && list_texture.Capacity > (int)_id.GetInfo().It_type)
        {
            if(mf_meshfilter != null && mr_meshrenderer != null)
            {
                mf_meshfilter.mesh = list_mesh[(int)_id.GetInfo().It_type];
                mr_meshrenderer.material.mainTexture = list_texture[(int)_id.GetInfo().It_type];
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (f_lifetime > 0)
        {
            f_lifetime -= Time.deltaTime;
           
            i_id.OnGround(this);
        }
        else
        {
            i_id.OnExpire(this);
            gameObject.SetActive(false);
        }

        if(Vector3.Distance(GetPosition(), ObjectPool.GetInstance().GetEntityPlayer().GetComponent<EntityPlayer>().GetPosition()) < 2)
        {
            i_id.OnTaken(this);
            gameObject.SetActive(false);
        }
    }

    public void DoFloating()
    {
        if(GetPosition().y > v3_original_pos.y + f_floating_offset)
        {
            f_floating_speed = -f_floating_speed;
        }
        else if (GetPosition().y < v3_original_pos.y)
        {
            f_floating_speed = -f_floating_speed;
        }

        SetPosition(new Vector3(GetPosition().x, GetPosition().y + f_floating_speed * Time.deltaTime, GetPosition().z));
        transform.Rotate(0, f_rotation_speed * Time.deltaTime, 0);
    }
}
