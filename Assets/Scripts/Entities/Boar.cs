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

public class Boar : Creature
{
    protected override void Awake()
    {
        base.Awake();

        IGender gender = Util.Random.Gender();
        Omnivore dietary = new(this);
        int health = 150;
        int weight = 130;
        float speed = .2f;
        initAttributes(gender, dietary, health, weight, speed);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (mission != Status.SLEEPING)
        {
            movement.MoveToTarget();
            evaluateVision();
            makeStatusBasedMove();
        }
    }

    /*Gets called by Parent*/
    protected override bool isSameSpecies(Creature c)
    {
        Boar partner = c.gameObject.GetComponent<Boar>();
        if (partner == null) return false;
        return true;
    }
}
