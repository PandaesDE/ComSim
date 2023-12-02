/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - helper class for tick based timer
 *  
 *  References:
 *      Scene:
 *          - scene independent
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public class Timer
{
    private int _minutesMax;
    private int _minutesLeft;
    public Timer(int minutes_max)
    {
        this._minutesMax = minutes_max;
        this._minutesLeft = 0;
    }

    public void Reset()
    {
        _minutesLeft = _minutesMax;
    }

    public void Tick()
    {
        _minutesLeft = Mathf.Clamp(_minutesLeft - Gamevariables.MinutesPerTick, 0, _minutesMax);
    }

    public bool Finished()
    {
        return _minutesLeft <= 0;
    }
    
}
