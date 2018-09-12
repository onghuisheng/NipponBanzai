using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : EntityTrigger {

    private float
        f_iframe_timer,
        f_timer;            //How long the hitbox stays in the world

    private DamageSource
        dmgs_damageObj;     //Damage source

    protected override void Start()
    {
        f_timer = 1;
        dmgs_damageObj = new DamageSource();

        //Material material = new Material(Shader.Find("Custom/Transparent"));
        //GetComponent<Renderer>().material = material;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (f_timer > 0)
            f_timer -= Time.deltaTime;
        else
            gameObject.SetActive(false);
        
	}

    protected override void OnTriggerEnter(Collider other)
    {
        if (dmgs_damageObj.GetSourceTag() != null)
        {
            EntityLivingBase _ent = null;

            if (!other.CompareTag(dmgs_damageObj.GetSourceTag()) && !other.CompareTag(gameObject.tag) && !TagHelper.IsTagBanned(other.tag))
            {
                _ent = other.gameObject.GetComponent<EntityLivingBase>();

                if (_ent != null)
                {
                    dmgs_damageObj.SetAttackedTag(other.gameObject.tag);
                    _ent.OnAttacked(dmgs_damageObj);
                }
                else
                {
                    _ent = other.gameObject.GetComponentInParent<EntityLivingBase>();

                    if (_ent != null)
                    {
                        dmgs_damageObj.SetAttackedTag(other.gameObject.tag);
                        _ent.OnAttacked(dmgs_damageObj, f_iframe_timer);
                    }
                }
            }
        }
    }

    public void SetHitbox(DamageSource _dmgsrc, Vector3 _size, float _timer = 1.0f, float _iframe_timer = 0.3f)
    {
        f_timer = _timer;
        f_iframe_timer = _iframe_timer;
        dmgs_damageObj = _dmgsrc;

        SetSize(_size);
    }
}
