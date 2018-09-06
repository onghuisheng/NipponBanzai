using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour
{

    /*--------------------------------------------------- INITIALIZATION ---------------------------------------------------*/
    public GameObject go_target;   //Current camera's target

    [SerializeField]
    private float
        f_speed_of_rotation;       //Speed of rotation around the camera's target


    private Vector3
        v3_target_position,        //Storing of Target's position as to not call the value from other script too many times
        v3_camera_last_position;   //Storing of camera's last moved position  

    [SerializeField]
    private float
        f_Xrestrict,
        f_Xdown_restrict;

    private void Start()
    {
        v3_target_position = go_target.transform.position;  //Setting the target position to a Vector3 variable

        transform.position = new Vector3(
              v3_target_position.x - (((go_target.transform.forward).normalized).x * 7),
              v3_target_position.y + 10,
              v3_target_position.z - (((go_target.transform.forward).normalized).z * 7));

        f_Xrestrict = 100;
        f_Xdown_restrict = 130;
    }
    /*---------------------------------------------------------------------------------------------------------------------*/


    /*------------------------------------------------------ UPDATES ------------------------------------------------------*/
    void Update()
    {
        transform.LookAt(v3_target_position);

        transform.RotateAround(v3_target_position, transform.up, 30 * Time.deltaTime * (f_speed_of_rotation * Input.GetAxis("Mouse X")));   //Rotating the camera around the target's position, with customizable rotation speed
        transform.RotateAround(v3_target_position, transform.right, 30 * Time.deltaTime * (f_speed_of_rotation * Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed

        Vector3 cubeTOCam = transform.position - v3_target_position;
        Vector3 camToCube = -cubeTOCam;
        camToCube.y = transform.position.y;

        // Debug.Log(Vector3.Angle(cubeTOCam, camToCube));

        if (Vector3.Angle(cubeTOCam, camToCube) < f_Xrestrict || Input.GetAxis("Mouse Y") < 0 && Vector3.Angle(cubeTOCam, camToCube) < f_Xdown_restrict)
            transform.RotateAround(v3_target_position, transform.right, -30 * Time.deltaTime * (f_speed_of_rotation * Input.GetAxis("Mouse Y")));   //Rotating the camera around the target's position, with customizable rotation speed

        //{
        //    Debug.Log("Before setbacks: " + transform.eulerAngles.x);
        //    if (transform.eulerAngles.x <= f_nXrestrict)
        //    {
        //        transform.eulerAngles = new Vector3(f_nXrestrict + 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
        //        Debug.Log("After negative setback: " + transform.eulerAngles.x);
        //    }
        //    else if (transform.eulerAngles.x >= f_pXrestrict)
        //    {
        //        transform.eulerAngles = new Vector3(f_pXrestrict - 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
        //        Debug.Log("After positive setback: " + transform.eulerAngles.x);
        //    }
        //}


        Vector3 angle = transform.rotation.eulerAngles;
        angle.z = 0;
        transform.rotation = Quaternion.Euler(angle);

        v3_camera_last_position = transform.position;   //Saving the Camera's last position into a Vect or3 variable

        if (v3_target_position != go_target.transform.position)
        {
            transform.position -= v3_target_position - go_target.transform.position;

            v3_target_position = go_target.transform.position; //Moving the target as well as to ensure proper rotation
        }
    }
    /*----------------------------------------------------------------------------------------------------------------------*/
}
