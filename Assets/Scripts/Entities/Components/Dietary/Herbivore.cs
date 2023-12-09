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
 *      - Handles food and related behaviour
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
    private static readonly float _S_DANGER_ZONE = 7;
    private Creature _creature;

    public Herbivore(Creature creature)
    {
        this._creature = creature;
    }

    public IDietary.Specification Spec
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

    StatusManager.State IDietary.OnAttacked()
    {
        return StatusManager.State.fleeing;
    }

    public bool IsInDangerZone(Creature approacher)
    {
        return Util.InRange(_creature.gameObject.transform.position, approacher.gameObject.transform.position, _S_DANGER_ZONE);
    }

    public StatusManager.State OnApproached()
    {
        return StatusManager.State.fleeing;
    }

    public StatusManager.State OnNoFood()
    {
        return _creature.StatusManager.Status;
    }
}
