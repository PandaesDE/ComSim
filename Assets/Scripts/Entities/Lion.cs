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

public class Lion : Creature
{
    protected override void Awake()
    {
        base.Awake();

        gender gender = Util.Random.Gender();
        Carnivore dietary = new(this);
        int health = 100;
        int weight = 80;
        float speed = .2f;
        initAttributes(gender, dietary, health, weight, speed);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (mission != Status.SLEEPING)
        {
            movement.MoveToTarget();

            drink();
        }
    }

    /*Gets called by Parent*/
    protected override bool isSameSpecies(Creature c)
    {
        Lion partner = c.gameObject.GetComponent<Lion>();
        if (partner == null) return false;
        return true;
    }
}
