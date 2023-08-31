using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private new Light2D light;
    [SerializeField] private int passed_time_minutes = 0;
    private int clock_minutes = 0;

    private UI ui;



    // Start is called before the first frame update
    private void Awake()
    {
        ui = GetComponent<UI>();
    }

    private void FixedUpdate()
    {
        passed_time_minutes += Gamevariables.MINUTES_PER_TICK;
        light.intensity = Mathf.Clamp(calculateLightIntensity(), .1f, 1f);

        displayUI();
    }

    private void displayUI()
    {
        ui.displayDay(1 + passed_time_minutes / (Gamevariables.MINUTES_PER_HOUR * Gamevariables.HOURS_PER_DAY));
        ui.displayTime(calculateDisplayTime());
    }

    private string calculateDisplayTime()
    {
        int display_hours = clock_minutes / Gamevariables.MINUTES_PER_HOUR;
        float display_minutes = clock_minutes % Gamevariables.MINUTES_PER_HOUR;
        string hourPrefix = "";
        string minutesPrefix = "";


        if (display_hours < 10) hourPrefix = "0";
        if (display_minutes < 10) minutesPrefix = "0";

        return hourPrefix + display_hours + ":" + minutesPrefix + display_minutes;
    }

    private float calculateLightIntensity()
    {
        clock_minutes = passed_time_minutes % (Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);

        int DARK_morningMinutes = 6 * Gamevariables.MINUTES_PER_HOUR;
        int UPRISE_morningMinutes = 4 * Gamevariables.MINUTES_PER_HOUR;
        int BRIGHT_dayMinutes = 10 * Gamevariables.MINUTES_PER_HOUR;
        int DOWNFALL_eveningMinutes = 2 * Gamevariables.MINUTES_PER_HOUR; 


        if (clock_minutes <= DARK_morningMinutes)
            return 0;

        if (clock_minutes <= DARK_morningMinutes + UPRISE_morningMinutes)
            return getPercentage(clock_minutes - DARK_morningMinutes, UPRISE_morningMinutes);

        if (clock_minutes <= DARK_morningMinutes + UPRISE_morningMinutes + BRIGHT_dayMinutes)
            return 1f;

        if (clock_minutes <= DARK_morningMinutes + UPRISE_morningMinutes + BRIGHT_dayMinutes + DOWNFALL_eveningMinutes)
            return 1f - getPercentage(clock_minutes - DARK_morningMinutes - UPRISE_morningMinutes - BRIGHT_dayMinutes, DOWNFALL_eveningMinutes);

        return 0;

        float getPercentage(int val, int max)
        {
            return (float)val/(float)max;
        }
    }
}
 
