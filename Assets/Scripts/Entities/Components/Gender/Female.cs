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

public class Female : IGender
{
    private Creature _creature;

    private Timer _durationPregnancy;
    private Timer _cooldownPregnancy;
    
    
    public bool IsPregnant
    {
        get;
        private set;
    }

    public bool IsReadyForMating
    {
        get
        {
            return  !IsPregnant &&
                    _creature.Age >= _creature.FertilityAge &&
                    _cooldownPregnancy.Finished();

        }
    }

    public bool IsMale
    {
        get
        {
            return false;
        }
    }

    public float Desire
    {
        get
        {
            return 0;
        }
    }

    public int Children
    {
        get;
        set;
    }

    public Female(Creature creature, int cooldownPregancy, int durationPregnancy)
    {
        this._creature = creature;
        this._cooldownPregnancy = new(cooldownPregancy);
        this._durationPregnancy = new(durationPregnancy);
        IsPregnant = false;
    }

    public void FixedUpdate()
    {
        if (IsPregnant)
        {
            if (!_durationPregnancy.Finished())
            {
                _durationPregnancy.Tick();
            } else
            {
                _creature.StatusManager.SetState(StatusManager.State.giving_birth);
                IsPregnant = false;
            }
        } else
        {
            if (!_cooldownPregnancy.Finished())
            {
                _cooldownPregnancy.Tick();
            }
        }
    }

    public void MateWith(IGender partner)
    {
        if (!IsSuitable(partner)) return;
        //chance of failure ?
        _durationPregnancy.Reset();
        _cooldownPregnancy.Reset();
        IsPregnant = true;
    }

    private bool IsSuitable(IGender partner)
    {
        return IsMale != partner.IsMale;
    }


}
