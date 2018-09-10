using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTapCheck : Singleton<DoubleTapCheck>
{
    private KeyCode 
        kc_current_entered_key,

        kc_current_entered_mouse;

    private float
        f_timer_when_pressed, 
        f_interval,

        f_timer_when_pressed_mouse,
        f_interval_mouse;

    private bool
        b_is_double_tap,

        b_is_double_tap_mouse;

    protected void Start()
    {
        kc_current_entered_key = KeyCode.None;
        b_is_double_tap = false;
        f_interval = 0.3f;

        kc_current_entered_mouse = KeyCode.None;
        b_is_double_tap_mouse = false;
        f_interval_mouse = 0.3f;
    }

    protected override void Update()
    {
        #region Keyboard
        if (Time.time - f_timer_when_pressed > f_interval && b_is_double_tap)
            b_is_double_tap = false;

        if (Input.anyKeyDown)
        {
            KeyCode temp = KeyCode.None;
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    if (vKey != KeyCode.Mouse0 &&
                        vKey != KeyCode.Mouse1 &&
                        vKey != KeyCode.Mouse2 &&
                        vKey != KeyCode.Mouse3 &&
                        vKey != KeyCode.Mouse4 &&
                        vKey != KeyCode.Mouse5 &&
                        vKey != KeyCode.Mouse6)
                        temp = vKey;
                    else
                        break;
                }
            }

            if (kc_current_entered_key == KeyCode.None || kc_current_entered_key != temp || Time.time - f_timer_when_pressed > f_interval)
            {
                kc_current_entered_key = temp;
                b_is_double_tap = false;
                f_timer_when_pressed = Time.time;
            }
            else
            {
                if (Time.time - f_timer_when_pressed < f_interval)
                {
                    b_is_double_tap = true;
                    //Debug.Log("Double Tapped: " + kc_current_entered_key);
                }
                else
                {
                    b_is_double_tap = false;
                }
            }
        }
        #endregion

        #region Mouse
        if (Time.time - f_timer_when_pressed_mouse > f_interval_mouse && b_is_double_tap_mouse)
            b_is_double_tap_mouse = false;

        if (Input.anyKeyDown)
        {
            KeyCode temp = KeyCode.None;
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    if (vKey == KeyCode.Mouse0 ||
                        vKey == KeyCode.Mouse1 ||
                        vKey == KeyCode.Mouse2 ||
                        vKey == KeyCode.Mouse3 ||
                        vKey == KeyCode.Mouse4 ||
                        vKey == KeyCode.Mouse5 ||
                        vKey == KeyCode.Mouse6)
                        temp = vKey;
                    else
                        break;
                }
            }

            if (kc_current_entered_mouse == KeyCode.None || kc_current_entered_mouse != temp || Time.time - f_timer_when_pressed_mouse > f_interval_mouse)
            {
                kc_current_entered_mouse = temp;
                b_is_double_tap_mouse = false;
                f_timer_when_pressed_mouse = Time.time;
            }
            else
            {
                if (Time.time - f_timer_when_pressed_mouse < f_interval_mouse)
                {
                    b_is_double_tap_mouse = true;
                    //Debug.Log("Double Clicked: " + kc_current_entered_mouse);
                }
                else
                {
                    b_is_double_tap_mouse = false;
                }
            }
        }
        #endregion
    }

    public KeyCode GetDoubleTapKey()
    {
        return kc_current_entered_key;
    }

    public bool IsDoubleTapTriggered()
    {
        return b_is_double_tap;
    }

    public KeyCode GetDoubleTapMouseKey()
    {
        return kc_current_entered_mouse;
    }

    public bool IsDoubleClickTriggered()
    {
        return b_is_double_tap_mouse;
    }
}
