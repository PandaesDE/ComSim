/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Dietary component of a creature
 *      - Handles food and related behavior
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

public class Carnivore : IDietary
{
    private static readonly float _s_dangerZone = 7;
    private Creature _creature;



    public Carnivore(Creature creature)
    {
        this._creature = creature;
    }

    public IDietary.Specification specification
    {
        get
        {
            return IDietary.Specification.CARNIVORE;
        }
    }

    public bool IsEdibleFoodSource(IConsumable food)
    {
        return food.IsMeat;
    }

    public StatusManager.State OnAttacked()
    {
        if (_creature.Health / _creature.MaxHealth <= .2f)
        {
            return StatusManager.State.fleeing;
        }

        return StatusManager.State.hunting;
    }

    public bool IsInDangerZone(Creature approacher)
    {
        return Util.InRange(_creature.gameObject.transform.position, approacher.gameObject.transform.position, _s_dangerZone);
    }

    public StatusManager.State OnApproached()
    {
        if (_creature.Health / _creature.MaxHealth <= .3f)
        {
            return StatusManager.State.fleeing;
        }
        if (_creature.hunger < 90)
        {
            return StatusManager.State.hunting;
        }
        return StatusManager.State.wandering;
    }

    StatusManager.State IDietary.OnNoFood()
    {
        return StatusManager.State.hunting;
    }
}
