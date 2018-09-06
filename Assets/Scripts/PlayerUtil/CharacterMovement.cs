using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    /*--------------------------------------------------- INITIALIZATION ---------------------------------------------------*/
    private Vector3
        v3_player_new_dir, 
        v3_player_last_position;   //Storing of camera's last moved position  


    private EntityPlayer
        Component_Player;

    [SerializeField]
    private GameObject
        go_camera;

    void Awake()
    {
        Component_Player = gameObject.GetComponent<EntityPlayer>();
    }

    void Start()
    {
        v3_player_last_position = transform.position;   //Saving the player's last position into a Vector3 variable    
    }

    /*---------------------------------------------------------------------------------------------------------------------*/


    /*------------------------------------------------------ UPDATES ------------------------------------------------------*/
    void Update()
    {
        //Debug.Log("STATE: " + Component_Player.GetPlayerState());

        if (Component_Player != null && Component_Player.Rb_rigidbody != null)
        {          
            if(Component_Player.GetPlayerState() == EntityPlayer.State.IDLE)
            {
                if(v3_player_last_position != transform.position)
                    v3_player_last_position = transform.position;   //Saving the Camera's last position into a Vector3 variable              
            }
            else if (Component_Player.GetPlayerState() == EntityPlayer.State.MOVING)
            {
                v3_player_last_position = transform.position;   //Saving the Camera's last position into a Vector3 variable              

                if (Input.GetKey(KeyCode.W))
                {
                    //transform.position += Time.deltaTime * (transform.forward).normalized * Component_Player.GetStats().F_speed * 2;
                    v3_player_new_dir = new Vector3(go_camera.transform.forward.x, Component_Player.GetPosition().y, go_camera.transform.forward.z).normalized;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    v3_player_new_dir = -new Vector3(go_camera.transform.forward.x, Component_Player.GetPosition().y, go_camera.transform.forward.z).normalized;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    v3_player_new_dir = -new Vector3(go_camera.transform.right.x, Component_Player.GetPosition().y, go_camera.transform.right.z).normalized;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    v3_player_new_dir = new Vector3(go_camera.transform.right.x, Component_Player.GetPosition().y, go_camera.transform.right.z).normalized;
                }

                Quaternion new_rotate = Quaternion.LookRotation(v3_player_new_dir);
                Quaternion new_target_rotation = Quaternion.Slerp(Component_Player.transform.rotation, new_rotate, Time.deltaTime * 5);

                Component_Player.Rb_rigidbody.MovePosition(Time.deltaTime * (transform.forward).normalized * Component_Player.GetStats().F_speed * 2);
                Component_Player.Rb_rigidbody.MoveRotation(new_target_rotation);

                Vector3 temp = (transform.position - v3_player_last_position).normalized;
            }
            //else if(Component_Player.GetPlayerState() == EntityPlayer.State.DODGE)
            //{
            //    if ((transform.position - v3_player_last_position).normalized.Equals(Vector3.zero))
            //    {
            //        transform.position += Time.deltaTime * (transform.forward).normalized * Component_Player.F_speed;
            //        //transform.Rotate(Vector3.right, Time.deltaTime * Component_Player.F_speed);
            //    }
            //    else
            //    {
            //        transform.position += Time.deltaTime * (transform.position - v3_player_last_position).normalized * Component_Player.F_speed;

            //        //Vector3 temp_vec = new Vector3(
            //        //    (((transform.position - v3_player_last_position).normalized.x) > 0 ? 1 : (((transform.position - v3_player_last_position).normalized.x) == 0 ? 0 : -1)),
            //        //    (((transform.position - v3_player_last_position).normalized.y) > 0 ? 1 : (((transform.position - v3_player_last_position).normalized.y) == 0 ? 0 : -1)),
            //        //    (((transform.position - v3_player_last_position).normalized.z) > 0 ? 1 : (((transform.position - v3_player_last_position).normalized.z) == 0 ? 0 : -1)));

            //        //transform.Rotate(Vector3.forward, Time.deltaTime * (temp_vec.x * Component_Player.F_speed));
            //        //transform.Rotate(Vector3.right, Time.deltaTime * (temp_vec.z * Component_Player.F_speed));
            //    }


            //}
        }
    }
    /*----------------------------------------------------------------------------------------------------------------------*/
}
