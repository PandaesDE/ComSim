/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Omnivore, Social Creature
 *  
 *  References:
 *      Scene: Human GameObjects
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

public class Human : Creature
{
    protected override void Awake()
    {
        base.Awake();

        BuildGender(Util.Random.IsMale());
        BuildDietary(new Omnivore(this));
        BuildHealth(80);
        BuildWeight(80);
        BuildSpeed(.2f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (StatusManager.Status == StatusManager.State.giving_birth) return;
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
        if (!c.TryGetComponent<Human>(out _)) return false;
        return true;
    }

    protected override void GiveBirth()
    {
        int amount = Util.Random.Int(1, 2);
        SpawnOptions options = new SpawnOptions()
            .SetAmount(amount)
            .SetPosition(gameObject.transform.position);
        Spawner.SpawnHumans(options);
    }

    protected override void OnDeath(DeathReason dr)
    {
        Spawner.MakeCorpse(this);
        Statistics.IncrementHumanDeathReason(dr);
    }

    public override Creature BuildGender(bool isMale)
    {
        if (isMale)
        {
            int matingCooldown = 1 * Gamevariables.MINUTES_PER_HOUR * Gamevariables.HOURS_PER_DAY;
            this.Gender = new Male(matingCooldown);
        }
        else
        {
            int durationPregnancy = (int)(1 * (float)Gamevariables.HUMAN_PREGNANCY_TIME_DAYS * (float)Gamevariables.MINUTES_PER_HOUR * (float)Gamevariables.HOURS_PER_DAY);
            int cooldownPregnancy = durationPregnancy / 2;
            this.Gender = new Female(this, cooldownPregnancy, durationPregnancy);
        }
        return this;
    }
}
