using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : EntityTrigger {

    private float
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
            if (!other.CompareTag(dmgs_damageObj.GetSourceTag()) && !other.CompareTag(gameObject.tag) && !TagHelper.IsTagBanned(other.tag))
            {
                if (other.gameObject.GetComponent<EntityLivingBase>() != null)
                {
                    other.gameObject.GetComponent<EntityLivingBase>().OnAttacked(dmgs_damageObj);
                }
            }
        }
    }

    public void SetHitbox(DamageSource _dmgsrc, Vector3 _size, float _timer = 1.0f)
    {
        f_timer = _timer;
        dmgs_damageObj = _dmgsrc;

        SetSize(_size);
    }
}
