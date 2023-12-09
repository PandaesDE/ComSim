/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Sensoric component of a creature
 *      - detects 2d Colliders within vision
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
 *      - https://www.youtube.com/watch?v=xp37Hz1t1Q8
 */

using UnityEngine;

public class Senses
{
    private Creature _creature;

    private static readonly int S_VISION_DISTANCE = 15;
    private static readonly int S_VISION_WIDTH = 7;

    public Senses(Creature creature)
    {
        this._creature = creature;
    }    

    public Vector2[] GetVisionCoordinates()
    {
        Vector2[] vc = new Vector2[S_VISION_DISTANCE * S_VISION_WIDTH];
        

        switch (_creature.Facing)
        {
            case Movement.Direction.north:
                for (int i = 0; i < S_VISION_DISTANCE; i++)
                {
                    for (int j = 0; j < S_VISION_WIDTH; j++)
                    {
                        vc[j + S_VISION_WIDTH * i] = new Vector2(
                            _creature.transform.position.x + j - (S_VISION_WIDTH/2),
                            _creature.transform.position.y + i
                        );
                    }
                }
                return vc;
            case Movement.Direction.east:
                for (int i = 0; i < S_VISION_DISTANCE; i++)
                {
                    for (int j = 0; j < S_VISION_WIDTH; j++)
                    {
                        vc[j + S_VISION_WIDTH * i] = new Vector2(
                            _creature.transform.position.x + i,
                            _creature.transform.position.y + j - (S_VISION_WIDTH / 2)
                        );
                    }
                }
                return vc;
            case Movement.Direction.south:
                for (int i = 0; i < S_VISION_DISTANCE; i++)
                {
                    for (int j = 0; j < S_VISION_WIDTH; j++)
                    {
                        vc[j + S_VISION_WIDTH * i] = new Vector2(
                            _creature.transform.position.x + j - (S_VISION_WIDTH / 2),
                            _creature.transform.position.y - i
                        );
                    }
                }
                return vc;
            default: //WEST
                for (int i = 0; i < S_VISION_DISTANCE; i++)
                {
                    for (int j = 0; j < S_VISION_WIDTH; j++)
                    {
                        vc[j + S_VISION_WIDTH * i] = new Vector2(
                            _creature.transform.position.x - i,
                            _creature.transform.position.y + j - (S_VISION_WIDTH / 2)
                        );
                    }
                }
                return vc;
        }
    }
}
