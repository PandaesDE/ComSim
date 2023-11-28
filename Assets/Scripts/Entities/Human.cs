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

public class Human : Creature
{
    protected override void Awake()
    {
        base.Awake();

        build_Gender(Util.Random.Gender(this));
        build_Dietary(new Omnivore(this));
        build_Health(80);
        build_Weight(80);
        build_Speed(.2f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (statusManager.status == StatusManager.Status.GIVING_BIRTH) return;
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
        Human partner = c.gameObject.GetComponent<Human>();
        if (partner == null) return false;
        return true;
    }

    protected override void giveBirth()
    {
        SpawnOptions options = new SpawnOptions()
            .set_Amount(1)
            .set_Position(gameObject.transform.position);
        Spawner.spawnHumans(options);
    }
}
