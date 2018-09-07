using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TPCamera : MonoBehaviour
{

    /*--------------------------------------------------- INITIALIZATION ---------------------------------------------------*/
    private GameObject go_target;   //Current camera's target
    private EntityPlayer
        script_entityplayer;

    [SerializeField]
    private float
        f_zoomed,
        f_zoomed_dist,
        f_speed_of_zooming,
        f_up_distance,
        f_speed_of_rotation;       //Speed of rotation around the camera's target


    private Vector3
        v3_target_position,        //Storing of Target's position as to not call the value from other script too many times
        v3_camera_last_position;   //Storing of camera's last moved position  

    private Quaternion
        q_prev_rotation;

    [SerializeField]
    private float
        f_Xrestrict;

    private bool
        b_is_zoomed;

    private void Start()
    {
        go_target = ObjectPool.GetInstance().GetEntityPlayer();
        script_entityplayer = go_target.GetComponent<EntityPlayer>();

        v3_target_position = new Vector3(go_target.transform.position.x, go_target.transform.position.y + f_up_distance, go_target.transform.position.z);  //Setting the target position to a Vector3 variable

        transform.position = new Vector3(
              v3_target_position.x - (((go_target.transform.forward).normalized).x * 7),
              v3_target_position.y + 10,
              v3_target_position.z - (((go_target.transform.forward).normalized).z * 7));

        f_Xrestrict = 140;
        f_zoomed = 0;
        f_zoomed_dist = 8;
        f_speed_of_zooming = 10;
        f_up_distance = 2;

        b_is_zoomed = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /*---------------------------------------------------------------------------------------------------------------------*/


    /*------------------------------------------------------ UPDATES ------------------------------------------------------*/
    void LateUpdate()
    {
        if (script_entityplayer != null)
        {
            switch (script_entityplayer.GetPlayerTargetState())
            {
                case EntityPlayer.TARGET_STATE.AIMING:
                    {
                        if (f_zoomed < f_zoomed_dist)
                        {
                            f_zoomed += (f_speed_of_zooming * Time.deltaTime);
                            transform.position -= (transform.position - (v3_target_position + (transform.right.normalized * 2))).normalized * (f_speed_of_zooming * Time.deltaTime);  //Performing the Zooming in feature by relying on the Mouse scroll wheel input, speed of zooming is customizable

                            q_prev_rotation = transform.rotation;
                            b_is_zoomed = true;
                        }

                        transform.LookAt((v3_target_position + (transform.right.normalized * 2)));

                        if (transform.position.y > -0.5f)
                        {
                            transform.RotateAround(v3_target_position, transform.up, 30 * Time.deltaTime * (f_speed_of_rotation * Input.GetAxis("Mouse X")));   //Rotating the camera around the target's position, with customizable rotation speed
                        }
                        transform.RotateAround(v3_target_position, transform.right, 30 * Time.deltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed

                        Vector3 cubeTOCam = transform.position - v3_target_position;
                        Vector3 camToCube = -cubeTOCam;
                        camToCube.y = transform.position.y;

                        // Debug.Log(Vector3.Angle(cubeTOCam, camToCube));

                        if (Vector3.Angle(cubeTOCam, camToCube) < f_Xrestrict)
                        {
                            if (-Input.GetAxis("Mouse Y") > 0)
                            {
                                if (gameObject.transform.position.y > v3_target_position.y)
                                {
                                    transform.RotateAround(v3_target_position, transform.right, -30 * Time.deltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed
                                }
                            }
                            else if (-Input.GetAxis("Mouse Y") < 0)
                            {
                                if (gameObject.transform.position.y < v3_target_position.y)
                                {
                                    transform.RotateAround(v3_target_position, transform.right, -30 * Time.deltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed
                                }
                            }
                        }
                    }
                    break;

                case EntityPlayer.TARGET_STATE.NOT_AIMING:
                    {

                        int layerMask = 1 << LayerMask.NameToLayer("Ground");

                        RaycastHit hit;

                        Vector3 camPos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);

                        if (Physics.Raycast(v3_target_position, (v3_target_position - camPos), out hit,layerMask))
                        {                
                            if (f_zoomed > 0 && q_prev_rotation != transform.rotation || b_is_zoomed && f_zoomed > 0)
                            {
                                f_zoomed -= (f_speed_of_zooming * Time.deltaTime);
                                transform.position += (transform.position - v3_target_position).normalized * (f_speed_of_zooming * Time.deltaTime);  //Performing the Zooming in feature by relying on the Mouse scroll wheel input, speed of zooming is customizable
                            }                         
                        }
                        else
                        {
                            if (f_zoomed < f_zoomed_dist)
                            {
                                b_is_zoomed = false;
                                f_zoomed += f_speed_of_zooming * Time.deltaTime;
                                transform.position -= (transform.position - v3_target_position).normalized * (f_speed_of_zooming * Time.deltaTime);  //Performing the Zooming in feature by relying on the Mouse scroll wheel input, speed of zooming is customizable
                            }

                            q_prev_rotation = transform.rotation;
                        }

                        transform.LookAt(v3_target_position);

                        transform.RotateAround(v3_target_position, transform.up, 30 * Time.deltaTime * (f_speed_of_rotation * Input.GetAxis("Mouse X")));   //Rotating the camera around the target's position, with customizable rotation speed
                        transform.RotateAround(v3_target_position, transform.right, 30 * Time.deltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed

                        Vector3 cubeTOCam = transform.position - v3_target_position;
                        Vector3 camToCube = -cubeTOCam;
                        camToCube.y = transform.position.y;

                        // Debug.Log(Vector3.Angle(cubeTOCam, camToCube));

                        if (Vector3.Angle(cubeTOCam, camToCube) < f_Xrestrict)
                        {
                            if (-Input.GetAxis("Mouse Y") > 0)
                            {
                                if (gameObject.transform.position.y > v3_target_position.y)
                                {
                                    transform.RotateAround(v3_target_position, transform.right, -30 * Time.deltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed
                                }
                            }
                            else if (-Input.GetAxis("Mouse Y") < 0)
                            {
                                if (gameObject.transform.position.y < v3_target_position.y)
                                {
                                    transform.RotateAround(v3_target_position, transform.right, -30 * Time.deltaTime * (f_speed_of_rotation * -Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            Vector3 angle = transform.rotation.eulerAngles;
            angle.z = 0;
            transform.rotation = Quaternion.Euler(angle);

            v3_camera_last_position = transform.position;   //Saving the Camera's last position into a Vect or3 variable

            Vector3 new_target_pos = new Vector3(go_target.transform.position.x, go_target.transform.position.y + f_up_distance, go_target.transform.position.z);

            if (v3_target_position != new_target_pos)
            {
                transform.position -= v3_target_position - new_target_pos;

                v3_target_position = new_target_pos; //Moving the target as well as to ensure proper rotation
            }
        }
    }
    /*----------------------------------------------------------------------------------------------------------------------*/
}