/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Consumable interface
 *      - Defines Edible Objects
 *  
 *  References:
 *      Scene:
 *          - Indirectly (IConsumable Objects) for simulation scene(s)
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

public interface IConsumable
{
    public bool IsConsumed { get; }
    public bool IsMeat { get; }
    public bool HasFood { get; }
    public GameObject gameObject { get; }
    public float Consume(float amount);
}
