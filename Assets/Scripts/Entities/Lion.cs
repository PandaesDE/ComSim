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
    [SerializeField] private Sprite spr_General;

    protected override void Awake()
    {
        base.Awake();

        build_Gender(Util.Random.Gender(this));
        build_Dietary(new Carnivore(this));
        build_Health(10);
        build_Weight(80);
        build_Speed(.2f);
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
        Lion partner = c.gameObject.GetComponent<Lion>();
        if (partner == null) return false;
        return true;
    }

    protected override void giveBirth()
    {
        SpawnOptions options = new SpawnOptions()
            .set_Amount(1)
            .set_Position(gameObject.transform.position);
        Spawner.spawnLions(options);
    }
}
