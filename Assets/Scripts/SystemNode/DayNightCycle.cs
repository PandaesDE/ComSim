/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private static readonly float MAX_LIGHT_INTENSITY = 1f;
    private static readonly float MIN_LIGHT_INTENSITY = .1f;
    [SerializeField] private new Light2D light;
    /*passed minutes relative to current Day*/
    private int clock_minutes = 0;

    private void FixedUpdate()
    {
        Gamevariables.MINUTES_PASSED += Gamevariables.MINUTES_PER_TICK;
        clock_minutes = Gamevariables.MINUTES_PASSED % (Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);

        Gamevariables.LIGHT_INTENSITY = Mathf.Clamp(calculateLightIntensity(), MIN_LIGHT_INTENSITY, MAX_LIGHT_INTENSITY);
        light.intensity = Gamevariables.LIGHT_INTENSITY;
    }

    private float calculateLightIntensity()
    {
        int DARK_morningMinutes =       6  * Gamevariables.MINUTES_PER_HOUR;
        int UPRISE_morningMinutes =     2  * Gamevariables.MINUTES_PER_HOUR;
        int BRIGHT_dayMinutes =         12 * Gamevariables.MINUTES_PER_HOUR;
        int DOWNFALL_eveningMinutes =   2  * Gamevariables.MINUTES_PER_HOUR; 


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
 
