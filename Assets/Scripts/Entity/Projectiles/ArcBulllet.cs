using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBulllet : EntityProjectiles {
    private Vector3
        v3_startPos,
        v3_endPos;

    private float
        f_lifeElapse,
        f_height,
        f_incrementor;

    private GameObject
        go_marker;

    public void SetUpProjectile(float _lifetime, float _damage, float _speed, float _height, Vector3 _start, Vector3 _end, Vector3 _size, GameObject _source, GameObject _marker)
    {
        base.SetUpProjectile(_source, _lifetime, _damage, _speed, _size, new Vector3(0, 0, 0));

        SetPosition(_start);
        f_height = _height;
        v3_startPos = _start;
        v3_endPos = _end;
        f_lifeElapse = 0;
        f_incrementor = 0;

        go_marker = _marker;
    }

    // Use this for initialization
    protected override void Start () {
		
	}

    // Update is called once per frame
    protected override void Update () {
        //f_lifeElapse += Time.deltaTime;

        //if (f_lifeElapse > F_lifetime)
        //{
        //    gameObject.SetActive(false);
        //}

        base.Update();

        //Arcs the bullet.
        if (transform.position != v3_endPos)
        {
            f_incrementor += F_speed * Time.deltaTime;
            Vector3 currentPos = Vector3.Lerp(v3_startPos, v3_endPos, f_incrementor);
            currentPos.y += f_height * Mathf.Sin(Mathf.Clamp01(f_incrementor) * Mathf.PI);
            SetPosition(currentPos);
        }
        else
        {
            //Disable the marker that is tracking the player.
            if (go_marker && go_marker.activeInHierarchy)
            {
                go_marker.SetActive(false);
                go_marker = null;

                //Setup Hitbox 
                SetUpHitBox(Go_owner.name, Go_owner.tag, Go_owner.GetInstanceID().ToString(), F_damage, GetSize() + new Vector3(2, 2, 2), GetPosition(), transform.rotation);

                //Apply Knockback
                float range = 6;
                Collider[] colliders = Physics.OverlapSphere(GetPosition(), range);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb && rb.tag.Equals("Player"))
                    {
                        rb.AddExplosionForce(15000f, GetPosition(), range * 2, 0, ForceMode.Acceleration);
                    }
                }

                //Spawn Crystal
                Crystal spawnedCrystal = ObjectPool.GetInstance().GetEnviromentObjectFromPool(ObjectPool.ENVIRONMENT.CRYSTAL).GetComponent<Crystal>();
                spawnedCrystal.SetUpObjectWLifeTime(15, gameObject.transform.position, new Vector3(3, 3, 3));

                gameObject.SetActive(false);
            }
        }
    }
}
