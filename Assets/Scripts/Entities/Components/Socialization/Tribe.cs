/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - this class defines a group of social creatures
 *  
 *  References:
 *      Scene:
 *          - Simulation scene(s)
 *      Script:
 *          - Attached to social creatures
 *          
 *  Notes:
 *      - Ideas:
 *          - Popularty System
 *      
 *  Sources:
 *      - 
 */

using UnityEngine;

public class Tribe
{
    public Vector2 Home;
    private readonly int _areaWidthHeight = 10;
    public Tribe()
    {
        if (Home == null)
        {
            Home = Util.Random.CoordinateInPlayground();
        }
    }

    public Vector2 GetHomeArea()
    {
        return Util.Random.CoordinateInAreaOfPlayground(10, 10, Home);
    }
    
}
