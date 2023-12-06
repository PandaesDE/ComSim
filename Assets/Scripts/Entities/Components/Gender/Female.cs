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

public class Female : IGender
{
    public Creature Creature { get; private set; }
    public Creature Partner { get; set; }

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
                    Creature.Age >= Creature.FertilityAge &&
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
        this.Creature = creature;
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
                Creature.StatusManager.SetState(StatusManager.State.giving_birth);
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
        if (!IsReadyForMating) return;
        if (Partner != null) return;

        //chance of failure ?
        _durationPregnancy.Reset();
        _cooldownPregnancy.Reset();
        IsPregnant = true;
        Partner = partner.Creature;
    }

    private bool IsSuitable(IGender partner)
    {
        return IsMale != partner.IsMale;
    }


}
