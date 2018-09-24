using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.PostProcessing;

public class TPCamera : MonoBehaviour
{

    /*--------------------------------------------------- INITIALIZATION ---------------------------------------------------*/
    [SerializeField]
    private GameObject
        go_cross_hair, 
        go_canvas, 
        
        go_parent;
     
    private GameObject go_target;   //Current camera's target
    private EntityPlayer
        script_entityplayer;

    [SerializeField]
    private PostProcessingProfile
        ppp_profile;

    [SerializeField]
    private float
        f_zoomed,
        f_zoomed_dist,
        f_speed_of_zooming,
        f_up_distance,
        f_speed_of_rotation;       //Speed of rotation around the camera's target


    private Vector3
        v3_tremble_position,
        v3_original_position,

        v3_target_position,        //Storing of Target's position as to not call the value from other script too many times
        v3_camera_last_position;   //Storing of camera's last moved position  

    private Quaternion
        q_prev_rotation;


    private Image
        img_crosshair;

    private CanvasGroup
        cg_canvas;

    [SerializeField]
    private float
        f_Xrestrict,
        f_Xrestrict_up,

        f_tremble,
        f_tremble_recover;

    private bool
        b_is_zoomed;

    public static float f_CurrentAngle = 0;

    private void Start()
    {
        go_target = ObjectPool.GetInstance().GetEntityPlayer();
        script_entityplayer = go_target.GetComponent<EntityPlayer>();

        cg_canvas = gameObject.GetComponentInChildren<CanvasGroup>();
        img_crosshair = go_cross_hair.GetComponent<Image>();

        v3_target_position = new Vector3(go_target.transform.position.x, go_target.transform.position.y + f_up_distance, go_target.transform.position.z);  //Setting the target position to a Vector3 variable

        go_parent.transform.position = new Vector3(
              v3_target_position.x - (((go_target.transform.forward).normalized).x * 7),
              v3_target_position.y + 10,
              v3_target_position.z - (((go_target.transform.forward).normalized).z * 7));

        v3_original_position = v3_tremble_position = Vector3.zero;

        f_Xrestrict = 115;
        f_Xrestrict_up = 50;
        f_zoomed = 0;
        f_zoomed_dist = 9;
        f_speed_of_zooming = 40;
        f_up_distance = 2;

        b_is_zoomed = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /*---------------------------------------------------------------------------------------------------------------------*/


    /*------------------------------------------------------ UPDATES ------------------------------------------------------*/
    void Update()
    {
        //Camera Effect
        if (ppp_profile.vignette.enabled)
        {
            VignetteModel.Settings _setting = ppp_profile.vignette.settings;
            _setting.intensity = 1 - Time.timeScale;
            _setting.intensity = Mathf.Clamp(_setting.intensity, 0.1f, 1.0f);
            ppp_profile.vignette.settings = _setting;
        }

        if (ppp_profile.colorGrading.enabled)
        {
            ColorGradingModel.Settings _setting = ppp_profile.colorGrading.settings;
            _setting.basic.saturation = (script_entityplayer.St_stats.F_health / script_entityplayer.St_stats.F_max_health);
            _setting.basic.saturation = Mathf.Clamp(_setting.basic.saturation, 0.1f, 1.0f);
            ppp_profile.colorGrading.settings = _setting;
        }
    }

    void LateUpdate()
    {
        if (script_entityplayer != null)
        {
            switch (script_entityplayer.GetPlayerTargetState())
            {
                case EntityPlayer.TARGET_STATE.AIMING:
                    {
                        if (!script_entityplayer.An_animator.GetBool("IsMelee"))
                        {
                            if (cg_canvas != null)
                            {
                                if (cg_canvas.alpha < 1)
                                    cg_canvas.alpha += 0.05f;

                                RaycastHit hit;
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(ray, out hit))
                                {
                                    if (hit.collider != null && hit.collider.gameObject.GetComponent<EntityLivingBase>() != null)
                                    {
                                        if (go_cross_hair.transform.localScale.x > 0.5f)
                                        {
                                            float _speed = 5;
                                            go_cross_hair.transform.eulerAngles = new Vector3(go_cross_hair.transform.eulerAngles.x, go_cross_hair.transform.eulerAngles.y, go_cross_hair.transform.eulerAngles.z + _speed);
                                            float _temp = 0.5f / (45 / _speed);
                                            go_cross_hair.transform.localScale = new Vector3(go_cross_hair.transform.localScale.x - _temp, go_cross_hair.transform.localScale.y - _temp, go_cross_hair.transform.localScale.z);
                                            //img_crosshair.color = new Color(img_crosshair.color.r + _temp, img_crosshair.color.g, img_crosshair.color.b, img_crosshair.color.a);

                                            //Debug.Log("Sizing Down");
                                        }
                                    }
                                    else
                                    {
                                        if (go_cross_hair.transform.localScale.x < 1)
                                        {
                                            float _speed = 5;
                                            go_cross_hair.transform.eulerAngles = new Vector3(go_cross_hair.transform.eulerAngles.x, go_cross_hair.transform.eulerAngles.y, go_cross_hair.transform.eulerAngles.z - _speed);
                                            float _temp = 0.5f / (45 / _speed);
                                            go_cross_hair.transform.localScale = new Vector3(go_cross_hair.transform.localScale.x + _temp, go_cross_hair.transform.localScale.y + _temp, go_cross_hair.transform.localScale.z);
                                            // img_crosshair.color = new Color(img_crosshair.color.r - _temp, img_crosshair.color.g, img_crosshair.color.b, img_crosshair.color.a);

                                            //Debug.Log("Sizing Up");
                                        }
                                    }
                                }
                                else
                                {
                                    if (go_cross_hair.transform.localScale.x < 1)
                                    {
                                        float _speed = 3;
                                        go_cross_hair.transform.eulerAngles = new Vector3(go_cross_hair.transform.eulerAngles.x, go_cross_hair.transform.eulerAngles.y, go_cross_hair.transform.eulerAngles.z - _speed);
                                        float _temp = 0.5f / (45 / _speed);
                                        go_cross_hair.transform.localScale = new Vector3(go_cross_hair.transform.localScale.x + _temp, go_cross_hair.transform.localScale.y + _temp, go_cross_hair.transform.localScale.z);
                                        img_crosshair.color = new Color(img_crosshair.color.r - _temp, img_crosshair.color.g, img_crosshair.color.b, img_crosshair.color.a);

                                        //Debug.Log("Sizing Up");
                                    }
                                }
                            }

                            if (f_zoomed < f_zoomed_dist)
                            {
                                f_zoomed += (f_speed_of_zooming * Time.unscaledDeltaTime);
                                transform.position -= (transform.position - (v3_target_position + (go_parent.transform.right.normalized * 2))).normalized * (f_speed_of_zooming * Time.unscaledDeltaTime);  //Performing the Zooming in feature by relying on the Mouse scroll wheel input, speed of zooming is customizable

                                if (f_zoomed > f_zoomed_dist)
                                {
                                    float minusor = f_zoomed - f_zoomed_dist;

                                    f_zoomed -= minusor;
                                    transform.position += (transform.position - (v3_target_position + (go_parent.transform.right.normalized * 2))).normalized * minusor;  //Performing the Zooming in feature by relying on the Mouse scroll wheel input, speed of zooming is customizable
                                }

                                q_prev_rotation = transform.rotation;
                                b_is_zoomed = true;
                            }

                            //transform.LookAt((v3_target_position + (transform.right.normalized * 2)));
                            go_parent.transform.LookAt((v3_target_position + (transform.right.normalized * 2)));


                            // Debug.Log(transform.position.y);

                            if (go_parent.transform.position.y > -8f && go_parent.transform.position.y < 12.0f)
                            {
                                go_parent.transform.RotateAround(v3_target_position, go_parent.transform.up, 30 * Time.unscaledDeltaTime * (f_speed_of_rotation * Input.GetAxis("Mouse X")));   //Rotating the camera around the target's position, with customizable rotation speed
                            }
                            go_parent.transform.RotateAround(v3_target_position, go_parent.transform.right, 30 * Time.unscaledDeltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed

                            Vector3 cubeTOCam = transform.position - v3_target_position;
                            Vector3 camToCube = -cubeTOCam;
                            camToCube.y = transform.position.y;

                            // Debug.Log(Vector3.Angle(cubeTOCam, camToCube));

                            if (Vector3.Angle(cubeTOCam, camToCube) < f_Xrestrict - f_Xrestrict_up && gameObject.transform.position.y > v3_target_position.y)
                            {
                                if (-Input.GetAxis("Mouse Y") > 0)
                                {
                                    if (gameObject.transform.position.y > v3_target_position.y)
                                    {
                                        go_parent.transform.RotateAround(v3_target_position, transform.right, -30 * Time.unscaledDeltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed
                                    }
                                }
                            }
                            else if (Vector3.Angle(cubeTOCam, camToCube) < f_Xrestrict && gameObject.transform.position.y < v3_target_position.y)
                            {
                                if (-Input.GetAxis("Mouse Y") < 0)
                                {
                                    if (gameObject.transform.position.y < v3_target_position.y)
                                    {
                                        go_parent.transform.RotateAround(v3_target_position, transform.right, -30 * Time.unscaledDeltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed
                                    }
                                }
                            }

                            Vector3 forward = go_parent.transform.forward;
                            Vector3 toOther = transform.position - go_parent.transform.position;

                            if (Vector3.Dot(forward, toOther) < 0)
                            {
                                transform.position = go_parent.transform.position;
                            }

                            //Debug.Log("Child position: " + transform.position);
                            //Debug.Log("Parent position: " + go_parent.transform.position);
                        }
                        break;
                    }

                case EntityPlayer.TARGET_STATE.NOT_AIMING:
                    {

                        if (cg_canvas != null)
                        {
                            if (cg_canvas.alpha > 0)
                            {
                                cg_canvas.alpha -= 0.05f;
                            }
                        }


                        float distance = 20.0f;

                        RaycastHit hit;

                        Vector3 camPos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);

                        if (Physics.Raycast(v3_target_position, (v3_target_position - camPos), out hit, distance))
                        {
                            if (q_prev_rotation != transform.rotation || b_is_zoomed)
                            {
                                if (f_zoomed > 0)
                                    f_zoomed -= (f_speed_of_zooming * Time.unscaledDeltaTime);
                                transform.position += (transform.position - v3_target_position).normalized * (f_speed_of_zooming * Time.unscaledDeltaTime);  //Performing the Zooming in feature by relying on the Mouse scroll wheel input, speed of zooming is customizable                              

                                Vector3 forward = go_parent.transform.forward;
                                Vector3 toOther = transform.position - go_parent.transform.position;

                                if (Vector3.Dot(forward, toOther) < 0 || Vector3.Distance(go_parent.transform.position, transform.position) > 9)
                                {
                                    transform.position = go_parent.transform.position;
                                }

                                //Debug.Log("Child position: " + transform.position);
                                //Debug.Log("Parent position: " + go_parent.transform.position);

                                if (f_zoomed < 0)
                                {
                                    float plusor = -f_zoomed;
                                    f_zoomed += plusor;
                                }
                            }
                        }
                        else
                        {
                            if (f_zoomed < f_zoomed_dist)
                            {
                                b_is_zoomed = false;
                                f_zoomed += f_speed_of_zooming * Time.unscaledDeltaTime;
                                transform.position -= (transform.position - v3_target_position).normalized * (f_speed_of_zooming * Time.unscaledDeltaTime);  //Performing the Zooming in feature by relying on the Mouse scroll wheel input, speed of zooming is customizable
                            }

                            if (f_zoomed > f_zoomed_dist)
                            {
                                float minusor = f_zoomed - f_zoomed_dist;

                                f_zoomed -= minusor;
                                transform.position += (transform.position - (v3_target_position + (go_parent.transform.right.normalized * 2))).normalized * minusor;  //Performing the Zooming in feature by relying on the Mouse scroll wheel input, speed of zooming is customizable
                            }

                            q_prev_rotation = transform.rotation;
                        }

                        //transform.LookAt(v3_target_position);
                        go_parent.transform.LookAt(v3_target_position);

                        go_parent.transform.RotateAround(v3_target_position, go_parent.transform.up, 30 * Time.unscaledDeltaTime * (f_speed_of_rotation * Input.GetAxis("Mouse X")));   //Rotating the camera around the target's position, with customizable rotation speed
                        go_parent.transform.RotateAround(v3_target_position, go_parent.transform.right, 30 * Time.unscaledDeltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed

                        Vector3 cubeTOCam = transform.position - v3_target_position;
                        Vector3 camToCube = -cubeTOCam;
                        camToCube.y = transform.position.y;

                        // Debug.Log(Vector3.Angle(cubeTOCam, camToCube));

                        if (Vector3.Angle(cubeTOCam, camToCube) < f_Xrestrict - f_Xrestrict_up && gameObject.transform.position.y > v3_target_position.y)
                        {
                            if (-Input.GetAxis("Mouse Y") > 0)
                            {
                                if (gameObject.transform.position.y > v3_target_position.y)
                                {
                                    go_parent.transform.RotateAround(v3_target_position, transform.right, -30 * Time.unscaledDeltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed
                                }
                            }
                        }
                        else if (Vector3.Angle(cubeTOCam, camToCube) < f_Xrestrict && gameObject.transform.position.y < v3_target_position.y)
                        {
                            if (-Input.GetAxis("Mouse Y") < 0)
                            {
                                if (gameObject.transform.position.y < v3_target_position.y)
                                {
                                    go_parent.transform.RotateAround(v3_target_position, transform.right, -30 * Time.unscaledDeltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            Vector3 angle = go_parent.transform.rotation.eulerAngles;
            angle.z = 0;
            go_parent.transform.rotation = Quaternion.Euler(angle);

            v3_camera_last_position = transform.position;   //Saving the Camera's last position into a Vect or3 variable

            Vector3 new_target_pos = new Vector3(go_target.transform.position.x, go_target.transform.position.y + f_up_distance, go_target.transform.position.z);

            if (v3_target_position != new_target_pos)
            {
                go_parent.transform.position -= v3_target_position - new_target_pos;

                v3_target_position = new_target_pos; //Moving the target as well as to ensure proper rotation
            }
        }

        Vector3 dir = script_entityplayer.transform.position - transform.position;
        f_CurrentAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        if (f_tremble > 0)
        {
            //f_tremble -= Time.deltaTime;

            //f_tremble = Mathf.Clamp(f_tremble, 0, Mathf.Infinity);

            //transform.DOShakePosition(f_tremble, f_tremble);

        //    if (v3_original_position.Equals(Vector3.zero))
        //    {
        //        v3_tremble_position = transform.position;
        //        v3_original_position = transform.position;
        //    }

            //    f_tremble -= Time.deltaTime;

            //    if (f_tremble <= 0)
            //        f_tremble = 0;

            //    if(v3_tremble_position.Equals(transform.position))
            //        v3_tremble_position = v3_original_position + new Vector3(Random.Range(-f_tremble, f_tremble), Random.Range(-f_tremble, f_tremble), Random.Range(-f_tremble, f_tremble));
            //    else
            //    {
            //        transform.position = Vector3.Lerp(transform.position, v3_tremble_position, 0.5f);
            //    }
            //}
            //else
            //{

            //}

        }
    }
    /*----------------------------------------------------------------------------------------------------------------------*/

    public void ShakeCamera(float _damage)
    {
        f_tremble += _damage * 0.1f;
        //go_parent.transform.DOComplete();
        //go_parent.transform.DOShakeRotation(f_tremble, f_tremble);
    }
}