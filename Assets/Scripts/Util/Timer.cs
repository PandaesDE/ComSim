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
    private readonly int _MINUTES_MAX;
    private int _minutesLeft;
    public Timer(int minutes_max)
    {
        this._MINUTES_MAX = minutes_max;
        this._minutesLeft = 0;
    }

    public void Reset()
    {
        _minutesLeft = _MINUTES_MAX;
    }

    public void Tick()
    {
        _minutesLeft = Mathf.Clamp(_minutesLeft - Gamevariables.MinutesPerTick, 0, _MINUTES_MAX);
    }

    public bool Finished()
    {
        return _minutesLeft <= 0;
    }
    
}
