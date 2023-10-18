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
        gender gender = Util.Random.Gender();
        Carnivore dietary = new();
        int health = 100;
        int weight = 80;
        float speed = .2f;
        initAttributes(gender, dietary, health, weight, speed);

        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (awake)
        {
            MoveToTarget();

            drink();
        }
    }

    /*Gets called by Parent*/
    protected override bool isSameSpecies(GameObject g)
    {
        Lion partner = g.GetComponent<Lion>();
        if (partner == null) return false;
        return true;
    }
}
