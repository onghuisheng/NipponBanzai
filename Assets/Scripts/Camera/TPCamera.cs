using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour {

    /*--------------------------------------------------- INITIALIZATION ---------------------------------------------------*/
    public GameObject go_target;   //Current camera's target

    public float
        f_speed_of_rotation;       //Speed of rotation around the camera's target


    private Vector3
        v3_target_position,        //Storing of Target's position as to not call the value from other script too many times
        v3_camera_last_position;   //Storing of camera's last moved position    

    private void Start()
    {
        v3_target_position = go_target.transform.position;  //Setting the target position to a Vector3 variable

        transform.position = new Vector3(
              v3_target_position.x - (((go_target.transform.forward).normalized).x * 4),
              v3_target_position.y + 5,
              v3_target_position.z - (((go_target.transform.forward).normalized).z * 4));
    }
    /*---------------------------------------------------------------------------------------------------------------------*/


    /*------------------------------------------------------ UPDATES ------------------------------------------------------*/
    void Update()
    {
        transform.LookAt(v3_target_position);

        transform.RotateAround(v3_target_position, Vector3.up, 30 * Time.deltaTime * (f_speed_of_rotation * Input.GetAxis("Mouse X")));   //Rotating the camera around the target's position, with customizable rotation speed

        v3_camera_last_position = transform.position;   //Saving the Camera's last position into a Vect or3 variable

        if (v3_target_position != go_target.transform.position)
        {
            transform.position -= v3_target_position - go_target.transform.position;

            v3_target_position = go_target.transform.position; //Moving the target as well as to ensure proper rotation
        }
    }
    /*----------------------------------------------------------------------------------------------------------------------*/
}
