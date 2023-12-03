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
    private Timer _cooldownMating;

    public bool IsReadyForMating
    {
        get
        {
            return _cooldownMating.Finished();
        }
    }
    
    public bool IsMale
    {
        get
        {
            return true;
        }
    }

    public bool IsPregnant
    {
        get
        {
            return false;
        }
    }

    public Male(int cooldownMating)
    {
        _cooldownMating = new(cooldownMating);
    }

    public void FixedUpdate()
    {
        if (!_cooldownMating.Finished())
        {
            _cooldownMating.Tick();
        }
    }

    public void MateWith(IGender partner)
    {
        partner.MateWith(this);
        _cooldownMating.Reset();
    }
}
