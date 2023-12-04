/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Gender interface
 *      - Defines methods for gender related behaviour
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

public interface IGender
{
    public static readonly float MAX_DESIRE = 100;
    public bool IsReadyForMating { get; }
    public bool IsMale { get; }
    public bool IsPregnant { get; }
    public float Desire { get; }
    public int Children { get; set; }
    public void FixedUpdate();
    public void MateWith(IGender partner);
}
