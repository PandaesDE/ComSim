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

public class Herbivore : IDietary
{
    private static readonly float dangerZone = 7;
    private Creature creature;

    public Herbivore(Creature creature)
    {
        this.creature = creature;
    }

    public IDietary.Specification specification
    {
        get
        {
            return IDietary.Specification.HERBIVORE;
        }
    }

    public bool IsEdibleFoodSource(IConsumable food)
    {
        return !food.IsMeat;
    }

    StatusManager.Status IDietary.OnAttacked()
    {
        return StatusManager.Status.fleeing;
    }

    public bool IsInDangerZone(Creature approacher)
    {
        return Util.InRange(creature.gameObject.transform.position, approacher.gameObject.transform.position, dangerZone);
    }

    public StatusManager.Status OnApproached()
    {
        return StatusManager.Status.fleeing;
    }

    public StatusManager.Status OnNoFood()
    {
        return creature.StatusManager.status;
    }
}
