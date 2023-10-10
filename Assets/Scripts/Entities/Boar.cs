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
        gender gender = Util.Random.Gender();
        foodType dietary = foodType.OMNIVORE;
        int health = 150;
        int weight = 130;
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
    protected override bool isValidPartner(GameObject g)
    {
        Boar partner = g.GetComponent<Boar>();

        if (partner == null) return false;
        return isGenericMate(partner);
    }

    protected override bool isEdibleFoodSource(GameObject g)
    {
        if (g.GetComponent<Boar>() != null) return false;
        return isDietaryFitting(g);
    }
}
