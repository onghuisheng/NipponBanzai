using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : Singleton<TimeHandler>
{
    private float 
        f_time_multiplier, 
        f_difference;

    private int
        i_percentage_recovery_speed;

    public void AffectTime(float _multiplier, int _percentage_speed_of_recovery)     //Multiplier - Time * multiplier, Percentage of speed of recovery - how fast it returns the time to normal
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale *= _multiplier;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            i_percentage_recovery_speed = _percentage_speed_of_recovery;
            f_difference = 1 - Time.timeScale;     //Difference positive means time is scaled down, negative means time is scaled up
        }
    }

    protected override void Update()
    {
        base.Update();

        //Debug.Log("Timing: " + Time.timeScale);

        if (f_difference < 0)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
            }
        }
        else if (f_difference > 0)
        {
            if (Time.timeScale > 1)
            {
                Time.timeScale = 1;
            }
        }

        if (Time.timeScale != 1)
        {
            Time.timeScale += (((float)i_percentage_recovery_speed / 100.0f) * f_difference) * Time.unscaledDeltaTime;
        }       
    }

    public bool IsTimeScaleDefault()
    {
        return Time.timeScale == 1;
    }
}
