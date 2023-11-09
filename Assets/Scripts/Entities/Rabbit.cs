/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using UnityEngine;

public class Rabbit : Creature
{
    protected override void Awake()
    {
        base.Awake();

        Herbivore dietary = new(this);
        int health = 45;
        int weight = 30;
        float speed = .2f;
        initAttributes(dietary, health, weight, speed);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (statusManager.status != StatusManager.Status.SLEEPING)
        {
            movement.MoveToTarget();
            evaluateVision();
            makeStatusBasedMove();
        }
    }

    /*Gets called by Parent*/
    protected override bool isSameSpecies(Creature c)
    {
        Rabbit partner = c.gameObject.GetComponent<Rabbit>();
        if (partner == null) return false;
        return true;
    }

    protected override void giveBirth()
    {
        Spawner.spawnRabbits(1, gameObject.transform.position);
    }
}
