using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private new Light2D light;
    private int ticksPerHour;
    private int ticksPerDay;
    private int ticks = 0;

    // Start is called before the first frame update
    private void Awake()
    {
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
        float intensity = calculateLightIntensity();
        light.intensity = Mathf.Clamp(intensity, .1f, 1f);
        ticks++;
    }

    private float calculateLightIntensity()
    {
        int time = ticks % ticksPerDay;
        int DARK_morningHours = 6;
        int nightHours = 2;
        int dayHours = 24 - DARK_morningHours - nightHours;
        if (time <= DARK_morningHours * ticksPerHour) return 0;
        if (time <= nightHours * ticksPerHour) return 0;
        return (float)time/ticksPerDay - DARK_morningHours * ticksPerHour + nightHours * ticksPerHour;
    }
}
 
