using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMelee : EntityEnemy
{

    protected override void Start()
    {
        base.Start();

        ClearAITask();

        St_stats.F_max_health = 50;
        St_stats.F_health = St_stats.F_max_health;
        St_stats.F_speed = 3;
        St_stats.F_defence = 20.0f;
        St_stats.F_damage = 2.0f;
        St_stats.F_mass = 2.0f;

        B_isHit = false;

        An_animator.Rebind();
        // RegisterAITask(new AIIdle(2, this));
        RegisterAITask(new AIAttackMelee(2, this, typeof(EntityPlayer), 3));
        RegisterAITask(new AIChase(1, this, typeof(EntityPlayer), 20, 90));
        //RegisterAITask(new AIRoam(3, this, 5.0f));

        GetComponent<Collider>().isTrigger = false;

        St_stats.S_name = "MeleeDude";
    }

    protected override void Update()
    {
        if (!IsDead())
        {
            base.Update();
        }
        else
        {
            F_death_timer += Time.deltaTime;

            if (!An_animator.GetBool("Dead"))
            {
                EndAndClearAITask();
                An_animator.SetBool("Dead", true);
                GetComponent<Collider>().isTrigger = true;
            }

            if (F_death_timer > 5.0f)
            {
                gameObject.SetActive(false);

                // GameObject go = ObjectPool.GetInstance().GetItemObjectFromPool();
                // go.GetComponent<EntityPickUps>().SetPosition(GetPosition());
            }
        }
    }

    public override void OnAttack()
    {
        SetUpHitBox(gameObject.name, gameObject.tag, gameObject.GetInstanceID().ToString(), St_stats.F_damage, Vector3.one, transform.position + (transform.forward * GetComponent<Collider>().bounds.extents.magnitude), transform.rotation);
    }

    public override void HardReset()
    {
        Start();
    }

    public override void OnAttacked(DamageSource _dmgsrc)
    {
        if (St_stats.F_health <= 0)
            return;

        base.OnAttacked(_dmgsrc);
        St_stats.F_health -= _dmgsrc.GetDamage();
        Debug.Log("OUCH, KENA HIT NOW MY HP IS " + St_stats.F_health + " DAMAGE DEALT IS " + _dmgsrc.GetDamage());
        An_animator.SetTrigger("Injured");
    }

}