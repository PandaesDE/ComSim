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
 *      - Handles gender and related behaviour
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
    public Creature Creature { get; private set; }
    public Creature Partner { get; set; } //only needed for Female -> no use (yet)

    private Timer _cooldownMating;
    private float _desireIncreaseRatePerMinute;


    public bool IsReadyForMating
    {
        get
        {
            return  Creature.GrowthFactor >= 1f &&
                    _cooldownMating.Finished();
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

    public float Desire
    {
        get;
        private set;
    }

    public int Children
    {
        get;
        set;
    } = 0;

    public Male(Creature creature, int cooldownMating, float daysUntilMaxDesire)
    {
        Creature = creature;
        _cooldownMating = new(cooldownMating);
        _desireIncreaseRatePerMinute = (float)IGender.MAX_DESIRE / (daysUntilMaxDesire * (float)Gamevariables.HOURS_PER_DAY * (float)Gamevariables.MINUTES_PER_HOUR);
        Desire = 0;
    }

    public void FixedUpdate()
    {
        if (!_cooldownMating.Finished())
        {
            _cooldownMating.Tick();
        }

        IncreaseDesire();
    }

    public void MateWith(IGender partner)
    {
        if (!IsReadyForMating) return;

        partner.MateWith(this);
        Desire = 0;
        _cooldownMating.Reset();
    }

    private void IncreaseDesire()
    {
        if (Creature.GrowthFactor < 1f) return;
        if (Desire >= IGender.MAX_DESIRE) return;
        float increase = _desireIncreaseRatePerMinute * Gamevariables.MinutesPerTick;
        Desire = Mathf.Clamp(Desire + increase, 0 , IGender.MAX_DESIRE);
    }
}
