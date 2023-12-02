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

    public StatusManager.Status OnAttacked()
    {
        if (_creature.Health / _creature.maxHealth <= .2f)
        {
            return StatusManager.Status.fleeing;
        }

        return StatusManager.Status.hunting;
    }

    public bool IsInDangerZone(Creature approacher)
    {
        return Util.InRange(_creature.gameObject.transform.position, approacher.gameObject.transform.position, _s_dangerZone);
    }

    public StatusManager.Status OnApproached()
    {
        if (_creature.Health / _creature.maxHealth <= .3f)
        {
            return StatusManager.Status.fleeing;
        }
        if (_creature.hunger < 90)
        {
            return StatusManager.Status.hunting;
        }
        return StatusManager.Status.wandering;
    }

    StatusManager.Status IDietary.OnNoFood()
    {
        return StatusManager.Status.hunting;
    }
}
