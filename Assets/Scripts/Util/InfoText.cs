using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoText : MonoBehaviour {

    private enum COLOR
    {
        AQUA, 
        BLACK,
        BLUE,
        BROWN,
        DARKBLUE,
        MEGENTA,
        GREEN,
        GREY,
        LIGHTBLUE,
        LIME,
        MAROON,
        NAVY,
        OLIVE,
        ORANGE,
        PURPLE,
        RED,
        SILVER,
        TEAL,
        WHITE,
        YELLOW
    }

    private Dictionary<COLOR, string>
        dic_color = new Dictionary<COLOR, string>();

    private float
       f_randomcolor_r,
       f_randomcolor_g,
       f_randomcolor_b;

    private bool
        b_reverse_r,
        b_reverse_g,
        b_reverse_b;

    private void Start()
    {
        f_randomcolor_r = f_randomcolor_g = f_randomcolor_b = 0;
        b_reverse_r = b_reverse_g = b_reverse_b = false;

        dic_color.Add(COLOR.AQUA, "#00ffffff");
        dic_color.Add(COLOR.BLACK, "#000000ff");
        dic_color.Add(COLOR.BLUE, "#0000ffff");
        dic_color.Add(COLOR.BROWN, "#a52a2aff");
        dic_color.Add(COLOR.DARKBLUE, "#0000a0ff");
        dic_color.Add(COLOR.GREEN, "#008000ff");
        dic_color.Add(COLOR.GREY, "#808080ff");
        dic_color.Add(COLOR.LIGHTBLUE, "#add8e6ff");
        dic_color.Add(COLOR.LIME, "#00ff00ff");
        dic_color.Add(COLOR.MAROON, "#800000ff");
        dic_color.Add(COLOR.MEGENTA, "#ff00ffff");
        dic_color.Add(COLOR.NAVY, "#000080ff");
        dic_color.Add(COLOR.OLIVE, "#808000ff");
        dic_color.Add(COLOR.ORANGE, "#ffa500ff");
        dic_color.Add(COLOR.PURPLE, "#800080ff");
        dic_color.Add(COLOR.RED, "#ff0000ff");
        dic_color.Add(COLOR.SILVER, "#c0c0c0ff");
        dic_color.Add(COLOR.TEAL, "#008080ff");
        dic_color.Add(COLOR.WHITE, "#ffffffff");
        dic_color.Add(COLOR.YELLOW, "#ffff00ff");
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100) < 30)
        {
            if (!b_reverse_r)
            {
                f_randomcolor_r += 0.1f;

                if (f_randomcolor_r >= 1)
                    b_reverse_r = true;
            }
            else
            {
                f_randomcolor_r -= 0.1f;

                if (f_randomcolor_r <= 0)
                    b_reverse_r = false;
            }
        }

        if (Random.Range(0, 100) < 30)
        {
            if (!b_reverse_b)
            {
                f_randomcolor_b += 0.1f;

                if (f_randomcolor_b >= 1)
                    b_reverse_b = true;
            }
            else
            {
                f_randomcolor_b -= 0.1f;

                if (f_randomcolor_b <= 0)
                    b_reverse_b = false;
            }
        }

        if (Random.Range(0, 100) < 30)
        {
            if (!b_reverse_g)
            {
                f_randomcolor_g += 0.1f;

                if (f_randomcolor_g >= 1)
                    b_reverse_g = true;
            }
            else
            {
                f_randomcolor_g -= 0.1f;

                if (f_randomcolor_g <= 0)
                    b_reverse_g = false;
            }
        }

        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up, -180);

        TextMesh temp = GetComponent<TextMesh>();

        if (temp != null)
        {
            //if (gameObject.GetComponentInParent<EntityPickUp>() != null)
            //{
            //    EntityPickUp _pickup = gameObject.GetComponentInParent<EntityPickUp>();

            //    temp.text = "Name: " + _pickup.It_item.S_name;  
            //    temp.text += "\nType: " + SetTextColor((((int)_pickup.It_item.GetSpeedType() == 0) ? COLOR.RED : (((int)_pickup.It_item.GetSpeedType() == 2) ? COLOR.GREEN : COLOR.GREY)), _pickup.It_item.GetSpeedType().ToString());
            //    temp.text += ((_pickup.It_item.St_stats.f_damage > 0) ? ("\nDamage: " + _pickup.It_item.St_stats.f_damage.ToString()/* + (())*/) : "");
            //    temp.text += ((_pickup.It_item.St_stats.f_defence > 0) ? ("\nDefence: " + _pickup.It_item.St_stats.f_defence.ToString()/* + (())*/) : "");
            //    temp.text += ((_pickup.It_item.St_stats.f_health > 0) ? ("\nHealth: " + _pickup.It_item.St_stats.f_health.ToString()/* + (())*/) : "");
            //    temp.text += ((_pickup.It_item.St_stats.f_speed > 0) ? ("\nSpeed: " + _pickup.It_item.St_stats.f_speed.ToString()/* + (())*/) : "");

            //    temp.color = new Color(0.1f, 0.7f, 0.0f);
            //}
        }

        if (temp.text.ToLower().Contains("hoopawolf"))
        {
            temp.color = new Color(f_randomcolor_r, f_randomcolor_b, f_randomcolor_g);
        }
    }

    private string SetTextColor(COLOR _color, string _text)
    {
        return "<color=" + dic_color[_color] + "> " + _text + "</color>";
    }
}
