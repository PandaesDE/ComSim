/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using UnityEngine;

/*  *** INFO ***
 *  This Script only Works under following conditions:
 *  - The Layer of the gameObject with the Script needs to be "Vision"
 *  - The Script is on the Child gameObject of the actual Entity
 */

/*  *** Notes ***
 *  https://www.youtube.com/watch?v=xp37Hz1t1Q8
 */

public class Senses
{
    private Creature creature;

    private static readonly int VISION_DISTANCE = 15;
    private static readonly int VISION_WIDTH = 7;

    public Senses(Creature creature)
    {
        this.creature = creature;
    }    

    public Vector2[] getVisionCoordinates()
    {
        Vector2[] vc = new Vector2[VISION_DISTANCE * VISION_WIDTH];
        

        switch (creature.facing)
        {
            case Creature.Direction.NORTH:
                for (int i = 0; i < VISION_DISTANCE; i++)
                {
                    for (int j = 0; j < VISION_WIDTH; j++)
                    {
                        vc[j + VISION_WIDTH * i] = new Vector2(
                            creature.transform.position.x + j - (VISION_WIDTH/2),
                            creature.transform.position.y + i
                        );
                    }
                }
                return vc;
            case Creature.Direction.EAST:
                for (int i = 0; i < VISION_DISTANCE; i++)
                {
                    for (int j = 0; j < VISION_WIDTH; j++)
                    {
                        vc[j + VISION_WIDTH * i] = new Vector2(
                            creature.transform.position.x + i,
                            creature.transform.position.y + j - (VISION_WIDTH / 2)
                        );
                    }
                }
                return vc;
            case Creature.Direction.SOUTH:
                for (int i = 0; i < VISION_DISTANCE; i++)
                {
                    for (int j = 0; j < VISION_WIDTH; j++)
                    {
                        vc[j + VISION_WIDTH * i] = new Vector2(
                            creature.transform.position.x + j - (VISION_WIDTH / 2),
                            creature.transform.position.y - i
                        );
                    }
                }
                return vc;
            default: //WEST
                for (int i = 0; i < VISION_DISTANCE; i++)
                {
                    for (int j = 0; j < VISION_WIDTH; j++)
                    {
                        vc[j + VISION_WIDTH * i] = new Vector2(
                            creature.transform.position.x - i,
                            creature.transform.position.y + j - (VISION_WIDTH / 2)
                        );
                    }
                }
                return vc;
        }
    }
}
