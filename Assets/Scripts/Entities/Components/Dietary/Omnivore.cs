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

public class Omnivore : IDietary
{
    private static readonly float dangerZone = 5;
    private Creature creature;

    public Omnivore(Creature creature)
    {
        this.creature = creature;
    }

    public IDietary.Specification specification
    {
        get
        {
            return IDietary.Specification.OMNIVORE;
        }
    }

    public bool IsEdibleFoodSource(IConsumable food)
    {
        return true;
    }

    public bool IsInDangerZone(Creature approacher)
    {
        return Util.InRange(creature.gameObject.transform.position, approacher.gameObject.transform.position, dangerZone);
    }

    public StatusManager.State OnAttacked()
    {
        if (creature.Health / creature.MaxHealth <= .5f)
        {
            return StatusManager.State.fleeing;
        }

        return StatusManager.State.hunting;
    }

    public StatusManager.State OnApproached()
    {
        if (creature.Health / creature.MaxHealth <= .8f)
        {
            return StatusManager.State.fleeing;
        }
        if (creature.hunger < 60)
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
