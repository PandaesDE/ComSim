/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Gender component of a creature
 *      - Handles gender and related behavior
 *  
 *  References:
 *      Scene:
 *          - Indirectly (Component of Creature.cs) for simulation scene(s)
 *      Script:
 *          - One instance per creature
 *  
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public class Male : IGender
{
    private static readonly int s_COOLDOWN_MATING = 3 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    private int _cooldownMating = 0;

    public bool IsReadyForMating
    {
        get
        {
            return _cooldownMating <= 0;
        }
    }
    
    public bool IsMale
    {
        get
        {
            return true;
        }
    }

    public void FixedUpdate()
    {
        if (_cooldownMating > 0)
        {
            _cooldownMating = Mathf.Clamp(_cooldownMating - Gamevariables.MinutesPerTick, 0, s_COOLDOWN_MATING);
        }
    }

    public void MateWith(IGender partner)
    {
        partner.MateWith(this);
        _cooldownMating = s_COOLDOWN_MATING;
    }
}
