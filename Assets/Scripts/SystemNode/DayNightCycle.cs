using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private new Light2D light;
    [SerializeField] private int ticks = 0;

    private UI ui;



    private int time = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        ui = GetComponent<UI>();
    }

    private void FixedUpdate()
    {
        displayUI();
        light.intensity = Mathf.Clamp(calculateLightIntensity(), .1f, 1f);
        ticks++;
    }

    private void displayUI()
    {
        ui.displayDay(1 + ticks / (Gamevariables.TICKS_PER_HOUR * Gamevariables.HOURS_PER_DAY));
        ui.displayTime(calculateDisplayTime());
    }

    private string calculateDisplayTime()
    {
        int minutesPerHour = 60;

        int display_hours = time / Gamevariables.TICKS_PER_HOUR;
        float display_minutes = minutesPerHour * ((time % Gamevariables.TICKS_PER_HOUR) / (float)Gamevariables.TICKS_PER_HOUR);
        string hourPrefix = "";
        string minutesPrefix = "";


        if (display_hours < 10) hourPrefix = "0";
        if (display_minutes < 10) minutesPrefix = "0";

        return hourPrefix + display_hours + ":" + minutesPrefix + display_minutes;
    }

    private float calculateLightIntensity()
    {
        time = ticks % (Gamevariables.TICKS_PER_HOUR * Gamevariables.HOURS_PER_DAY);

        int DARK_morningTicks = 6 * Gamevariables.TICKS_PER_HOUR;
        int UPRISE_morningTicks = 4 * Gamevariables.TICKS_PER_HOUR;
        int BRIGHT_dayTicks = 10 * Gamevariables.TICKS_PER_HOUR;
        int DOWNFALL_eveningTicks = 2 * Gamevariables.TICKS_PER_HOUR; 


        if (time <= DARK_morningTicks)
            return 0;

        if (time <= DARK_morningTicks + UPRISE_morningTicks)
            return getPercentage(time - DARK_morningTicks, UPRISE_morningTicks);

        if (time <= DARK_morningTicks + UPRISE_morningTicks + BRIGHT_dayTicks)
            return 1f;

        if (time <= DARK_morningTicks + UPRISE_morningTicks + BRIGHT_dayTicks + DOWNFALL_eveningTicks)
            return 1f - getPercentage(time - DARK_morningTicks - UPRISE_morningTicks - BRIGHT_dayTicks, DOWNFALL_eveningTicks);

        return 0;

        float getPercentage(int val, int max)
        {
            return (float)val/(float)max;
        }
    }
}
 
