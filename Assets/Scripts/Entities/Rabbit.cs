/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Herbivore Creature
 *  
 *  References:
 *      Scene: Rabbit GameObjects
 *          - 
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public class Rabbit : Creature
{
    [SerializeField] private Sprite spr_General;        //TODO DELETE?

    protected override void Awake()
    {
        base.Awake();

        BuildGender(Util.Random.Gender(this));
        BuildDietary(new Herbivore(this));
        BuildHealth(45);
        BuildWeight(30);
        BuildSpeed(.2f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (StatusManager.status != StatusManager.Status.sleeping)
        {
            Movement.MoveToTarget();
            EvaluateVision();
            MakeStatusBasedMove();
        }
    }

    /*Gets called by Parent*/
    protected override bool IsSameSpecies(Creature c)
    {
        Rabbit partner = c.gameObject.GetComponent<Rabbit>();   //TODO TryGet
        if (partner == null) return false;
        return true;
    }

    protected override void GiveBirth()
    {
        SpawnOptions options = new SpawnOptions()
            .SetAmount(1)
            .SetPosition(gameObject.transform.position);
        Spawner.SpawnRabbits(options);
    }
}
