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

    private Timer _durationPregnancy = new Timer(18 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);
    private Timer _cooldownPregnancy = new Timer(9 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);
    private bool _isPregnant = false;

    public bool IsReadyForMating
    {
        get
        {
            return  !_isPregnant &&
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

    public Female(Creature creature)
    {
        this._creature = creature;
    }

    public void FixedUpdate()
    {
        if (_isPregnant)
        {
            if (!_durationPregnancy.Finished())
            {
                _durationPregnancy.Tick();
            } else
            {

                _creature.StatusManager.SetState(StatusManager.Status.giving_birth);
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
        _isPregnant = true;
        _durationPregnancy.Reset();
        _cooldownPregnancy.Reset();
    }

    private bool IsSuitable(IGender partner)
    {
        return IsMale != partner.IsMale;
    }


}
