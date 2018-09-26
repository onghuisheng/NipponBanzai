using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSwordSummoning : SkillBase
{
    private List<GameObject>
        list_swords;

    private bool
        b_spawn_sword;

    private EntityLivingBase
        go_target;

    private int
        i_prev_list_count;

    private Vector3[]
        ar_postion = new Vector3[5] 
        {
            new Vector3(0, 4, -3),
            new Vector3(2, 3, -2),
            new Vector3(-2, 3, -2),
            new Vector3(4, 2, -1),           
            new Vector3(-4, 2, -1)
        };

    public override void SetUpSkill()
    {
        s_name = "Swords of Revealing Light";
        s_description = "Summons a number of swords based on your mana";

        type_style = TYPE.OFFENCE;
        f_mana_amount = 50;
        i_id = 1;

        f_cooldown = 0;
        f_maxcooldown = 10;
        f_timer = 0;

        list_swords = new List<GameObject>();
        b_spawn_sword = false;
    }

    public override void StartSkill(EntityLivingBase _caster, float _manaused)
    {
        go_caster = _caster;
        f_mana_amount_used = _manaused;

        if(list_swords.Count > 0)
        {
            foreach (GameObject go in list_swords)
                go.SetActive(false);

            b_spawn_sword = false;
        }

        go_target = null;
    }

    public override void UpdateSkill()
    {
        base.UpdateSkill();

        if (b_spawn_sword)
        {
            for (; list_swords.Count < (int)(f_mana_amount_used * 0.1f);)
            {
                float _temp = (f_mana_amount_used * 0.1f);
                GameObject _proj = ObjectPool.GetInstance().GetProjectileObjectFromPool(ObjectPool.PROJECTILE.SWORD_PROJECTILE);
                _proj.GetComponent<Sword_Projectile>().SetUpProjectile(go_caster.gameObject, new Vector3(go_caster.GetPosition().x, go_caster.GetPosition().y + 5, go_caster.GetPosition().z), Vector3.one, 50, f_mana_amount_used, 50, new Vector3((_temp * 0.5f) * 0.3f, (_temp * 0.5f) * 0.3f, (_temp * 0.5f) * 0.3f));
                list_swords.Add(_proj);
            }

            b_spawn_sword = false;
        }

        for(int i = list_swords.Count - 1; i >= 0; --i)
        {
            if(!list_swords[i].activeSelf)
            {
                list_swords.Remove(list_swords[i]);
                continue;
            }

            Sword_Projectile _temp = list_swords[i].GetComponent<Sword_Projectile>();

            if (!_temp.HasTarget())
            {
                Vector3 _newpos = go_caster.GetPosition();
                //forward
                _newpos += go_caster.Rb_rigidbody.transform.forward * ar_postion[i].z;
                //right
                _newpos += go_caster.Rb_rigidbody.transform.right * ar_postion[i].x;
                //up
                _newpos += go_caster.Rb_rigidbody.transform.up * ar_postion[i].y;


                list_swords[i].transform.position = Vector3.Lerp(list_swords[i].transform.position, _newpos, 0.1f);

                list_swords[i].transform.localEulerAngles = Vector3.Lerp(list_swords[i].transform.localEulerAngles, Vector3.zero, 0.1f);
            }
        }

        if (go_target != null)
        {
            if (go_target.IsDead() || !go_target.gameObject.activeSelf || i_prev_list_count != list_swords.Count)
                go_target = null;
        }

        if (go_target == null && go_caster != null)
        {
            if (list_swords.Count > 0)
            {
                foreach (GameObject go in ObjectPool.GetInstance().GetAllActiveInSurrounding(go_caster.transform.position, 10, typeof(EntityLivingBase)))
                {
                    if (!go.CompareTag(go_caster.tag))
                    {
                        go_target = go.GetComponent<EntityLivingBase>();
                        if (go_target.B_isAIEnabled && !go_target.IsDead())
                        {
                            list_swords[Random.Range(0, list_swords.Count - 1)].GetComponent<Sword_Projectile>().SetTarget(go_target);
                            break;
                        }
                    }
                }
            }
        }

        i_prev_list_count = list_swords.Count;
    }

    public override void RunSkill()
    {
        f_timer += Time.deltaTime;

        if (f_timer > 1)
        {
            go_caster.An_animator.SetBool("IsSummoning", false);
            b_spawn_sword = true;
        }
    }

    public override void EndSkill()
    {
        f_cooldown = f_maxcooldown;
        f_timer = 0;
        OnSkillEnd();
    }
}
