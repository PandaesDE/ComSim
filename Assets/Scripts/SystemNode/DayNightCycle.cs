/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - handles the day and night cylce
 *      - handles the light intensity for the given time
 *      - increases the Gametime (MinutesPassed)
 *  
 *  References:
 *      Scene:
 *          - 
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private static readonly float MAX_LIGHT_INTENSITY = 1f;
    private static readonly float MIN_LIGHT_INTENSITY = .1f;
    [SerializeField] private Light2D _light;
    /*passed minutes relative to current Day*/
    private int _clock_minutes = 0;

    private void FixedUpdate()
    {
        Gamevariables.MinutesPassed += Gamevariables.MinutesPerTick;
        _clock_minutes = Gamevariables.MinutesPassed % (Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);

        Gamevariables.LightIntensity = Mathf.Clamp(CalculateLightIntensity(), MIN_LIGHT_INTENSITY, MAX_LIGHT_INTENSITY);
        _light.intensity = Gamevariables.LightIntensity;
    }

    private float CalculateLightIntensity()
    {
        int DARK_morningMinutes =       6  * Gamevariables.MINUTES_PER_HOUR;
        int UPRISE_morningMinutes =     2  * Gamevariables.MINUTES_PER_HOUR;
        int BRIGHT_dayMinutes =         12 * Gamevariables.MINUTES_PER_HOUR;
        int DOWNFALL_eveningMinutes =   2  * Gamevariables.MINUTES_PER_HOUR; 


        if (_clock_minutes <= DARK_morningMinutes)
            return 0;

        if (_clock_minutes <= DARK_morningMinutes + UPRISE_morningMinutes)
            return GetPercentage(_clock_minutes - DARK_morningMinutes, UPRISE_morningMinutes);

        if (_clock_minutes <= DARK_morningMinutes + UPRISE_morningMinutes + BRIGHT_dayMinutes)
            return 1f;

        if (_clock_minutes <= DARK_morningMinutes + UPRISE_morningMinutes + BRIGHT_dayMinutes + DOWNFALL_eveningMinutes)
            return 1f - GetPercentage(_clock_minutes - DARK_morningMinutes - UPRISE_morningMinutes - BRIGHT_dayMinutes, DOWNFALL_eveningMinutes);

        return 0;

        float GetPercentage(int val, int max)
        {
            return (float)val/(float)max;
        }
    }
}
 
