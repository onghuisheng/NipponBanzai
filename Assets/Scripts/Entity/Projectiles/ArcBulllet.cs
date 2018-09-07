using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBulllet : EntityProjectiles {
    private Vector3
        v3_startPos,
        v3_endPos;

    private float
        f_life,
        f_height,
        f_incrementor;

    private GameObject
        go_marker;

    public void SetUpProjectile(float _lifetime, float _damage, float _speed, float _height, Vector3 _start, Vector3 _end, Vector3 _size, GameObject _marker)
    {
        base.SetUpProjectile(_lifetime,_damage, _speed, _size);

        f_height = _height;
        v3_startPos = _start;
        v3_endPos = _end;
        f_life = 0;
        f_incrementor = 0;

        go_marker = _marker;
    }

    // Use this for initialization
    protected override void Start () {
		
	}

    // Update is called once per frame
    protected override void Update () {
        f_life += Time.deltaTime;

        if (f_life > F_lifetime)
        {
            gameObject.SetActive(false);
        }
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
                Debug.Log("BB MARKER");
                go_marker.SetActive(false);
                go_marker = null;
            }
        }
    }
}
