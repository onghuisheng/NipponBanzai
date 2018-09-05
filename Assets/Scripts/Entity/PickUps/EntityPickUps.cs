using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPickUps : Entity
{
    public enum Items
    {
        HEALTH_POTION = 0,
        MANA_POTION
    }

    private float
       f_lifetime;

    private Items
        i_id;

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

    protected override void Start()
    {
        //list_mesh = new List<Mesh>();
        //list_texture = new List<Texture>();

        mf_meshfilter = GetComponent<MeshFilter>();
        mr_meshrenderer = GetComponent<MeshRenderer>();
    }

    public virtual void SetUpPickUp(float _lifetime, Items _id)
    {
        f_lifetime = _lifetime;
        i_id = _id;

        if(list_mesh.Capacity > (int)_id && list_texture.Capacity > (int)_id)
        {
            if(mf_meshfilter != null && mr_meshrenderer != null)
            {
                mf_meshfilter.mesh = list_mesh[(int)_id];
                mr_meshrenderer.material.mainTexture = list_texture[(int)_id];
            }
        }
    }
}
