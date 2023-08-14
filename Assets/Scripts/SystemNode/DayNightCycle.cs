using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private new Light2D light;
    [SerializeField] private int ticks = 0;

    private UI ui;

    private int ticksPerHour;
    private int ticksPerDay;

    private int time = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        ui = GetComponent<UI>();

        ticksPerHour = 4; //4 -> each tick ~ 15min
        ticksPerDay = 24 * ticksPerHour;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        displayUI();
        light.intensity = Mathf.Clamp(calculateLightIntensity(), .1f, 1f);
        ticks++;
    }

    private void displayUI()
    {
        ui.displayDay(1 + ticks / ticksPerDay);
        ui.displayTime(calculateDisplayTime());
    }

    private string calculateDisplayTime()
    {
        int minutesPerHour = 60;

        int display_hours = time / ticksPerHour;
        float display_minutes = minutesPerHour * ((time %ticksPerHour)/ (float)ticksPerHour);
        string hourPrefix = "";
        string minutesPrefix = "";


        if (display_hours < 10) hourPrefix = "0";
        if (display_minutes < 10) minutesPrefix = "0";

        return hourPrefix + display_hours + ":" + minutesPrefix + display_minutes;
    }

    private float calculateLightIntensity()
    {
        time = ticks % ticksPerDay;

        int DARK_morningTicks = 6 * ticksPerHour;
        int UPRISE_morningTicks = 4 * ticksPerHour;
        int BRIGHT_dayTicks = 10 * ticksPerHour;
        int DOWNFALL_eveningTicks = 2 * ticksPerHour; 


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
 
