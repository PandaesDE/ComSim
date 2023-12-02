/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Dietary interface
 *      - Defines methods for food related behaviour
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

public interface IDietary
{
    public enum Specification {
        OMNIVORE,
        CARNIVORE,
        HERBIVORE
    }
    public Specification specification { get; }
    public bool IsEdibleFoodSource(IConsumable food);
    public bool IsInDangerZone(Creature creature);
    public StatusManager.Status OnNoFood();
    public StatusManager.Status OnAttacked();
    public StatusManager.Status OnApproached();

}
