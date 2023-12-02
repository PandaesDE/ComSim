/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Omnivore, Strong Creature
 *  
 *  References:
 *      Scene: 
 *          - Boar GameObject
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

public class Boar : Creature
{
    protected override void Awake()
    {
        base.Awake();

        BuildGender(Util.Random.Gender(this));
        BuildDietary(new Omnivore(this));
        BuildHealth(150);
        BuildWeight(130);
        BuildSpeed(.2f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (StatusManager.Status != StatusManager.State.sleeping)
        {
            Movement.MoveToTarget();
            EvaluateVision();
            MakeStatusBasedMove();
        }
    }

    /*Gets called by Parent*/
    protected override bool IsSameSpecies(Creature c)
    {
        if (!c.TryGetComponent<Boar>(out _)) return false;
        return true;
    }

    protected override void GiveBirth()
    {
        SpawnOptions options = new SpawnOptions()
            .SetAmount(1)
            .SetPosition(gameObject.transform.position);
        Spawner.SpawnBoars(options);
    }

    protected override void OnDeath(DeathReason dr)
    {
        Spawner.MakeCorpse(this);
        Statistics.incrementBoarDeathReason(dr);
    }
}
